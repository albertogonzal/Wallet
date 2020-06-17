using System;
using System.Threading.Tasks;
using Wallet.Core.Entities;
using Wallet.Core.Interfaces;

namespace Wallet.Core.Services
{
  public class BalanceService : IBalanceService
  {
    private readonly IBalanceRepository _balanceRepository;
    private readonly IEthereumService _ethService;
    private readonly IAsyncRepository<Transaction> _txRepository;
    public BalanceService(IBalanceRepository balanceRepository, IAsyncRepository<Transaction> txRepository, IEthereumService ethService)
    {
      _txRepository = txRepository;
      _ethService = ethService;
      _balanceRepository = balanceRepository;
    }
    public async Task Withdraw(Guid userId, string address, decimal amount)
    {
      try
      {
        var balance = await _balanceRepository.GetByUserIdAsync(userId);

        if (balance == null)
        {
          throw new Exception("Balance not found");
        }

        if (balance.Amount < amount)
        {
          throw new Exception("Not enough balance");
        }

        var transaction = await _ethService.CreateTransactionAsync(0, 0, address, amount);

        if (String.IsNullOrEmpty(transaction.TransactionHash))
        {
          throw new Exception("Network error");
        }
        else
        {
          balance.Withdraw(amount);
          await _balanceRepository.UpdateAsync(balance);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"error: {ex.Message}");
      }
    }
  }
}