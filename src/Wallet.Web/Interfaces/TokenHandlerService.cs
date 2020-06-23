using Microsoft.AspNetCore.Identity;
using Wallet.Infrastructure.Identity;

namespace Wallet.Web.Services
{
  public class TokenHandlerService
  {
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenHandlerService(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }


  }
}