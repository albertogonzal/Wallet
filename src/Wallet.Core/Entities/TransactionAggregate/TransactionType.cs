namespace Wallet.Core.Entities
{
  public class TransactionType : Enumeration
  {
    public static readonly TransactionType Deposit = new TransactionType(0, "Deposit");
    public static readonly TransactionType Withdraw = new TransactionType(1, "Withdraw");

    private TransactionType(int id, string name) : base(id, name) { }
  }
}