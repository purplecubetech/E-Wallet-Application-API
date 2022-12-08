using E_WalletApp.CORE.Interface.RepoInterface;
using E_WalletApp.DB.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_WalletRepository.Repository
{
    public abstract class RepositoryBase<TEntity>: IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly ApplicationContext _applicationContext;
        public RepositoryBase(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public async Task<IEnumerable<TEntity>> GetAll()
        {
            try
            {
                var result = _applicationContext.Set<TEntity>().AsNoTracking().ToList();
                if(result== null) {
                return Enumerable.Empty<TEntity>();
                }
                return result;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<TEntity>();
            }
        }

        public async Task<TEntity> GetById(Guid id)
        {
            try
            {
                var result = _applicationContext.Set<TEntity>().Find(id);
                if(result == null)
                {
                    return null;
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                var result = _applicationContext.Set<TEntity>().Where(expression).AsNoTracking().ToList();
                if (result == null)
                {
                    return Enumerable.Empty<TEntity>();
                }
                return result;
            }
            catch (Exception ex)
            {
                return new List<TEntity>();
            }
        }
        public void Create(TEntity entity) 
        {
            try
            {
                _applicationContext.Set<TEntity>().Add(entity);

            }
            catch (Exception ex)
            {

            }
        }
        public void Update(TEntity entity)
        {
            try
            {
                _applicationContext.Set<TEntity>().Update(entity);

            }
            catch (Exception ex)
            {

            }
        }
        public void AddRange(IEnumerable<TEntity> entity)
        {
            try
            {
                _applicationContext.Set<TEntity>().AddRange(entity);

            }
            catch (Exception ex) { }
        }

        public void Delete(TEntity entity) 
        {
            try
            {
                _applicationContext.Set<TEntity>().Remove(entity);

            }
            catch (Exception ex) { }
        }
        public void DeleteRange(IEnumerable<TEntity> entity)
        {
            try
            {
                _applicationContext.Set<TEntity>().RemoveRange(entity);

            }
            catch (Exception ex) { }
        }
    }
}
