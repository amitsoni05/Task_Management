using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;
using TaskManagementSystem.Common.Utility;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TaskManagementSystem.Repository.Models;


namespace TaskManagementSystem.Repository.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        //private static ILogger logger = LogManager.GetCurrentClassLogger();
        internal TestDbContext context;
        internal DbSet<TEntity> dbSet;
        public GenericRepository(TestDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            try
            {
                IQueryable<TEntity> query = dbSet.AsQueryable();
                return query;
            }
            catch (Exception)
            {

            }
            return null;
        }
        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                IQueryable<TEntity> query = dbSet.Where(predicate);
                return query;
            }
            catch (Exception)
            {

            }
            return null;
        }
        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                bool query = dbSet.Any(predicate);
                return query;
            }
            catch (Exception)
            {

            }
            return false;
        }
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                TEntity query = dbSet.Where(predicate).FirstOrDefault();
                return query;
            }
            catch (Exception)
            {

            }
            return null;
        }
        public virtual TEntity GetById(object id)
        {
            try
            {
                return dbSet.Find(id);
            }
            catch (Exception)
            {

            }
            return null;
        }
        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public virtual void Insert(TEntity entity, int userId, string IP)
        {
            entity = CreateCommonData(entity, userId, IP);
            dbSet.Add(entity);
        }
        private TEntity CreateCommonData(TEntity entity, int userId, string IP)
        {
            PropertyInfo createdOnProperty = entity.GetType().GetProperty("CreatedOn", BindingFlags.Public | BindingFlags.Instance);
            if (null != createdOnProperty && createdOnProperty.CanWrite)
            {
                createdOnProperty.SetValue(entity, AppCommon.CurrentDate, null);
            }

            PropertyInfo CreatedDateProperty = entity.GetType().GetProperty("CreatedDate", BindingFlags.Public | BindingFlags.Instance);
            if (null != CreatedDateProperty && CreatedDateProperty.CanWrite)
            {
                CreatedDateProperty.SetValue(entity, AppCommon.CurrentDate, null);
            }

            PropertyInfo createdByProperty = entity.GetType().GetProperty("CreatedBy", BindingFlags.Public | BindingFlags.Instance);
            if (null != createdByProperty && createdByProperty.CanWrite)
            {
                createdByProperty.SetValue(entity, userId, null);
            }

            PropertyInfo IpProperty = entity.GetType().GetProperty("Ip", BindingFlags.Public | BindingFlags.Instance);
            if (null != IpProperty && IpProperty.CanWrite)
            {
                IpProperty.SetValue(entity, IP, null);
            }
            return entity;
        }
        public virtual void InsertRange(IEnumerable<TEntity> itemsToInsert)
        {
            try
            {
                if (itemsToInsert == null)
                {
                    throw new ArgumentNullException("entity");
                }
                dbSet.AddRange(itemsToInsert);
            }
            catch (Exception)
            {

            }
        }
        public virtual void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }
        public virtual void Update(TEntity entity, int userId, string IP)
        {
            entity = UpdateCommonData(entity, userId, IP);
            dbSet.Update(entity);
        }
        public virtual void UpdateRange(IEnumerable<TEntity> items, int userId, string IP)
        {
            items = items.Select(p =>
            {
                p = UpdateCommonData(p, userId, IP);
                return p;
            }).AsEnumerable<TEntity>();
            dbSet.UpdateRange(items);
        }
        public virtual void UpdateRange(IEnumerable<TEntity> items)
        {
            dbSet.UpdateRange(items);
        }
        private TEntity UpdateCommonData(TEntity entity, int userId, string IP)
        {
            PropertyInfo updatedOnProperty = entity.GetType().GetProperty("UpdatedOn", BindingFlags.Public | BindingFlags.Instance);
            if (null != updatedOnProperty && updatedOnProperty.CanWrite)
            {
                updatedOnProperty.SetValue(entity, AppCommon.CurrentDate, null);
            }
            PropertyInfo updatedByProperty = entity.GetType().GetProperty("UpdatedBy", BindingFlags.Public | BindingFlags.Instance);
            if (null != updatedByProperty && updatedByProperty.CanWrite)
            {
                updatedByProperty.SetValue(entity, userId, null);
            }
            PropertyInfo IpProperty = entity.GetType().GetProperty("Ip", BindingFlags.Public | BindingFlags.Instance);
            if (null != IpProperty && IpProperty.CanWrite)
            {
                IpProperty.SetValue(entity, IP, null);
            }
            return entity;
        }
        public virtual void Delete(object id)
        {
            try
            {
                TEntity entityToDelete = dbSet.Find(id);
                Delete(entityToDelete);
            }
            catch (Exception)
            {

            }
        }
        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
        public virtual void DeleteAll(IEnumerable<TEntity> itemsToDelete)
        {
            try
            {
                dbSet.RemoveRange(itemsToDelete);
            }
            catch (Exception)
            {

            }
        }

    }
}
