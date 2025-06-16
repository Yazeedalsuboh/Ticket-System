using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SupportTicketingData.Data;
using SupportTicketingData.Interface;

namespace SupportTicketingData.Repositories
{
    public class GenericRepo<T>: IGenericRepo<T> where T : class

    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepo(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                ? await _dbSet.ToListAsync()
                : await _dbSet.Where(predicate).ToListAsync();
        }

       
        public async Task<IList<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _dbSet
                .OrderBy(v => EF.Property<object>(v, "Id"))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        public async Task Update(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }   

    }
}
