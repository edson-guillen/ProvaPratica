using ProvaPratica.Domain.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProvaPratica.Service.Interfaces
{
    public interface IBaseService<TModel> where TModel : BaseModel
    {
        IQueryable<TModel> Get();

        IQueryable<TModel> Get(int id);

        IQueryable<TModel> Get(Expression<Func<TModel, bool>> predicate);

        int Insert(TModel model);

        Task<int> InsertAsync(TModel model);

        int Update(TModel model);

        Task<int> UpdateAsync(TModel model);

        int Delete(int id);

        Task<int> DeleteAsync(int id);
    }
}
