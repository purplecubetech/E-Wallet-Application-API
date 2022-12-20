using E_Wallet_App.Domain.Dtos;
using E_Wallet_App.Domain.Models;
using E_Wallet_App.Entity.Dtos;
using E_Wallet_App.Entity.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_WalletApp.CORE.Interface
{
    public interface IUserService
    {
        Task<User> GetByEmail(string email);
        Task<GetUserDto> GetUserById(Guid id);
        //void DeleteAll(IEnumerable<User> users, IEnumerable<Wallet> wallets, IEnumerable<Transaction> transactions);
        Task<PageList<GetUserDto>> GetAllUser(PaginationParameter pagin);
        Task<User> RegisterUser(Register register);
    }
}
