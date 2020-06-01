using System;

namespace Wallet.Core.Entities
{
  public class BaseEntity
  {
    public Guid Id { get; protected set; }
  }
}