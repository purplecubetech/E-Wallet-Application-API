using E_Wallet_App.Core.Interface;
using E_Wallet_App.Core.Service;
using E_Wallet_App.Domain.Models;
using E_Wallet_App.Entity.Dtos;
using E_WalletApp.CORE.Interface.RepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Wallet_App.Core.Core
{
    public class TransLogic: ITransLogic
    {
        private readonly IWalletService _walletService;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpClientFactory _httpClientFactory;
        private ILoggerManager _logger;

        public TransLogic(IWalletService walletService, IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, ILoggerManager logger) 
        {
            _walletService = walletService;
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task<bool> Deposite(TransDto transDto)
        {
            try
            {
                bool check = true;
                var wallet = await _walletService.GetWalledByIdAsync(transDto.WalletId);
                if (wallet == null)
                {
                    check = false;
                    return check;
                }
                wallet.Balance += transDto.amount;
                wallet.Date = DateTime.Now;


                var transaction = new Transaction();
                transaction.Amount = transDto.amount;
                transaction.TransactionId = Guid.NewGuid();
                transaction.TransactionType = "Deposite";
                transaction.Description = $"your wallet: {transDto.WalletId} has been credited with {transDto.amount}";
                transaction.Date = DateTime.Now;
                transaction.WalletId = wallet.WalletId;
                transaction.PreviousBalance = wallet.Balance + transDto.amount;
                transaction.CurrentBalance = wallet.Balance;
                _unitOfWork.Wallet.Update(wallet);
                _unitOfWork.Transaction.Create(transaction);
                _unitOfWork.Complete();
                return check;
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

        public async Task<bool> Withdrawal(TransDto transDto)
        {
            try
            {
                var wallet = await _walletService.GetWalledByIdAsync(transDto.WalletId);
                if (wallet != null)
                {
                    if(wallet.IsActive == false)
                    {
                        return false;
                    }
                    if (wallet.Balance < transDto.amount)
                    {
                        return false;
                    }
                    wallet.Balance -= transDto.amount;
                    wallet.Date = DateTime.Now;

                    var transaction = new Transaction();
                    transaction.Amount = transDto.amount;
                    transaction.TransactionId = Guid.NewGuid();
                    transaction.TransactionType = "withdrawal";
                    transaction.Description = $"your wallet: {transDto.WalletId} has been debited with {transDto.amount}";
                    transaction.Date = DateTime.Now;
                    transaction.WalletId = wallet.WalletId;
                    transaction.PreviousBalance = wallet.Balance + transDto.amount;
                    transaction.CurrentBalance = wallet.Balance;
                    _unitOfWork.Wallet.Update(wallet);
                    _unitOfWork.Transaction.Create(transaction);
                    _unitOfWork.Complete();
                    return true;

                }
                return false;
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
        public async Task<bool> Transfer(TransferDto transferDto)
        {
            try
            {
                var fromWallet = await _walletService.GetWalledByIdAsync(transferDto.FromWallet);
                var toWallet = await _walletService.GetWalledByIdAsync(transferDto.ToWallet);
                if (fromWallet != null && toWallet != null)
                {
                    if(fromWallet.IsActive == false)
                    {
                        return false;
                    }
                    if (fromWallet.Balance < transferDto.Amount)
                    {
                        return false;
                    }
                    fromWallet.Balance -= transferDto.Amount;
                    fromWallet.Date = DateTime.Now;

                    toWallet.Balance += transferDto.Amount;
                    toWallet.Date = DateTime.Now;

                    var fromtransaction = new Transaction();
                    fromtransaction.Amount = transferDto.Amount;
                    fromtransaction.TransactionId = Guid.NewGuid();
                    fromtransaction.TransactionType = "transfer out";
                    fromtransaction.Description = $"your wallet {transferDto.FromWallet} hsa been debited with {transferDto.Amount}";
                    fromtransaction.Date = DateTime.Now;
                    fromtransaction.WalletId = fromWallet.WalletId;
                    fromtransaction.PreviousBalance = fromWallet.Balance + transferDto.Amount;
                    fromtransaction.CurrentBalance = fromWallet.Balance;

                    var totransaction = new Transaction();
                    totransaction.Amount = transferDto.Amount;
                    totransaction.TransactionId = Guid.NewGuid();
                    totransaction.TransactionType = "transfer in";
                    totransaction.Description = $"your wallet {transferDto.ToWallet} hsa been credited with {transferDto.Amount}";
                    totransaction.Date = DateTime.Now;
                    totransaction.WalletId = toWallet.WalletId;
                    totransaction.PreviousBalance = toWallet.Balance + transferDto.Amount;
                    totransaction.CurrentBalance = toWallet.Balance;

                    _unitOfWork.Wallet.Update(toWallet);
                    _unitOfWork.Transaction.Create(totransaction);
                    _unitOfWork.Wallet.Update(fromWallet);
                    _unitOfWork.Transaction.Create(fromtransaction);
                    _unitOfWork.Complete();
                    return true;

                }
                return false;
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
        public async Task<BalanceDto> GetBalance(string walletid, string currency)
        {
            try
            {
                var wallet = await _walletService.GetWalledByIdAsync(walletid);
                if (wallet == null)
                {
                    return new BalanceDto();
                }
                var walletadetails = new BalanceDto();
                {
                    walletadetails.WalletId = wallet.WalletId;
                    walletadetails.Balance = $"{currency.ToUpper()} {Math.Round(wallet.Balance * await GetCurrencyApi(currency), 3)}";
                }
                return walletadetails;
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
        public async Task<double> GetCurrencyApi(string currency)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                using (var response = await httpClient.GetAsync("https://open.er-api.com/v6/latest/NGN", HttpCompletionOption.ResponseHeadersRead))

                {
                    var curr = currency.ToUpper();
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();
                    var rates = await JsonSerializer.DeserializeAsync<Currency>(stream);
                    if (rates != null)
                    {
                        return rates.rates[curr];
                    }
                    return 0;
                }
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
            return 0;
        }
    }
}
