using System.Threading.Tasks;
using Wallet.Core.Entities;

namespace Wallet.Core.Interfaces
{
  public interface IEthereumService
  {
    Task<decimal> GetBalanceAsync(string address, string contractAddress);
    Task<Transaction> CreateTransactionAsync(int accountIndex, int addressIndex, string recipient, decimal amountEth, Asset asset);
    Task VerifyTransactionAsync(Transaction transaction);
    string GetEthAddress(int accountIndex, int addressIndex);
  }
}