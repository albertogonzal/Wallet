using System.Numerics;
namespace Wallet.Core.Options
{
  public class TransactionOptions
  {
    public decimal MinimumDeposit { get; set; }
    public decimal GasPrice { get; set; }
    public decimal Gas { get; set; }
  }
}