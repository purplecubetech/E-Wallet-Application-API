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
        private readonly ILoggerManager _logger;

        public UserRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }

        public UserRepository(ILoggerManager logger, ApplicationContext applicationContext) : base(applicationContext)
        {
            _logger = logger;
        }
       
        public async Task<User> GetByEmail(string email)
        {
            try
            {
                var user = _applicationContext.users.FirstOrDefault(x => x.EmailAddress == email.ToLower());
                if (user == null)
                {
                    return null;
                }
                return user;
            }
            catch(Exception ex) {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
                return null;
            }
        }
        
    }
}
