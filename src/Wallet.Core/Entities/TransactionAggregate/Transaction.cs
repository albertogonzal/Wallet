using Wallet.Core.Interfaces;

namespace Wallet.Core.Entities
{
  public class Transaction : BaseEntity, IAggregateRoot
  {
    private readonly int _typeValue;
    private readonly int _statusValue;
    public Transaction(int typeValue, int statusValue, string transactionHash, string sender, string recipient, decimal amount)
    {
      _typeValue = typeValue;
      _statusValue = statusValue;
      TransactionHash = transactionHash;
      Sender = sender;
      Recipient = recipient;
      Amount = amount;
    }

    public TransactionType Type => TransactionType.FromValue<TransactionType>(_typeValue);
    public TransactionStatus Status => TransactionStatus.FromValue<TransactionStatus>(_statusValue);
    public string TransactionHash { get; private set; }
    public string Sender { get; private set; }
    public string Recipient { get; private set; }
    public decimal Amount { get; private set; }
  }
}