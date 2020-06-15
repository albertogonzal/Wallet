using Wallet.Core.Entities;

namespace Wallet.Core.Specifications
{
  public class TransactionsByStatusSpecification : BaseSpecification<Transaction>
  {
    // fix to work with ef core
    public TransactionsByStatusSpecification(TransactionStatus status) : base(t => t.Status == status) { }
  }
}