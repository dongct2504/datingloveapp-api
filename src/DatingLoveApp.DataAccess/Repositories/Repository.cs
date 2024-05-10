using DatingLoveApp.DataAccess.Data;
using DatingLoveApp.DataAccess.Extensions;
using DatingLoveApp.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DatingLoveApp.DataAccess.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DatingLoveAppDbContext _dbContext;

    private readonly DbSet<T> _dbSet;

    private int count;

    public Repository(DatingLoveAppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(QueryOptions<T>? options = null)
    {
        if (options != null)
        {
            return await BuildQuery(options).ToListAsync();
        }

        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetAsync(long id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetAsync(QueryOptions<T> options)
    {
        return await BuildQuery(options).FirstOrDefaultAsync();
    }

    public async Task<int> GetCountAsync()
    {
        return count > 0 ? count : await _dbSet.CountAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    private IQueryable<T> BuildQuery(QueryOptions<T> options)
    {
        IQueryable<T> query = _dbSet; // ex: _context.Books;

        if (options.HasInclude)
        {
            foreach (string include in options.GetIncludes)
            {
                query = query.Include(include);
            }
        }

        if (options.HasWhereClause)
        {
            foreach (Expression<Func<T, bool>> expression in options.WhereClauses)
            {
                query = query.Where(expression);
            }
            count = query.Count(); // get filter count
        }

        if (options.HasOrderBy)
        {
            query = query.OrderBy(options.OrderBy);
        }

        if (options.HasPaging)
        {
            query = query.Skip(options.PageSize * (options.PageNumber - 1))
                .Take(options.PageSize);
        }

        return query;
    }
}
