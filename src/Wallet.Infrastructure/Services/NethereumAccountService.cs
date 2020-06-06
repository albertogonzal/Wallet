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

    public async Task<Address> NewAddress(Core.Entities.Asset asset)
    {
      // remove userId param, use Identity
      // Get Account from repository
      var account = await _repository.GetByIdAsync(new Guid());
      int accountIndex = 45;
      int addressIndex = 12;

      // m / purpose' / coin_type' / account' / chain / address_index
      var masterKey = new ExtKey(_seed);
      string keyPathString = $"m/44'/60'/{accountIndex}'/0/{addressIndex}";
      var keyPath = new NBitcoin.KeyPath(keyPathString);

      // get masterPubKey
      var masterPubKey = masterKey.Derive(keyPath).Neuter();

      // get privatekey
      var privateKey0 = masterKey.Derive(keyPath).PrivateKey.ToBytes();

      // Get ethereum EC Key
      var ethEcKey = new EthECKey(privateKey0, true);

      // Eth format private key
      var privateKey = ethEcKey.GetPrivateKeyAsBytes().ToHex();

      var newAddress = new Address(Guid.NewGuid(), Guid.NewGuid(), addressIndex, "0");
      account.AddAddress(newAddress);

      // Update repository

      return newAddress;
    }
  }
}