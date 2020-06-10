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

namespace Wallet.Infrastructure.Services
{
  public class NethereumAccountService : IAccountService
  {
    private readonly string _seed = "16236c2028fd2018eb7049825e6b4f0191de4dbff003579918de7b7348ff06ac";
    private readonly IAsyncRepository<Core.Entities.Account> _repository;

    public NethereumAccountService(IAsyncRepository<Core.Entities.Account> repository)
    {
      _repository = repository;
    }

    public async Task<Address> NewAddress(Guid accountId, Guid assetId)
    {
      // Guid accountId = new Guid("61c676f4-11ff-4998-be0f-57561ebe8a57");
      // Guid assetId = new Guid("8d57d4eb-314c-4c52-bc4e-88a5631aca35");
      // Guid userId = new Guid("2c241706-e00d-4d7f-bc18-ad4df5a5b2ec");

      var account = await _repository.GetByIdAsync(accountId);

      int accountIndex = account.AccountIndex;
      int addressIndex = account.Addresses.Where(a => a.AssetId == assetId).Count();

      var masterKey = new ExtKey(_seed);

      string keyPathString = $"m/44'/60'/{accountIndex}'/0/{addressIndex}";
      var keyPath = new NBitcoin.KeyPath(keyPathString);

      var privateKey = masterKey.Derive(keyPath).PrivateKey.ToBytes();

      var ethEcKey = new EthECKey(privateKey, true);
      var ethPrivateKey = ethEcKey.GetPrivateKeyAsBytes().ToHex();

      var newAddress = new Address(accountId, assetId, addressIndex);
      account.AddAddress(newAddress);

      await _repository.UpdateAsync(account);

      return newAddress;
    }
  }
}