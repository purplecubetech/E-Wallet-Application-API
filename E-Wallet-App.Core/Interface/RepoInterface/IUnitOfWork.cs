using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_WalletApp.CORE.Interface.RepoInterface
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IWalletRepository Wallet { get; }
        ITransactionRepository Transaction { get; }
        void Complete();
    }
}
