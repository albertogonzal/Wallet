using System;
using Wallet.Core.Entities;

namespace Wallet.Core.Specifications
{
  public class BalanceByUserIdSpecification : BaseSpecification<Balance>
  {
    public BalanceByUserIdSpecification(Guid userId) : base(b => b.UserId == userId) { }
  }
}