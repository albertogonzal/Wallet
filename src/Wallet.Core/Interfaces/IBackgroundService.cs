using System.Collections.Generic;
using System.Threading.Tasks;
namespace Wallet.Core.Interfaces
{
  public interface IBackgroundService
  {
    Task<List<string>> Transfer();
  }
}