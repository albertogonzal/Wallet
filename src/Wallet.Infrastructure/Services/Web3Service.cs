using System.Numerics;
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
using Nethereum.Util;

namespace Wallet.Infrastructure.Services
{
  public class Web3Service : IEthereumService
  {
    private readonly Web3 _web3;
    private readonly IOptions<WalletOptions> _options;
    public Web3Service(IOptions<WalletOptions> options)
    {
      _options = options;
      _web3 = Web3Client(0, 0);
    }

    public async Task<string> GetBalanceAsync(string address)
    {
      var balance = await _web3.Eth.GetBalance.SendRequestAsync(address);
      return balance.Value.ToString();
    }

    public async Task<string> CreateTransactionAsync(int accountIndex, int addressIndex, string recipient, string amount)
    {
      Web3 txWeb3 = Web3Client(accountIndex, addressIndex);
      var balanceInEther = Web3.Convert.FromWei(BigInteger.Parse(amount));

      var gasPrice = Web3.Convert.ToWei(25, UnitConversion.EthUnit.Gwei);
      var fee = new BigInteger(21000) * gasPrice;

      var balanceInEtherMinusFee = balanceInEther - Web3.Convert.FromWei(fee);
      if (balanceInEtherMinusFee < 0)
      {
        return $"{txWeb3.Eth.Accounts.ToString()} {accountIndex} {addressIndex}";
      }

      var txReceipt = await txWeb3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(recipient, balanceInEtherMinusFee);
      return txReceipt.TransactionHash;
    }

    private Web3 Web3Client(int accountIndex, int addressIndex)
    {
      var ethEcKey = EthereumHelper.GetEthECKey(accountIndex, addressIndex, _options.Value.Seed);
      var account = new Nethereum.Web3.Accounts.Account(ethEcKey);
      var clientUri = _options.Value.Network == "mainnet" ? _options.Value.MainNetUri : _options.Value.TestNetUri;
      var client = new Nethereum.JsonRpc.Client.RpcClient(new Uri(clientUri));

      return new Web3(account, client);
    }
  }
}