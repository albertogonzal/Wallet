namespace Wallet.Core.Entities
{
  public class Asset : BaseEntity
  {
    public Asset(string name)
    {
      Name = name;
    }

    public string Name { get; private set; }
  }
}