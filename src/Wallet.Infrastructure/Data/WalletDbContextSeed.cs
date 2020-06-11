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
    public static async Task SeedAsync(IAsyncRepository<Account> accountRepository, IAsyncRepository<Asset> assetRepository, IAccountService service, UserManager<ApplicationUser> userManager)
    {
      var admin = await userManager.FindByNameAsync("admin@wallet.com");
      var adminId = new Guid(admin.Id);

      var account = (await accountRepository.ListAsync()).Where(a => a.UserId == adminId).FirstOrDefault();
      if (account == null)
      {
        account = new Account(adminId, 0);
        await accountRepository.AddAsync(account);
      }

      var asset = (await assetRepository.ListAsync()).Where(a => a.Name == "ETH").FirstOrDefault();
      if (asset == null)
      {
        asset = new Asset("ETH");
        await assetRepository.AddAsync(asset);
      }
    }
  }
}