using System.Linq;
using Microsoft.EntityFrameworkCore;
using Wallet.Core.Entities;
using Wallet.Core.Interfaces;

namespace Wallet.Infrastructure.Data
{
  public class SpecificationEvaluator<T> where T : BaseEntity
  {
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
    {
      var query = inputQuery;

      if (specification.Criteria != null)
      {
        query = query.Where(specification.Criteria);
      }

      query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

      return query;
    }
  }
}