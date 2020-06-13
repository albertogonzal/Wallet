using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wallet.Core.Entities;
using Wallet.Core.Interfaces;

namespace Wallet.Infrastructure.Data
{
  public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
  {
    private readonly WalletDbContext _dbContext;

    public EfRepository(WalletDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
      return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<List<T>> ListAsync()
    {
      return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<List<T>> ListAsync(ISpecification<T> spec)
    {
      return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
      await _dbContext.Set<T>().AddAsync(entity);
      await _dbContext.SaveChangesAsync();

      return entity;
    }

    public async Task UpdateAsync(T entity)
    {
      _dbContext.Entry(entity).State = EntityState.Modified;
      await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
      _dbContext.Set<T>().Remove(entity);
      await _dbContext.SaveChangesAsync();
    }

    public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
    {
      return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
      return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
    }
  }
}