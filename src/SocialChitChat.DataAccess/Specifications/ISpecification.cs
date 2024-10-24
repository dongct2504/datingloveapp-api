using System.Linq.Expressions;

namespace SocialChitChat.DataAccess.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Where { get; }

    List<Expression<Func<T, object>>> Includes { get; }
}
