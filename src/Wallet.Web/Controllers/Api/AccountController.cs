using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wallet.Core.Entities;
using Wallet.Core.Interfaces;
using System.Collections.Generic;

namespace Wallet.Web.Controllers
{
  public class AccountController : BaseApiController
  {
    private readonly IAccountService _accountService;
    private readonly IEthereumService _ethService;
    private readonly IBackgroundService _backgroundService;

    public AccountController(IAccountService accountService, IEthereumService ethService, IBackgroundService backgroundService)
    {
      _accountService = accountService;
      _ethService = ethService;
      _backgroundService = backgroundService;
    }

    [HttpPost]
    public async Task<Address> NewAddress(Guid accountId, Guid assetId)
    {
      // Validate request userId
      return await _accountService.NewAddress(accountId, assetId);
    }

    [HttpPost]
    public async Task<string> Balance(string address)
    {
      return await _ethService.GetBalanceAsync(address);
    }

    [HttpPost]
    public async Task<List<string>> Test()
    {
      return await _backgroundService.Transfer();
    }
  }
}