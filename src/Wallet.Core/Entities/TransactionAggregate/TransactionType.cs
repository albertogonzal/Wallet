namespace Wallet.Core.Entities
{
  public class TransactionType : Enumeration
  {
    public static readonly TransactionType Deposit = new TransactionType(0, "deposit");
    public static readonly TransactionType Withdraw = new TransactionType(1, "withdraw");

    private TransactionType(int value, string name) : base(value, name) { }
  }
}