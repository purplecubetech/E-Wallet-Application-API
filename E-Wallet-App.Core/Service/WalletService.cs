using E_Wallet_App.Core.Interface;
using E_Wallet_App.Domain.Models;
using E_WalletApp.CORE.Interface.RepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Core.Service
{
    public class WalletService: IWalletService
    {
        private readonly ILoggerManager _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWalletLogic _walletLogic;

        public WalletService(ILoggerManager logger, IUnitOfWork unitOfWork, IWalletLogic walletLogic)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _walletLogic = walletLogic;
        }
        public async Task<Wallet> GetWalledById(string walletid)
        {
            try
            {
                var wallets = await _unitOfWork.Wallet.FindByCondition(x => x.WalletId == walletid);
                var wallet = new Wallet();
                if(wallets == null)
                {
                  return  null;
                }
                foreach(var item in wallets)
                {
                    wallet.UserId = item.UserId;
                    wallet.WalletId = item.WalletId;
                    wallet.Balance = item.Balance;
                    wallet.Date = item.Date;
                }
                return wallet;
            }
            catch (Exception ex)
            {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
            }
            return null;
        }

    }
}
