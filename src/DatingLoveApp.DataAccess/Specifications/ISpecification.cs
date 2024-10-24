using System.Linq.Expressions;

namespace DatingLoveApp.DataAccess.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Where { get; }

    List<Expression<Func<T, object>>> Includes { get; }
}
