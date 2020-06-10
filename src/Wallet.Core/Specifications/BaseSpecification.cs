using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Wallet.Core.Interfaces;

namespace Wallet.Core.Specifications
{
  public abstract class BaseSpecification<T> : ISpecification<T>
  {
    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
      Criteria = criteria;
    }
    public Expression<Func<T, bool>> Criteria { get; }
    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
    protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
    {
      Includes.Add(includeExpression);
    }
  }
}