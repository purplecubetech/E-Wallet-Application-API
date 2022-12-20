using E_Wallet_App.Core.Core;
using E_Wallet_App.Core.Interface;
using E_Wallet_App.Domain.Dtos;
using E_Wallet_App.Domain.Models;
using E_Wallet_App.Entity.Dtos;
using E_Wallet_App.Entity.Helper;
using E_WalletApp.CORE.Interface.RepoInterface;
using E_WalletRepository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace E_Wallet_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransService _transService;
        private readonly IWalletLogic _wallet;
        private readonly IWalletService _walletService;
        private readonly ITransLogic _transLogic;
        private readonly ILoggerManager _logger;

        public TransactionController(ITransService transService, IWalletLogic wallet, IWalletService walletService, ITransLogic transLogic, ILoggerManager logger)
        {
            _transService = transService;
            _wallet = wallet;
            _walletService = walletService;
            _transLogic = transLogic;
            _logger = logger;
        }

        [HttpPost("Deposit")]
        [Authorize(Roles = "user")]

        public async Task<ActionResult<TransDto>> Deposite([FromForm]TransDto transDto)
        {
            try
            {
                var user = await _walletService.GetWalledByIdAsync(transDto.WalletId);
                if (user == null)
                {
                    return BadRequest($"{transDto.WalletId} does not already exixts");
                }
                var check = await _transLogic.Deposite(transDto);
                if (!check)
                {
                    return BadRequest("transaction unsuccessful");
                }
                return Ok("transaction completed");

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
        [HttpPost("Withdrawal")]
        [Authorize(Roles = "user")]

        public async Task<ActionResult<TransDto>> Withdrawal([FromForm]TransDto transDto)
        {
            try
            {
                var user = await _walletService.GetWalledByIdAsync(transDto.WalletId);
                if (user == null)
                {
                    return BadRequest($"{transDto.WalletId} does not already exixts");
                }
                var check = await _transLogic.Withdrawal(transDto);
                if (!check)
                {
                    return BadRequest("transaction unsuccessful");
                }
                return Ok("transaction completed");

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
        [HttpPost("Transfer")]
        [Authorize(Roles = "user")]

        public async Task<ActionResult<TransDto>> Transfer([FromForm]TransferDto transferDto)
        {
            try
            {
                var user1 = await _walletService.GetWalledByIdAsync(transferDto.FromWallet);
                var user2 = await _walletService.GetWalledByIdAsync(transferDto.ToWallet);
                if (user1 == null)
                {
                    return BadRequest($"{transferDto.FromWallet} does not exixts");
                }
                if(user2 == null)
                {
                    return BadRequest($"{transferDto.ToWallet} does not exist");
                }
                var check = await _transLogic.Transfer(transferDto);
                if (!check)
                {
                    return BadRequest("transaction unsuccessful");
                }
                return Ok("transaction completed");

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
        [HttpGet("GetAllTransactions")]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult> GetAllTransactions([FromQuery]PaginationParameter pagin)
        {
            try
            {
                var alltransactions = await _transService.GetAllTransaction(pagin);
                if(alltransactions == null)
                {
                    return NotFound("no transactions yet ");
                }
                return Ok(alltransactions);
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
        [HttpGet("GetTransactionByWalletId")]
        //[Authorize(Roles = "user")]

        public async Task<ActionResult> GetTransByWalletId(string walletId, [FromQuery] PaginationParameter pagin)
        {
            try
            {
                var transwithId = await _walletService.GetWalledByIdAsync(walletId);
                if (transwithId == null)
                {
                    return NotFound("wallet nnot found");
                }
                var trans = await _transService.GetTransactionByWalledId(transwithId.WalletId, pagin);
                return Ok(trans);
            }
            catch(Exception ex)
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
