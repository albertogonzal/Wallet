using System.Linq;
using System;
using Wallet.Core.Interfaces;
using System.Threading.Tasks;
using Wallet.Core.Entities;
using Wallet.Core.Specifications;
using Wallet.Core.Options;

namespace Wallet.Core.Services
{
  public class AccountService : IAccountService
  {
    private readonly IAsyncRepository<Core.Entities.Account> _repository;
    private readonly IEthereumService _ethService;

    public AccountService(IAsyncRepository<Core.Entities.Account> repository, IEthereumService ethService)
    {
      _ethService = ethService;
      _repository = repository;
    }

    public async Task NewAccount(Guid userId)
    {
      var accounts = await _repository.ListAsync();
      if (accounts.Where(a => a.UserId == userId) == null)
      {
        int accountIndex = accounts.Count;
        var account = new Core.Entities.Account(userId, accountIndex);

        await _repository.AddAsync(account);
      }
    }

    public async Task<Address> NewAddress(Guid accountId)
    {
      var accountSpec = new AccountWithAddressesSpecification(accountId);
      var account = await _repository.FirstOrDefaultAsync(accountSpec);

      int accountIndex = account.AccountIndex;
      int addressIndex = account.Addresses.Count();

      string publicAddress = _ethService.GetEthAddress(accountIndex, addressIndex);

      var newAddress = new Address(accountId, addressIndex, publicAddress);
      account.AddAddress(newAddress);

      await _repository.UpdateAsync(account);

      return newAddress;
    }
  }
}