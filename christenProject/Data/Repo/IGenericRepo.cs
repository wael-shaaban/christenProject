using christenProject.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace christenProject.Data.Repo
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(FindOptions? findOptions = null);
        TEntity GetById<ID>(ID id) where ID : struct;
        TEntity FindOne(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null);
        TEntity Add(TEntity entity);
        IQueryable<TEntity> AddMany(IEnumerable<TEntity> entities);
        TEntity Update(TEntity entity);
        TEntity Delete(TEntity entity);
        void DeleteMany(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> UpdateMany(Expression<Func<TEntity, bool>> predicate);
        TEntity Attach(TEntity entity);
        TEntity AttachIfNot(TEntity entity);
        bool Any(Expression<Func<TEntity, bool>> predicate);
        int Count(Expression<Func<TEntity, bool>> predicate);
        #region Transactions Functions
        int Commit();
        Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
        #endregion Transactions Functions
        // Execute stored procedure
        Task<IEnumerable<TResult>> ExecuteStoredProcedureAsync<TResult>(string procedureName, params object[] parameters) where TResult : class;

        // Views or any custom queries
        Task<IEnumerable<TResult>> QueryViewAsync<TResult>(string sqlQuery, params object[] parameters) where TResult : class;
    }
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ITIContext _ITIContext;
        public Repository(ITIContext ITIContext)
        {
            this._ITIContext = ITIContext;
        }
        public TEntity Add(TEntity entity) => GetDbSet().Add(entity).Entity;


        public IQueryable<TEntity> AddMany(IEnumerable<TEntity> entities)
        {
            GetDbSet().AddRange(entities);
            return entities.AsQueryable();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return GetDbSet().Any(predicate);
        }

        public TEntity Attach(TEntity entity)
        {
            var dbSet =  GetDbSet();
            var attachedEntity = dbSet.Attach(entity).Entity;
            return attachedEntity;
        }

        public TEntity AttachIfNot(TEntity entity)
        {
            if (_ITIContext.Entry(entity).State == EntityState.Detached)
                return Attach(entity);
            return entity;
        }

       
        public TEntity Delete(TEntity entity)
        {
            if (_ITIContext.Entry(entity).State == EntityState.Detached)
                _ITIContext.Entry(entity).State = EntityState.Deleted;
            else
                GetDbSet().Remove(entity);
            return  entity;
        }

        public void DeleteMany(Expression<Func<TEntity, bool>> predicate)
        {
            var entities =  Find(predicate);
            GetDbSet().RemoveRange(entities);
        }

        public  IQueryable<TEntity> GetAll(FindOptions? findOptions = null)
        {
            return Get(findOptions);
        }

        public TEntity GetById<ID>(ID id) where ID : struct
        {
            return GetDbSet().Find(id);
        }
     
        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null)
        {
            return  Get(findOptions).AsQueryable().FirstOrDefault(predicate)!;
        }
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null)
        {
            return Get(findOptions).Where(predicate).AsQueryable();
        }
        // Dispose the context
        public void Dispose() => _ITIContext.Dispose();
        public TEntity Update(TEntity entity)
        {
            return GetDbSet().Update(entity).Entity;
        }

        public IQueryable<TEntity> UpdateMany(Expression<Func<TEntity, bool>> predicate)
        {
            GetDbSet().UpdateRange(GetDbSet().Where(predicate));
            return GetDbSet().Where(predicate).AsQueryable();
        }
        public virtual int Commit() => _ITIContext.SaveChanges();
        public virtual async Task<int> CommitAsync(CancellationToken cancellationToken = default) =>await _ITIContext.SaveChangesAsync(cancellationToken);
        // Execute a stored procedure and map the result
        public async Task<IEnumerable<TResult>> ExecuteStoredProcedureAsync<TResult>(string procedureName, params object[] parameters) where TResult : class
            => await _ITIContext.Set<TResult>().FromSqlRaw(procedureName, parameters).ToListAsync();

        // Execute raw SQL for custom views or complex queries
        public async Task<IEnumerable<TResult>> QueryViewAsync<TResult>(string sqlQuery, params object[] parameters) where TResult : class
            => await _ITIContext.Set<TResult>().FromSqlRaw(sqlQuery, parameters).ToListAsync();
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            return GetDbSet().Count(predicate);
        }

        #region Private Functions
        private DbSet<TEntity>? Get(FindOptions? findOptions = null)
        {
            findOptions ??= new FindOptions(false, false);
            var entity = GetDbSet();
            if (findOptions.IsAsNoTracking && findOptions.IsIgnoreAutoIncludes)
            {
                entity.IgnoreAutoIncludes().AsNoTracking();
            }
            else if (findOptions.IsIgnoreAutoIncludes)
            {
                entity.IgnoreAutoIncludes();
            }
            else if (findOptions.IsAsNoTracking)
            {
                entity.AsNoTracking();
            }
            return entity;
        }
        private DbSet<TEntity>? GetDbSet() => _ITIContext?.Set<TEntity>();
        #endregion Private Functions
    }
}
