using HealthTracker.Exceptions;
using HealthTracker.Models;
using HealthTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace ATMApplication.Repositories
{
    public abstract class AbstractRepositoryClass<K, T> : IRepository<K, T> where T : class
    {
        protected readonly HealthTrackerContext _context;
        protected readonly DbSet<T> _dbSet;

        public AbstractRepositoryClass(HealthTrackerContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }

        public async virtual Task<T> Delete(K id)
        {
            var ob = await GetById(id);
            _dbSet.Remove(ob);
            await _context.SaveChangesAsync();
            return ob;
        }

        public async virtual Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async virtual Task<T> GetById(K id)
        {
            T ob = await _dbSet.FindAsync(id);
            if (ob == null)
            {
                throw new EntityNotFoundException("Entity not found!");
            }
            return ob;
        }

        public async virtual Task<T> Add(T entity)
        {
            var ob = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return ob.Entity;
        }

        public async virtual Task<T> Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}