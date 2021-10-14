using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UserService.Handles;

namespace UserService.Infrastructure.GenericRepository
{

    /// <summary>
    /// standard generic class (CRUD function)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial interface IGenericRepository<T> where T : class
    {
        Task<T> GetByKeyAsync(object key);
        Task InsertAsync(T entity);
        Task InsertListAsync(List<T> entity);
        Task UpdateAsync(T entity);
        Task UpdateListAsync(List<T> entity);
        Task DeleteAsync(T entity);
        Task DeleteListAsync(List<T> entity);
        IQueryable<T> Table { get; }
    }

    public partial class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MyDB_Context _context;
        private DbSet<T> _entities;

        public GenericRepository(MyDB_Context context)
        {
            this._context = context;
            this._entities = context.Set<T>();
        }

        public virtual async Task<T> GetByKeyAsync(object key)
        {
            return await Entities.FindAsync(key);
        }
        public virtual async Task InsertAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new Exception("cannot add new record (Object null)");
                this.Entities.Add(entity);
                await this._context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async Task UpdateAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new Exception("cannot update record (Object null)");
                await this._context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async Task DeleteListAsync(List<T> entity)
        {
            try
            {
                if (entity == null)
                    throw new Exception("cannot delete record (Object null)");
                this.Entities.RemoveRange(entity);
                await this._context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async Task DeleteAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new Exception("cannot delete record (Object null)");
                this.Entities.Remove(entity);
                await this._context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task InsertListAsync(List<T> entity)
        {
            try
            {
                if (entity == null)
                    throw new Exception("cannot add new record (Object null)");
                foreach (var item in entity)
                {
                    this.Entities.Add(item);
                }
                await this._context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateListAsync(List<T> entity)
        {
            try
            {
                if (entity == null)
                    throw new Exception("cannot update record (Object null)");
                await this._context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      

        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }
        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }

    }
}
