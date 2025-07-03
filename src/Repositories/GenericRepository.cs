using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApiApp.Data;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIDAsync(Guid id);
    Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, object>>? orderBy = null);
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity, object updatedObject);
    Task<TEntity> RemoveAsync(TEntity entity);
}

namespace WebApiApp.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIDAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, object>>? orderBy = null)
        {
            var query = _dbSet.AsNoTracking();
            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }
            return query;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {         
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, object updatedObject)
        {
            var currentValues = _context.Entry(entity).CurrentValues;
            
            if (updatedObject is Dictionary<string, object> updatedData)
            {
                foreach (var keyValue in updatedData)
                {
                    currentValues[keyValue.Key] = keyValue.Value;
                }
            }
            else
            {
                _context.Entry(entity).CurrentValues.SetValues(updatedObject);
            }

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> RemoveAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
