using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wallet.Core.Entities;
using Wallet.Core.Options;
using Wallet.Core.Interfaces;
using Wallet.Infrastructure.Data;
using Wallet.Infrastructure.Identity;
using Wallet.Web.Options;
using Wallet.Infrastructure.Services;
using Wallet.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Wallet.Web
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<WalletOptions>(Configuration.GetSection("WalletOptions"));
      services.Configure<TransactionOptions>(Configuration.GetSection("TransactionOptions"));
      services.Configure<JwtOptions>(Configuration.GetSection("JwtOptions"));

      services.AddDbContext<WalletDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("WalletDb")));
      services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityDb")));

      services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
      services.Configure<IdentityOptions>(options =>
      {
        options.Password.RequiredLength = 4;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
      });

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(config =>
      {
        config.RequireHttpsMetadata = false;
        config.SaveToken = true;
        config.TokenValidationParameters = new TokenValidationParameters
        {
          ValidIssuer = Configuration["JwtOptions:Issuer"],
          ValidAudience = Configuration["JwtOptions:Issuer"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtOptions:Key"].ToString())),
          ClockSkew = TimeSpan.Zero
        };
      });
      services.AddAuthorization();

      services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(Configuration.GetConnectionString("HangfireDb"), new SqlServerStorageOptions
        {
          CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
          SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
          QueuePollInterval = TimeSpan.Zero,
          UseRecommendedIsolationLevel = true,
          UsePageLocksOnDequeue = true,
          DisableGlobalLocks = true
        }));
      services.AddHangfireServer();

      services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
      services.AddScoped<IBalanceRepository, BalanceRepository>();
      services.AddScoped<IAccountService, AccountService>();
      services.AddScoped<IBalanceService, BalanceService>();
      services.AddScoped<IEthereumService, Web3Service>();
      services.AddScoped<IBackgroundService, HangfireService>();
      services.AddControllers();

      services.AddCors(options =>
      {
        options.AddPolicy("AllowAllCors", builder =>
        {
          builder.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .SetIsOriginAllowed(requestingOrigin => true)
            .Build();
        });
      });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseCors("AllowAllCors");
      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseHangfireDashboard();
      RecurringJob.AddOrUpdate<IBackgroundService>(service => service.Transfer(), () => "*/5 * * * *");
      RecurringJob.AddOrUpdate<IBackgroundService>(service => service.Credit(), () => "*/5 * * * *");

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
