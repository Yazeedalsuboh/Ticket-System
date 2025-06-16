using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingData.Interface
{
    public interface IGenericRepo<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        //------------
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate = null);
       // Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null);
        Task<IList<T>> GetPagedAsync(int pageNumber, int pageSize);
        //-------------
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
