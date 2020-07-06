using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace ParticipationMarker.Infrastrucutre
{
    public interface IBaseRepository<T> where T : ITableEntity, new()
    {
        Task<TableResult> CreateAsync(T entity);
        Task<TableResult> DeleteAsync(T entity);
        IQueryable<T> Find(Expression<Func<T, bool>> expression);
        Task<TableResult> UpdateAsync(T entity);
        T FindById(string id);
    }
}