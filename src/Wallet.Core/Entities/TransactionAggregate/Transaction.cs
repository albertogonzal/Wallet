using Wallet.Core.Interfaces;

namespace Wallet.Core.Entities
{
  public class Transaction : BaseEntity, IAggregateRoot
  {
    private readonly int _typeValue;
    public Transaction(int typeValue, string transactionHash, string sender, string recipient, int status)
    {
      _typeValue = typeValue;
      TransactionHash = transactionHash;
      Sender = sender;
      Recipient = recipient;
      Status = status;
    }

    public TransactionType Type => TransactionType.FromValue<TransactionType>(_typeValue);
    public string TransactionHash { get; private set; }
    public string Sender { get; private set; }
    public string Recipient { get; private set; }
    public int Status { get; private set; }

  }
}