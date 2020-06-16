using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Wallet.Core.Options;
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
    private readonly IAsyncRepository<Transaction> _txRepository;
    private readonly IOptions<WalletOptions> _options;
    private readonly IOptions<TransactionOptions> _txOptions;

    public HangfireService(IAccountService accountService, IEthereumService ethService, IAsyncRepository<Account> repository, IAsyncRepository<Transaction> txRepository, IOptions<WalletOptions> options, IOptions<TransactionOptions> txOptions)
    {
      _accountService = accountService;
      _ethService = ethService;
      _repository = repository;
      _txRepository = txRepository;
      _options = options;
      _txOptions = txOptions;
    }

    public async Task<List<string>> Transfer()
    {
      List<string> txHashes = new List<string>();
      var spec = new AccountWithAddressesSpecification();
      var allAccounts = await _repository.ListAsync(spec);
      var accounts = allAccounts.Where(a => a.AccountIndex != 0).ToList();
      var adminAddress = allAccounts.Where(a => a.AccountIndex == 0).First().Addresses.First().PublicAddress;

      foreach (var account in accounts)
      {
        foreach (var address in account.Addresses)
        {
          decimal balanceEth = await _ethService.GetBalanceAsync(address.PublicAddress);
          if (balanceEth > _txOptions.Value.MinimumDeposit)
          {
            int accountIndex = account.AccountIndex;
            int addressIndex = address.AddressIndex;

            string txHash = await _ethService.CreateTransactionAsync(accountIndex, addressIndex, adminAddress, balanceEth);
            txHashes.Add(txHash);
          }
          if (balanceEth > 0m)
          {
            txHashes.Add($"Not enough balance: {address.PublicAddress} : {balanceEth}");
          }
        }
      }

      return txHashes;
    }

    public async Task Credit()
    {
      // fix iqueryable to run on sqlserver using specification
      // var spec = new TransactionsByStatusSpecification(TransactionStatus.FromName<TransactionStatus>("pending"));
      // var txPending = await _txRepository.ListAsync(spec);
      var txPending = (await _txRepository.ListAsync()).Where(t => t.Status.Name == "pending").ToList();

      // get confirmed deposits from web3
      // web3 list tx

      // foreach uncredited check if deposit is confirmed

      // mark as credited, sender's account
      //
    }
  }
}