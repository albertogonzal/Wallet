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

namespace Wallet.Infrastructure.Services
{
  public class NethereumAccountService : IAccountService
  {
    private readonly IAsyncRepository<Core.Entities.Account> _repository;
    private readonly IOptions<WalletOptions> _options;

    public NethereumAccountService(IAsyncRepository<Core.Entities.Account> repository, IOptions<WalletOptions> options)
    {
      _repository = repository;
      _options = options;
    }

    public async Task<Address> NewAddress(Guid accountId, Guid assetId)
    {
      var accountSpec = new AccountWithAddressesSpecification(accountId);
      var account = await _repository.FirstOrDefaultAsync(accountSpec);

      int accountIndex = account.AccountIndex;
      int addressIndex = account.Addresses.Where(a => a.AssetId == assetId).Count();

      var masterKey = new ExtKey(_options.Value.Seed);

      string keyPathString = $"m/44'/60'/{accountIndex}'/0/{addressIndex}";
      var keyPath = new NBitcoin.KeyPath(keyPathString);

      var privateKey = masterKey.Derive(keyPath).PrivateKey.ToBytes();

      var ethEcKey = new EthECKey(privateKey, true);
      string publicAddress = ethEcKey.GetPublicAddress();

      var newAddress = new Address(accountId, assetId, addressIndex, publicAddress);
      account.AddAddress(newAddress);

      await _repository.UpdateAsync(account);

      return newAddress;
    }
  }
}