using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Wallet.Core;
using Wallet.Core.Entities;
using Wallet.Core.Interfaces;
using Wallet.Core.Specifications;

namespace Wallet.Infrastructure.Services
{
  public class HangfireService : IBackgroundService
  {
    private readonly IAccountService _accountService;
    private readonly IEthereumService _ethService;
    private readonly IAsyncRepository<Account> _repository;
    private readonly IOptions<WalletOptions> _options;

    public HangfireService(IAccountService accountService, IEthereumService ethService, IAsyncRepository<Account> repository, IOptions<WalletOptions> options)
    {
      _accountService = accountService;
      _ethService = ethService;
      _repository = repository;
      _options = options;
    }

    public async Task<List<string>> Transfer()
    {
      List<string> txHashes = new List<string>();
      Dictionary<int, List<(int, string)>> addresses = new Dictionary<int, List<(int, string)>>();
      var spec = new AccountWithAddressesSpecification();
      var allAccounts = await _repository.ListAsync(spec);
      var accounts = allAccounts.Where(a => a.AccountIndex != 0).ToList();
      var adminAddress = allAccounts.Where(a => a.AccountIndex == 0).First().Addresses.First().PublicAddress;

      foreach (var account in accounts)
      {
        foreach (var address in account.Addresses)
        {
          string balance = await _ethService.GetBalanceAsync(address.PublicAddress);
          if (balance != "0")
          {
            int accountIndex = account.AccountIndex;
            int addressIndex = address.AddressIndex;
            if (!addresses.ContainsKey(accountIndex))
            {
              addresses.Add(accountIndex, new List<(int AddressIndex, string Balance)>());
            }
            addresses[accountIndex].Add((addressIndex, balance));

            string txHash = await _ethService.CreateTransactionAsync(accountIndex, addressIndex, adminAddress, balance);
            txHashes.Add(txHash);
          }
        }
      }

      return txHashes;
    }
  }
}