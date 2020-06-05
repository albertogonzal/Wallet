using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wallet.Core.Entities;
using Wallet.Core.Interfaces;

namespace Wallet.Web.Controllers
{
  public class AccountController : BaseApiController
  {
    private readonly IAccountService _accountService;
    public AccountController(IAccountService accountService)
    {
      _accountService = accountService;

    }

    [HttpPost]
    public async Task<object> NewAddress()
    {
      Asset coin = new Asset();

      return await _accountService.NewAddress(coin);
    }
  }
}