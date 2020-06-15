namespace Wallet.Core.Entities
{
  public class TransactionStatus : Enumeration
  {
    public static readonly TransactionStatus Pending = new TransactionStatus(0, "pending");
    public static readonly TransactionStatus Completed = new TransactionStatus(1, "completed");
    public static readonly TransactionStatus Rejected = new TransactionStatus(2, "rejected");

    private TransactionStatus(int value, string name) : base(value, name) { }
  }
}