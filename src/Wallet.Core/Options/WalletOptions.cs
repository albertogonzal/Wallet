using System;
namespace Wallet.Core.Options
{
  public class WalletOptions
  {
    public string Seed { get; set; }
    public string Mnemonic { get; set; }
    public string Network { get; set; }
    public string MainNetUri { get; set; }
    public string TestNetUri { get; set; }
  }
}