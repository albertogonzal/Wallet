using System.Linq.Expressions;
using System.Reflection;
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
using Wallet.Core.Options;
using Wallet.Core.Interfaces;
using Wallet.Infrastructure.Helpers;
using Nethereum.Util;
using Wallet.Core.Entities;
using Nethereum.RPC.TransactionManagers;

namespace Wallet.Infrastructure.Services
{
  public class Web3Service : IEthereumService
  {
    private readonly Web3 _web3;
    private readonly IOptions<WalletOptions> _options;
    private readonly IOptions<Core.Options.TransactionOptions> _txOptions;
    private readonly IAsyncRepository<Wallet.Core.Entities.Transaction> _repository;

    public Web3Service(IAsyncRepository<Wallet.Core.Entities.Transaction> repository, IOptions<WalletOptions> options, IOptions<Core.Options.TransactionOptions> txOptions)
    {
      _repository = repository;
      _options = options;
      _txOptions = txOptions;
      _web3 = Web3Client(0, 0);
    }

    public async Task<decimal> GetBalanceAsync(string address)
    {
      var balanceWei = await _web3.Eth.GetBalance.SendRequestAsync(address);
      return Web3.Convert.FromWei(balanceWei);
    }

    public async Task<string> CreateTransactionAsync(int accountIndex, int addressIndex, string recipient, decimal amountEth)
    {
      try
      {
        Web3 txWeb3 = Web3Client(accountIndex, addressIndex);

        decimal gasPriceGwei = _txOptions.Value.GasPrice;
        BigInteger gas = (BigInteger)_txOptions.Value.Gas;

        BigInteger gasPriceWei = Web3.Convert.ToWei(gasPriceGwei, UnitConversion.EthUnit.Gwei);
        BigInteger feeWei = gasPriceWei * gas;
        decimal feeEth = Web3.Convert.FromWei(feeWei);
        decimal amountToSendEth = amountEth - feeEth;

        var txReceipt = await txWeb3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(recipient, amountToSendEth, gasPriceGwei, gas);
        var txHash = txReceipt.TransactionHash;

        var sender = txWeb3.TransactionManager.Account.Address;
        var transactionType = accountIndex == 0 && addressIndex == 0
          ? TransactionType.FromName<TransactionType>("withdraw").Value
          : TransactionType.FromName<TransactionType>("deposit").Value;

        var transactionStatus = TransactionStatus.FromName<TransactionStatus>("pending").Value;

        var transaction = new Wallet.Core.Entities.Transaction(transactionType, transactionStatus, txHash, sender, recipient, amountToSendEth);
        await _repository.AddAsync(transaction);

        return txHash;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"accountIndex: {accountIndex}, addressIndex: {addressIndex}, {ex.ToString()}");
        return $"accountIndex: {accountIndex}, addressIndex: {addressIndex}, {ex.ToString()}";
      }
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