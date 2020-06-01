namespace Wallet.Core.Entities
{
  public class Address : BaseEntity
  {
    private readonly decimal _balance;
    private readonly int _accountIndex;

    public Address(decimal balance, int accountIndex)
    {
      _balance = balance;
      _accountIndex = accountIndex;
    }

    public void Send(Address recipientAddress)
    {
      // Send to recipient
    }
  }
}