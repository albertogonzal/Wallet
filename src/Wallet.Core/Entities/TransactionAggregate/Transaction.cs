using Wallet.Core.Interfaces;

namespace Wallet.Core.Entities
{
  public class Transaction : BaseEntity, IAggregateRoot
  {
    public Transaction(int typeValue, string transactionHash, string sender, string recipient, int status)
    {
      Type = TransactionType.FromValue<TransactionType>(typeValue);
      TransactionHash = transactionHash;
      Sender = sender;
      Recipient = recipient;
      Status = status;
    }

    public TransactionType Type { get; private set; }
    public string TransactionHash { get; private set; }
    public string Sender { get; private set; }
    public string Recipient { get; private set; }
    public int Status { get; private set; }

  }
}