using NBitcoin;
using Nethereum.Signer;

namespace Wallet.Infrastructure.Helpers
{
  public static class EthereumHelper
  {
    public static EthECKey GetEthECKey(int accountIndex, int addressIndex, string seed)
    {
      var masterKey = new ExtKey(seed);

      string keyPathString = $"m/44'/60'/{accountIndex}'/0/{addressIndex}";
      var keyPath = new NBitcoin.KeyPath(keyPathString);

      var privateKey = masterKey.Derive(keyPath).PrivateKey.ToBytes();
      var ethEcKey = new EthECKey(privateKey, true);

      return ethEcKey;
    }
  }
}