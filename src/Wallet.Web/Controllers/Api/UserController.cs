using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wallet.Infrastructure.Identity;
using Wallet.Web.ViewModels;

namespace Wallet.Web.Controllers.Api
{
  public class UserController : BaseApiController
  {
    private readonly UserManager<ApplicationUser> __userManager;
    private readonly SignInManager<ApplicationUser> __signInManager;

    public UserController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
    {
      __signInManager = _signInManager;
      __userManager = _userManager;
    }

    // add jwt to signin
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
      try
      {
        ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        IdentityResult result = await __userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
          await __signInManager.SignInAsync(user, false);
          return StatusCode(201, user);
        }

        return StatusCode(400, result);
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
      try
      {
        var result = await __signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

        if (result.Succeeded)
        {
          var user = await __userManager.FindByEmailAsync(model.Email);
          return StatusCode(200, user);
        }

        return StatusCode(400, result);
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }
  }
}