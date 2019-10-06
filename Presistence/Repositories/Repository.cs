using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JwtTemplate.Domain.Repositories;

namespace JwtTemplate.Presistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext AppDbContext;
        private readonly DbSet<TEntity> _entity;

        public Repository(AppDbContext _appDbContext)
        {
            AppDbContext = _appDbContext;
            _entity = AppDbContext.Set<TEntity>();
        }


        public async Task<TEntity> GetAsync(int id)
        {
            return await _entity.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {

            return await _entity.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entity.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entity.SingleOrDefaultAsync(predicate);

        }

        public async Task AddAsync(TEntity entity)
        {
            await _entity.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _entity.AddRangeAsync(entities);
        }

        public void Remove(TEntity entity)
        {
            _entity.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entity.RemoveRange(entities);
        }

        public async Task SaveAsync()
        {
            await AppDbContext.SaveChangesAsync();

        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entity.FirstOrDefaultAsync(predicate);
        }
    }
}