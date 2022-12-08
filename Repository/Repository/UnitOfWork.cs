using E_WalletApp.CORE.Interface.RepoInterface;
using E_WalletApp.DB.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace E_WalletRepository.Repository
{
    public class UnitOfWork: IUnitOfWork
    {
        private ApplicationContext _applicationContext;
        private IUserRepository _user;
        private IWalletRepository _wallet;
        private ITransactionRepository _transaction;
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_applicationContext);
                }
                return _user;
            }
        }
        public IWalletRepository Wallet
        {
            get
            {
                if (_wallet == null)
                {
                    _wallet = new WalletRepository(_applicationContext);
                }
                return _wallet;
            }
        }
        public ITransactionRepository Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    _transaction = new TransactionRepository(_applicationContext);
                }
                return _transaction;
            }
        }

        public UnitOfWork(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public void Complete()
        {
            _applicationContext.SaveChanges();
        }
    }
}
