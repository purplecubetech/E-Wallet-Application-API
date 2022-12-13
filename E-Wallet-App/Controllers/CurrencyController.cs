using E_Wallet_App.Core.Core;
using E_Wallet_App.Core.Interface;
using E_Wallet_App.Entity.Dtos;
using E_WalletApp.CORE.Interface.RepoInterface;
using E_WalletRepository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace E_Wallet_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize(Roles = "user")]
    public class CurrencyController : ControllerBase
    {
        private ITransLogic _transLogic;
        private IWalletRepository _walletRepository;
        private readonly ILoggerManager _logger;

        public CurrencyController(ITransLogic transLogic, IWalletRepository walletRepository, ILoggerManager logger)
        {
            _transLogic = transLogic;
            _walletRepository = walletRepository;
            _logger = logger;
        }
        [HttpGet("CurrencyConverter")]
        public async Task<ActionResult<BalanceDto>> GetWalletBalance(string walletid, string currency)
        {
            try
            {
                var wallet = await _walletRepository.GetByWalletId(walletid);
                if(wallet == null)
                {
                    return NotFound("wallet does not exist");
                }
                var bal = await _transLogic.GetBalance(walletid, currency);
                var balance = new BalanceDto 
                {
                    WalletId = bal.WalletId,
                    Balance = bal.Balance
                };
                return Ok(balance);
            }
            catch (Exception ex)
            {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
