using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NBitcoin;
using Nethereum.JsonRpc.Client;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Wallet.Core;
using Wallet.Core.Interfaces;
using Wallet.Infrastructure.Helpers;

namespace Wallet.Infrastructure.Services
{
  public class Web3Service : IEthereumService
  {
    private readonly Web3 _web3;
    private readonly IOptions<WalletOptions> _options;
    private readonly IAccountService _accountService;
    public Web3Service(IOptions<WalletOptions> options, IAccountService accountService)
    {
      _options = options;
      _accountService = accountService;

      _web3 = Web3Client();
    }

    public async Task<string> GetBalanceAsync(string address)
    {
      var balance = await _web3.Eth.GetBalance.SendRequestAsync(address);
      return balance.Value.ToString();
    }

    private Web3 Web3Client()
    {
      var ethEcKey = EthereumHelper.GetEthECKey(0, 0, _options.Value.Seed);
      var account = new Nethereum.Web3.Accounts.Account(ethEcKey);
      var clientUri = _options.Value.Network == "mainnet" ? _options.Value.MainNetUri : _options.Value.TestNetUri;
      var client = new Nethereum.JsonRpc.Client.RpcClient(new Uri(clientUri));
      clientUri = "https://ropsten.infura.io/v3/9f381e87622d453fbc1c6f312b92527e";

      return new Web3(account, client);
    }
  }
}