using E_Wallet_App.Domain.Models;
using E_Wallet_App.Entity.Dtos;
using E_Wallet_App.Entity.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Core.Interface
{
    public interface IWalletService
    {
        Task<Wallet> GetWalledByIdAsync(string walletid);
        Task<bool> DeactivateWalletAsync(string walletid);
        Task<bool> ActivateWalletAsync(string walletid);
        Task<PageList<GetWalletDto>> GetAllWalletAsync(PaginationParameter pagin);
        Task<PageList<GetWalletDto>> GetAllActiveWalletAsync(PaginationParameter pagin);
    }
}
