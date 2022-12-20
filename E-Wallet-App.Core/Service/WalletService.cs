using E_Wallet_App.Core.Interface;
using E_Wallet_App.Domain.Dtos;
using E_Wallet_App.Domain.Models;
using E_Wallet_App.Entity.Dtos;
using E_Wallet_App.Entity.Helper;
using E_WalletApp.CORE.Interface.RepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
        public async Task<Wallet> GetWalledByIdAsync(string walletid)
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
        public async Task<PageList<GetWalletDto>> GetAllWalletAsync(PaginationParameter pagin)
        {
            try
            {
                var wallets = await _unitOfWork.Wallet.GetAll() ;
                var walletlist = new List<GetWalletDto>();
                if (wallets != null)
                {
                   foreach(var wallet in wallets)
                    {
                        var walletdto = new GetWalletDto();

                        walletdto.WalletId = wallet.WalletId;
                        walletdto.Balance = wallet.Balance;
                        walletdto.IsActive = wallet.IsActive;
                        walletdto.Date = wallet.Date;
                        walletlist.Add(walletdto);
                    }
                    return PageList<GetWalletDto>.ToPageList(walletlist, pagin.PageNumber, pagin.PageSize);
                }
                return null;
            }
            catch(Exception ex)
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
        public async Task<PageList<GetWalletDto>> GetAllActiveWalletAsync(PaginationParameter pagin)
        {
            try
            {
                var wallets = await _unitOfWork.Wallet.FindByCondition(x => x.IsActive == true);
                var walletlist = new List<GetWalletDto>();
                if (wallets != null)
                {
                    foreach (var wallet in wallets)
                    {
                        var walletdto = new GetWalletDto();
                        walletdto.WalletId = wallet.WalletId;
                        walletdto.Balance = wallet.Balance;
                        walletdto.IsActive = wallet.IsActive;
                        walletdto.Date = wallet.Date;
                        walletlist.Add(walletdto);

                    }
                    return  PageList<GetWalletDto>.ToPageList(walletlist, pagin.PageNumber, pagin.PageSize);
                }
                return null;
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
        public async Task<bool> DeactivateWalletAsync(string walletid)
        {
            try
            {
                var wallet = await GetWalledByIdAsync(walletid);
                if (wallet == null)
                {
                    return false;
                }
                wallet.IsActive = false;
                _unitOfWork.Wallet.Update(wallet);
                _unitOfWork.Complete();
                return true;
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
            return false;
        }
        public async Task<bool> ActivateWalletAsync(string walletid)
        {
            try
            {
                var wallet = await GetWalledByIdAsync(walletid);
                if (wallet == null)
                {
                    return false;
                }
                wallet.IsActive = true;
                _unitOfWork.Wallet.Update( wallet );
                _unitOfWork.Complete();
                return true;
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
            return false;
        }

    }
}
