using E_Wallet_App.Domain.Models;
using E_Wallet_App.Entity.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace E_Wallet_App.Core.Interface
{
    public interface ITransService
    {
        Task<PageList<Transaction>> GetAllTransaction(PaginationParameter pagin);
        Task<PageList<Transaction>> GetTransactionByWalledId(string walletid, PaginationParameter pagin);
    }
}
