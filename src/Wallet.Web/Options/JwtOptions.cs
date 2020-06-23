namespace Wallet.Web.Options
{
  public class JwtOptions
  {
    public string Key { get; set; }
    public string Issuer { get; set; }
    public int Expiration { get; set; }
    public int RefreshExpiration { get; set; }
  }
}