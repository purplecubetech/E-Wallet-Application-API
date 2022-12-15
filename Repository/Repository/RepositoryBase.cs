using E_Wallet_App.Core.Interface;
using E_WalletApp.CORE.Interface.RepoInterface;
using E_WalletApp.DB.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_WalletRepository.Repository
{
    public abstract class RepositoryBase<TEntity>: IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly ApplicationContext _applicationContext;
        private readonly ILoggerManager _logger;

        public RepositoryBase(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            
        }
        public RepositoryBase(ILoggerManager logger) 
        {
            _logger = logger;
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
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
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
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
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
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
                return Enumerable.Empty<TEntity>();
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
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
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
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
            }
        }
        public void AddRange(IEnumerable<TEntity> entity)
        {
            try
            {
                _applicationContext.Set<TEntity>().AddRange(entity);

            }
            catch (Exception ex) {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
            }
        }

        public void Delete(TEntity entity) 
        {
            try
            {
                _applicationContext.Set<TEntity>().Remove(entity);

            }
            catch (Exception ex) 
            {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
            }
        }
        public void DeleteRange(IEnumerable<TEntity> entity)
        {
            try
            {
                _applicationContext.Set<TEntity>().RemoveRange(entity);

            }
            catch (Exception ex) 
            {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
            }
        }
    }
}
