using VHV_FileSaver.Data.Models;
using System.Linq.Expressions;

namespace VHV_FileSaver.Data.Repository
{
    public interface IRepository<T> where T : IBaseEntity
    {
        IQueryable<T> GetAll();
        T? Find(int id);
        void Add(T entity);
        void Update(T entity);
        T? Remove(int id);
        bool SaveChanges();
        T? FindByCondition(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindAllByCondition(Expression<Func<T, bool>> predicate);
    }
}

