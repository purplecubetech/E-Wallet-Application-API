using E_Wallet_App.Domain.Dtos;
using E_Wallet_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_WalletApp.CORE.Interface
{
    public interface IUserLogic
    {
        Task<bool> Login(string email, string password);
       // Task<bool> Logout();
        Task<User> RegisterUser(Register register);
        Task<Wallet> CreateWallet();
        Task<string> ForgetPassword(string email);
        Task<bool> ResetPassword(string email, string login);
    }
}
