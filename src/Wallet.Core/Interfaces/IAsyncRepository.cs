using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wallet.Core.Entities;

namespace Wallet.Core.Interfaces
{
  public interface IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
  {
    Task<T> GetByIdAsync(Guid id);
    Task<List<T>> ListAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
  }
}