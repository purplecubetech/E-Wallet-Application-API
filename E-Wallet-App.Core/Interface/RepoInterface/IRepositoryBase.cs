using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_WalletApp.CORE.Interface.RepoInterface
{
    public interface IRepositoryBase<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> expression);
        void Create(TEntity entity);
        void AddRange(IEnumerable<TEntity> entity);
        void DeleteRange(IEnumerable<TEntity> entity);
        void Update(TEntity entity);
        void Delete(TEntity user);


    }
}
