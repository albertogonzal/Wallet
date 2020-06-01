using System.Threading.Tasks;

namespace Wallet.Core.Interfaces
{
  public interface IAccountService
  {
    Task NewAddress();
  }
}