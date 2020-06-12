using System.Net;
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
    private readonly IEthereumService _ethService;
    public AccountController(IAccountService accountService, IEthereumService ethService)
    {
      _accountService = accountService;
      _ethService = ethService;
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
  }
}