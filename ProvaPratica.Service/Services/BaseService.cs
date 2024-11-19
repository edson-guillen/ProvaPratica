using ProvaPratica.Repository.Data;
using ProvaPratica.Service.Interfaces;
using ProvaPratica.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProvaPratica.Service.Services
{
    public class BaseService<TModel> : IBaseService<TModel> where TModel : BaseModel
    {
        public AppDbContext _db;

        public BaseService(AppDbContext db)
        {
            _db = db;
        }

        public IQueryable<TModel> Get(int id)
        {
            return _db.Set<TModel>()
                .Where(p => p.Id.Equals(id))
                .AsQueryable();
        }

        public IQueryable<TModel> Get()
        {
            return _db.Set<TModel>().AsQueryable();
        }

        public IQueryable<TModel> Get(Expression<Func<TModel, bool>> predicate)
        {
            return _db.Set<TModel>().Where(predicate.Compile()).AsQueryable();
        }

        public virtual int Insert(TModel model)
        {
            _db.Add(model);

            return _db.SaveChanges();
        }

        public virtual async Task<int> InsertAsync(TModel model)
        {
            await _db.AddAsync(model);

            return await _db.SaveChangesAsync();
        }

        public virtual int Update(TModel model)
        {
            _db.Update(model);

            return _db.SaveChanges();
        }

        public virtual async Task<int> UpdateAsync(TModel model)
        {
            _db.Update(model);

            return await _db.SaveChangesAsync();
        }

        public virtual int Delete(int id)
        {
            TModel model = Get(id).FirstOrDefault();

            if (model != null)
            {
                _db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

                try
                {

                    return _db.SaveChanges();
                }
                catch
                {
                    return -2;
                }
            }

            return -1;
        }

        public virtual async Task<int> DeleteAsync(int id)
        {
            TModel model = Get(id).FirstOrDefault();

            if (model != null)
            {
                _db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

                try
                {
                    return await _db.SaveChangesAsync();
                }
                catch { return -2; }
            }

            return -1;
        }
    }
}
