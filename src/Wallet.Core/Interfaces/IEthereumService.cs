using System.Threading.Tasks;

namespace Wallet.Core.Interfaces
{
  public interface IEthereumService
  {
    Task<string> GetBalanceAsync(string address);

  }
}