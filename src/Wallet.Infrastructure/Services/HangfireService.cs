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
    private readonly IEthereumService _ethService;
    private readonly IAsyncRepository<Account> _repository;
    private readonly IAsyncRepository<Transaction> _txRepository;
    private readonly IAsyncRepository<Asset> _assetRepository;
    private readonly IOptions<TransactionOptions> _txOptions;

    public HangfireService(IEthereumService ethService, IAsyncRepository<Account> repository, IAsyncRepository<Transaction> txRepository, IAsyncRepository<Asset> assetRepository, IOptions<TransactionOptions> txOptions)
    {
      _ethService = ethService;
      _repository = repository;
      _txRepository = txRepository;
      _assetRepository = assetRepository;
      _txOptions = txOptions;
    }

    public async Task<List<string>> Transfer()
    {
      List<string> txHashes = new List<string>();
      var spec = new AccountWithAddressesSpecification();
      var allAccounts = await _repository.ListAsync(spec);
      var accounts = allAccounts.Where(a => a.AccountIndex != 0).ToList();
      var adminAddress = allAccounts.Where(a => a.AccountIndex == 0).Single().Addresses.First().PublicAddress;

      var assets = await _assetRepository.ListAsync();

      foreach (var account in accounts)
      {
        foreach (var address in account.Addresses)
        {
          foreach (var asset in assets)
          {
            decimal balance = await _ethService.GetBalanceAsync(address.PublicAddress, asset.ContractAddress);
            if (balance > _txOptions.Value.MinimumDeposit)
            {
              int accountIndex = account.AccountIndex;
              int addressIndex = address.AddressIndex;

              var transaction = await _ethService.CreateTransactionAsync(accountIndex, addressIndex, adminAddress, balance, asset);
              txHashes.Add(transaction.TransactionHash);
            }

            if (balance > 0m)
            {
              txHashes.Add($"Not enough balance: {asset.Name} : {address.PublicAddress} : {balance}");
            }
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
      var txPending = (await _txRepository.ListAsync()).Where(t => t.Status.Name == "pending" && t.Type.Name == "deposit").ToList();

      foreach (var tx in txPending)
      {
        await _ethService.VerifyTransactionAsync(tx);
      }
    }
  }
}