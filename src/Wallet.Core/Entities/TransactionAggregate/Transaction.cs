using System;
using Wallet.Core.Interfaces;

namespace Wallet.Core.Entities
{
  public class Transaction : BaseEntity, IAggregateRoot
  {
    private string _type;
    private string _status;
    public Transaction(string type, string status, string transactionHash, string sender, string recipient, decimal amount, Guid userId)
    {
      _type = type;
      _status = status;
      TransactionHash = transactionHash;
      Sender = sender;
      Recipient = recipient;
      Amount = amount;
      UserId = userId;
    }

    public TransactionType Type => TransactionType.FromName<TransactionType>(_type);
    public TransactionStatus Status => TransactionStatus.FromName<TransactionStatus>(_status);
    public string TransactionHash { get; private set; }
    public string Sender { get; private set; }
    public string Recipient { get; private set; }
    public decimal Amount { get; private set; }
    public Guid UserId { get; private set; }

    public void UpdateStatus(TransactionStatus status)
    {
      _status = status.Name;
    }
  }
}