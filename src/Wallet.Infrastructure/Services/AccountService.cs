using System.Linq;
using System.Text;
using NBitcoin;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Util;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.HdWallet;
using System;
using Wallet.Core.Interfaces;
using System.Threading.Tasks;
using Wallet.Core.Entities;
using Nethereum.Signer;
using Wallet.Core.Specifications;
using Microsoft.Extensions.Options;
using Wallet.Core;
using Wallet.Infrastructure.Helpers;

namespace Wallet.Infrastructure.Services
{
  public class AccountService : IAccountService
  {
    private readonly IAsyncRepository<Core.Entities.Account> _repository;
    private readonly IOptions<WalletOptions> _options;

    public AccountService(IAsyncRepository<Core.Entities.Account> repository, IOptions<WalletOptions> options)
    {
      _repository = repository;
      _options = options;
    }

    public async Task NewAccount(Guid userId)
    {
      var accounts = await _repository.ListAsync();
      if (accounts.Where(a => a.UserId == userId) == null)
      {
        int accountIndex = accounts.Count;
        var account = new Core.Entities.Account(userId, accountIndex);

        await _repository.AddAsync(account);
      }
    }

    public async Task<Address> NewAddress(Guid accountId, Guid assetId)
    {
      var accountSpec = new AccountWithAddressesSpecification(accountId);
      var account = await _repository.FirstOrDefaultAsync(accountSpec);

      int accountIndex = account.AccountIndex;
      int addressIndex = account.Addresses.Where(a => a.AssetId == assetId).Count();

      var ethEcKey = EthereumHelper.GetEthECKey(accountIndex, addressIndex, _options.Value.Seed);
      string publicAddress = ethEcKey.GetPublicAddress();

      var newAddress = new Address(accountId, assetId, addressIndex, publicAddress);
      account.AddAddress(newAddress);

      await _repository.UpdateAsync(account);

      return newAddress;
    }
  }
}