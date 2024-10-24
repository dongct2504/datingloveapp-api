using System.Linq.Expressions;

namespace DatingLoveApp.DataAccess.Specifications;

public class Specification<T> : ISpecification<T>
{
    public Specification()
    {
    }

    public Specification(Expression<Func<T, bool>> where)
    {
        Where = where;
    }

    public Expression<Func<T, bool>>? Where { get; }

    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

    protected void AddIncludes(Expression<Func<T, object>> include)
    {
        Includes.Add(include);
    }
}
