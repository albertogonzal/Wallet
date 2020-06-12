using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NBitcoin;
using Nethereum.Signer;
using Nethereum.Web3;
using Wallet.Core;
using Wallet.Core.Interfaces;

namespace Wallet.Infrastructure.Services
{
  public class Web3Service : IEthereumService
  {
    private readonly Web3 _web3;
    public Web3Service(IOptions<WalletOptions> options)
    {
      // remove duplicate code
      var masterKey = new ExtKey(options.Value.Seed);

      string keyPathString = $"m/44'/60'/0'/0/0";
      var keyPath = new NBitcoin.KeyPath(keyPathString);

      var privateKey = masterKey.Derive(keyPath).PrivateKey.ToBytes();
      var ethEcKey = new EthECKey(privateKey, true);

      var account = new Nethereum.Web3.Accounts.Account(ethEcKey);
      Uri baseUrl = new Uri("https://mainnet.infura.io/v3/9f381e87622d453fbc1c6f312b92527e");
      var client = new Nethereum.JsonRpc.Client.RpcClient(baseUrl);
      _web3 = new Web3(account, client);
    }
    public async Task<string> GetBalanceAsync(string address)
    {
      var balance = await _web3.Eth.GetBalance.SendRequestAsync(address);
      return balance.Value.ToString();
    }
  }
}