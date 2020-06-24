using Wallet.Core.Interfaces;

namespace Wallet.Core.Entities
{
  public class Asset : BaseEntity, IAggregateRoot
  {
    public Asset(string name, string symbol, string contractAddress, int decimalPlaces)
    {
      Name = name;
      Symbol = symbol;
      ContractAddress = contractAddress;
      DecimalPlaces = decimalPlaces;
    }

    public string Name { get; private set; }
    public string Symbol { get; private set; }
    public string ContractAddress { get; private set; }
    public int DecimalPlaces { get; private set; }
  }
}