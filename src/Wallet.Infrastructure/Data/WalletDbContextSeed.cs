using System.Reflection.Emit;
using System;
using Microsoft.AspNetCore.Identity;
using Wallet.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Wallet.Core.Interfaces;
using Wallet.Infrastructure.Identity;
using System.Threading.Tasks;
using System.Linq;

namespace Wallet.Infrastructure.Data
{
  public static class WalletDbContextSeed
  {
    public static async Task SeedAsync(WalletDbContext context, AppIdentityDbContext identityContext)
    {
      if (!context.Accounts.Any())
      {
        var adminRole = await identityContext.Roles.Where(r => r.Name == "admin").FirstOrDefaultAsync();
        var adminUserRole = await identityContext.UserRoles.Where(r => r.RoleId == adminRole.Id).FirstOrDefaultAsync();
        var admin = await identityContext.Users.Where(u => u.Id == adminUserRole.UserId).FirstOrDefaultAsync();

        if (admin != null)
        {
          var account = new Account(new Guid(admin.Id), 0);
          await context.Accounts.AddAsync(account);
          await context.SaveChangesAsync();
        }
      }

      if (!context.TransactionTypes.Any())
      {
        var transactionTypes = TransactionType.GetAll<TransactionType>();
        await context.TransactionTypes.AddRangeAsync(transactionTypes);
        await context.SaveChangesAsync();
      }
    }
  }
}