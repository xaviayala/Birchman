using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Birchman.RemoteProvisioning.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
        T Get(object primaryKey);
        IQueryable<T> GetAll();
        IQueryable<T> GetAllIncluding(string propertyName);
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
        void SaveChanges();
        void Dispose();
    }
}