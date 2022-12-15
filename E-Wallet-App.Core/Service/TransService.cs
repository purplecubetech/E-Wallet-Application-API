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
using Transaction = E_Wallet_App.Domain.Models.Transaction;

namespace E_Wallet_App.Core.Service
{
    public class TransService: ITransService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;

        public TransService(IUnitOfWork unitOfWork, ILoggerManager logger) 
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<PageList<Transaction>> GetAllTransaction(PaginationParameter pagin)
        {
            try
            {
                var alltransaction = await _unitOfWork.Transaction.GetAll();
                if (alltransaction == null)
                {
                    return null;
                }
                var transactions = new List<Transaction>();
                foreach (var trans in alltransaction)
                {
                    var transact = new Transaction();

                    transact.TransactionId = trans.TransactionId;
                    transact.WalletId = trans.WalletId;
                    transact.PreviousBalance = trans.PreviousBalance;
                    transact.Amount = trans.Amount;
                    transact.CurrentBalance = trans.CurrentBalance;
                    transact.Date = trans.Date;
                    transact.Description = trans.Description;
                    transact.TransactionType = trans.TransactionType;

                    transactions.Add(transact);
                }
                return PageList<Transaction>.ToPageList(transactions, pagin.PageNumber, pagin.PageSize);

            }
            catch (Exception ex)
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
        public async Task<PageList<Transaction>> GetTransactionByWalledId(string walletid, PaginationParameter pagin)
        {
            try
            {
                var wallets = await _unitOfWork.Transaction.FindByCondition(x => x.WalletId == walletid);
                var transactions = new List<Transaction>();
                if (wallets == null)
                {
                    return null;
                }
                foreach (var item in wallets)
                {
                    var transact = new Transaction();

                    transact.TransactionId = item.TransactionId;
                    transact.WalletId = item.WalletId;
                    transact.PreviousBalance = item.PreviousBalance;
                    transact.Amount= item.Amount;
                    transact.CurrentBalance = item.CurrentBalance;
                    transact.Date = item.Date;
                    transact.Description = item.Description;
                    transact.TransactionType = item.TransactionType;
                    transactions.Add( transact );
                }
                return PageList<Transaction>.ToPageList(transactions, pagin.PageNumber, pagin.PageSize);
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
