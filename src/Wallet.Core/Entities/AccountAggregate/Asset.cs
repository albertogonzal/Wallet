using Wallet.Core.Interfaces;

namespace Wallet.Core.Entities
{
  public class Asset : BaseEntity, IAggregateRoot
  {
    public Asset(string name)
    {
      Name = name;
    }

    public string Name { get; private set; }
  }
}