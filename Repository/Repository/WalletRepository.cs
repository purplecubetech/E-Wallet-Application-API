using E_Wallet_App.Domain.Models;
using E_WalletApp.CORE.Interface.RepoInterface;
using E_WalletApp.DB.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_WalletRepository.Repository
{
    public class WalletRepository : RepositoryBase<Wallet>, IWalletRepository
    {
        public WalletRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }
        public async Task<Wallet> GetByWalletId(string Walletid)
        {
            var user = _applicationContext.wallets.FirstOrDefault(x => x.WalletId == Walletid);
            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}
