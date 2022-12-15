using E_Wallet_App.Domain.Models;
using E_WalletApp.CORE.Interface.RepoInterface;
using E_WalletApp.DB.Context;
using Transaction = E_Wallet_App.Domain.Models.Transaction;

namespace E_WalletRepository.Repository
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }
        
    }
}