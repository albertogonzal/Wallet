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

    public async Task Transfer()
    {
      Dictionary<int, List<int>> addresses = new Dictionary<int, List<int>>();
      var spec = new AccountWithAddressesSpecification();
      var accounts = await _repository.ListAsync(spec);

      foreach (var account in accounts)
      {
        foreach (var address in account.Addresses)
        {
          string balance = await _ethService.GetBalanceAsync(address.PublicAddress);
          if (balance != "0")
          {
            int accountIndex = account.AccountIndex;
            if (!addresses.ContainsKey(accountIndex))
            {
              addresses.Add(accountIndex, new List<int>());
            }
            addresses[accountIndex].Add(address.AddressIndex);
          }
        }
      }
    }
  }
}