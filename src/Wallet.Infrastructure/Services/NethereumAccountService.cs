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
    // Move to secure location
    private readonly string _words = "ripple scissors kick mammal hire column oak again sun offer wealth tomorrow wagon turn fatal";
    private readonly string _password = "password";

    public NethereumAccountService()
    {
    }

    public async Task<Address> NewAddress(Guid userId, Core.Entities.Asset asset)
    {
      // Get Account from repository
      var account = new Wallet.Core.Entities.Account(0);
      int accountIndex = account.AccountIndex;
      int addressIndex = account.Addresses.Count;

      // m / purpose' / coin_type' / account' / chain / address_index
      var masterKey = new ExtKey(_words);
      string keyPathString = $"m/44'/60'/{accountIndex}'/0/{addressIndex}";
      var keyPath = new NBitcoin.KeyPath(keyPathString);

      // get masterPubKey
      var masterPubKey = masterKey.Derive(keyPath).Neuter();

      // get privatekey
      // var privateKey0 = masterKey.Derive(keyPath).PrivateKey.ToBytes();
      var privateKey0 = masterKey.Derive(keyPath).PrivateKey.ToString();
      // get pubkey
      var pubKey0 = masterPubKey.PubKey.Compress(false);


      // Get ethereum EC Key
      var ethEcKey = new EthECKey(privateKey0);

      // Eth format private key
      var privateKey = ethEcKey.GetPrivateKeyAsBytes().ToHex();
      var nAccount = new Nethereum.Web3.Accounts.Account(privateKey);

      var web3 = new Web3(nAccount);
      var balance = await web3.Eth.GetBalance.SendRequestAsync(nAccount.Address);

      var newAddress = new Address(asset, addressIndex, balance.ToString());
      account.AddAddress(newAddress);

      // Update repository

      return newAddress;
    }
  }
}