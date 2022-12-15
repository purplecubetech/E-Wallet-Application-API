using E_Wallet_App.Core.Interface;
using E_Wallet_App.Domain.Models;
using E_WalletApp.CORE.Interface.RepoInterface;
using E_WalletApp.DB.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_WalletRepository.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {

        public UserRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }        
    }
}