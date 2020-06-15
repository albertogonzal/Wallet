using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Wallet.Infrastructure.Identity
{
  public static class AppIdentityDbContextSeed
  {
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      await roleManager.CreateAsync(new IdentityRole("admin"));

      string adminUserName = "admin@wallet.com";
      var admin = new ApplicationUser { UserName = adminUserName, Email = adminUserName };
      await userManager.CreateAsync(admin, "PAssword12!@");

      admin = await userManager.FindByNameAsync(adminUserName);
      await userManager.AddToRoleAsync(admin, "admin");
    }
  }
}