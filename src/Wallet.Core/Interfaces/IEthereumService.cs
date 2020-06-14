using System.Threading.Tasks;

namespace Wallet.Core.Interfaces
{
  public interface IEthereumService
  {
    Task<decimal> GetBalanceAsync(string address);
    Task<string> CreateTransactionAsync(int accountIndex, int addressIndex, string recipient, decimal amountEth);
  }
}