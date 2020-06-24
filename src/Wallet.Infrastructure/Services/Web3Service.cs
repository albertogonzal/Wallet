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
using Nethereum.Util;
using Wallet.Core.Entities;
using Nethereum.RPC.TransactionManagers;
using Wallet.Core.Specifications;
using Nethereum.StandardTokenEIP20;
using Nethereum.ABI.FunctionEncoding.Attributes;
// using Nethereum.StandardTokenEIP20.Functions;
using Nethereum.Contracts.CQS;

namespace Wallet.Infrastructure.Services
{
  public class Web3Service : IEthereumService
  {
    private readonly Web3 _web3;
    private readonly IOptions<WalletOptions> _options;
    private readonly IOptions<Core.Options.TransactionOptions> _txOptions;
    private readonly IAsyncRepository<Wallet.Core.Entities.Transaction> _repository;
    private readonly IAsyncRepository<Balance> _balanceRepository;
    private readonly IAsyncRepository<Core.Entities.Account> _accountRepository;

    public Web3Service(IAsyncRepository<Wallet.Core.Entities.Transaction> repository, IAsyncRepository<Balance> balanceRepository, IAsyncRepository<Core.Entities.Account> accountRepository, IOptions<WalletOptions> options, IOptions<Core.Options.TransactionOptions> txOptions)
    {
      _accountRepository = accountRepository;
      _balanceRepository = balanceRepository;
      _repository = repository;
      _options = options;
      _txOptions = txOptions;
      _web3 = Web3Client();
    }

    public async Task<decimal> GetBalanceAsync(string address, string contractAddress)
    {
      var tokenService = new StandardTokenService(_web3, contractAddress);
      var balanceWei = await _web3.Eth.GetBalance.SendRequestAsync(address);
      // var balance = await tokenService.GetBalanceOfAsync<BigInteger>(address);
      return Web3.Convert.FromWei(balanceWei);
    }

    public async Task<Wallet.Core.Entities.Transaction> CreateTransactionAsync(int accountIndex, int addressIndex, string recipient, decimal amountEth)
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
          ? TransactionType.FromName<TransactionType>("withdraw").Name
          : TransactionType.FromName<TransactionType>("deposit").Name;

        var transactionStatus = TransactionStatus.FromName<TransactionStatus>("pending").Name;

        var spec = new AccountByIndexSpecification(accountIndex);
        var accountEntity = await _accountRepository.FirstOrDefaultAsync(spec);
        var transaction = new Wallet.Core.Entities.Transaction(transactionType, transactionStatus, txHash, sender, recipient, amountEth, Guid.Empty, accountEntity.UserId);
        await _repository.AddAsync(transaction);

        return transaction;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"accountIndex: {accountIndex}, addressIndex: {addressIndex}, {ex.ToString()}");
        return null;
      }
    }

    public async Task VerifyTransactionAsync(Wallet.Core.Entities.Transaction transaction)
    {
      Web3 txWeb3 = Web3Client();

      var liveTransaction = await txWeb3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(transaction.TransactionHash);
      var block = await txWeb3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
      var confirmations = block.Value - liveTransaction.BlockNumber.Value;

      if (confirmations > 12)
      {
        transaction.UpdateStatus(TransactionStatus.FromName<TransactionStatus>("completed"));
        await _repository.UpdateAsync(transaction);

        var spec = new BalanceByUserIdSpecification(transaction.UserId);
        var balance = await _balanceRepository.FirstOrDefaultAsync(spec);
        if (balance == null)
        {
          balance = new Balance(transaction.UserId, transaction.Amount);
          await _balanceRepository.AddAsync(balance);
        }
        else
        {
          balance.Deposit(transaction.Amount);
          await _balanceRepository.UpdateAsync(balance);
        }
      }
    }

    public string GetEthAddress(int accountIndex, int addressIndex)
    {
      return GetEthECKey(accountIndex, addressIndex).GetPublicAddress();
    }

    private Web3 Web3Client(int accountIndex, int addressIndex)
    {
      var ethEcKey = GetEthECKey(accountIndex, addressIndex);
      var account = new Nethereum.Web3.Accounts.Account(ethEcKey);
      var clientUri = _options.Value.Network == "mainnet" ? _options.Value.MainNetUri : _options.Value.TestNetUri;
      var client = new Nethereum.JsonRpc.Client.RpcClient(new Uri(clientUri));

      return new Web3(account, client);
    }

    private Web3 Web3Client()
    {
      return Web3Client(0, 0);
    }

    private EthECKey GetEthECKey(int accountIndex, int addressIndex)
    {
      var masterKey = new ExtKey(_options.Value.Seed);

      string keyPathString = $"m/44'/60'/{accountIndex}'/0/{addressIndex}";
      var keyPath = new NBitcoin.KeyPath(keyPathString);

      var privateKey = masterKey.Derive(keyPath).PrivateKey.ToBytes();
      var ethEcKey = new EthECKey(privateKey, true);

      return ethEcKey;
    }
  }
}