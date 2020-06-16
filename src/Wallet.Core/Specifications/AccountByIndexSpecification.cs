using Wallet.Core.Entities;

namespace Wallet.Core.Specifications
{
  public class AccountByIndexSpecification : BaseSpecification<Account>
  {
    public AccountByIndexSpecification(int index) : base(a => a.AccountIndex == index) { }
  }
}