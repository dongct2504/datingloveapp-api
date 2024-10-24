using Microsoft.EntityFrameworkCore;
using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Interfaces;
using SocialChitChat.DataAccess.Specifications;

namespace SocialChitChat.DataAccess.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DatingLoveAppDbContext _dbContext;

    private readonly DbSet<T> _dbSet;

    public Repository(DatingLoveAppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = false)
    {
        if (asNoTracking)
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        return await _dbSet.ToListAsync();
    }

    public async Task<int> GetCountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<T?> GetAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetAsync(long id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetAsync(string id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveAllAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    // specs
    public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec, bool asNoTracking = false)
    {
        if (asNoTracking)
        {
            return await ApplySpec(spec).AsNoTracking().ToListAsync();
        }

        return await ApplySpec(spec).ToListAsync();
    }

    public async Task<T?> GetWithSpecAsync(ISpecification<T> spec, bool asNoTracking = false)
    {
        if (asNoTracking)
        {
            return await ApplySpec(spec).AsNoTracking().FirstOrDefaultAsync();
        }

        return await ApplySpec(spec).FirstOrDefaultAsync();
    }

    private IQueryable<T> ApplySpec(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
    }
}
