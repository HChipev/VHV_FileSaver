﻿using VHV_FileSaver.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace VHV_FileSaver.Data.Repository
{

    public class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {
        private readonly DataContext _context;
        private readonly DbSet<T> _entities;

        public Repository(DataContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _entities.AsQueryable();
        }

        public T? Find(int id)
        {
            T? entity = _entities.Find(id);
            if (entity is null)
            {
                return null;
            }

            return entity;
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

        public T? FindByCondition(Expression<Func<T, bool>> predicate)
        {
            T? entity = _entities.FirstOrDefault(predicate);
            if (entity is null)
            {
                return null;
            }

            return entity;
        }

        public IQueryable<T> FindAllByCondition(Expression<Func<T, bool>> predicate)
        {
            var entities = _entities.Where(predicate);

            return entities.AsQueryable();
        }

        public T? Remove(int id)
        {
            T? entity = Find(id);
            if (entity is null)
            {
                return null;
            }

            _entities.Remove(entity);

            return entity;
        }

        public void Update(T obj)
        {
            _entities.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
    }
}

