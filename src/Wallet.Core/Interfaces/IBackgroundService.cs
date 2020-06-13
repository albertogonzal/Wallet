using System.Threading.Tasks;
namespace Wallet.Core.Interfaces
{
  public interface IBackgroundService
  {
    Task Transfer();
  }
}