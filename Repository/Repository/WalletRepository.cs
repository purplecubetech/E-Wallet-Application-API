using E_Wallet_App.Core.Interface;
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
        private readonly ILoggerManager _logger;

        public WalletRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }

        public WalletRepository(ILoggerManager logger, ApplicationContext applicationContext) : base(applicationContext)
        {
            _logger = logger;
        }
        public async Task<Wallet> GetByWalletId(string Walletid)
        {
            try
            {
                var user = _applicationContext.wallets.FirstOrDefault(x => x.WalletId == Walletid);
                if (user == null)
                {
                    return null;
                }
                return user;
            }
            catch(Exception ex)
            {
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
