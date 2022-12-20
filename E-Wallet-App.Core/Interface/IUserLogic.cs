using E_Wallet_App.Domain.Dtos;
using E_Wallet_App.Domain.Models;
using E_Wallet_App.Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_WalletApp.CORE.Interface
{
    public interface IUserLogic
    {
        Task<string> ForgetPassword(string email);
        Task<string> ResetPassword(ResetPasswordRequest request);
        Task<string> Login(string email, string password);
        Task<string> Generatetoken(string email, string role);
        Task<bool> VerifyUser(string token);
        void CreatepasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        Task<bool> VerifypasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
