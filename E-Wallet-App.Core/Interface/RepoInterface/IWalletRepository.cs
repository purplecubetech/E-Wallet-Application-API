using E_Wallet_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_WalletApp.CORE.Interface.RepoInterface
{
    public interface IWalletRepository : IRepositoryBase<Wallet>
    {
        Task<Wallet> GetByWalletId(string email);

    }
}
