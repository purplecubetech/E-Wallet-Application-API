using E_Wallet_App.Core.Interface;
using E_Wallet_App.Core.Service;
using E_Wallet_App.Domain.Dtos;
using E_Wallet_App.Domain.Models;
using E_Wallet_App.Entity.Dtos;
using E_Wallet_App.Entity.Helper;
using E_WalletApp.CORE.Core;
using E_WalletApp.CORE.Interface;
using E_WalletApp.CORE.Interface.RepoInterface;
using E_WalletRepository.Repository;
using EmailService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace E_Wallet_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;
        private readonly IUserLogic _userLogic;
        private readonly IESender _eSender;
        private readonly IWalletService _walletService;

        public UserController(IUserService userService, ILoggerManager logger, IWalletService walletService, IUserLogic userLogic, IESender eSender)
        {
            _userService = userService;
            _logger = logger;
            _userLogic = userLogic;
            _eSender = eSender;
            _walletService = walletService;
        }
        [HttpPost("RegisterUser")]
        public async Task<ActionResult<Register>> RegisterUser([FromForm] Register register)
        {
            try
            {
                var user = await _userService.GetByEmail(register.EmailAddress);
                if (user == null)
                {
                    return BadRequest($"{register.EmailAddress} already exixts");
                }
                else if(register.Password != register.ComfirmPassword)
                {
                    return BadRequest("password does not match");
                }
                else
                {
                    var output = await _userService.RegisterUser(register);
                    return Ok($"{output}/n account registered");
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
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("verifyUser")]
        [Authorize(Roles = "user")]

        public async Task<ActionResult> VerifyUser([FromForm]string token)
        {
            try
            {
                var check = await _userLogic.VerifyUser(token);
                if (!check)
                {
                    return BadRequest("user not verified");
                }
                
                return Ok("user verified");
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
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login([FromForm] Login user)
        {
            try
            {
                    var token = await _userLogic.Login(user.Email, user.Password);
                    if (token != null)
                    {
                        return Ok($"you are logged in/n {token}");
                    }
 
                return BadRequest("wrong email or password");
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
        [HttpGet("GetAllUser")]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult> GetAllUser([FromQuery]PaginationParameter pagin)
        {
            try
            {
                var alluser = await _userService.GetAllUser(pagin);
                if(alluser == null)
                {
                    return NotFound("no user was found");
                }

                return Ok(alluser);
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
        [HttpGet("GetUserById")]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = _userService.GetUserById(id) ;
                if(user == null)
                {
                    return NotFound("user not found");
                }
                return Ok(user);
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
        [HttpGet("GetUserByWalletId")]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult> GetUserByWalletId(string walletId)
        {
            try
            {
                var userwithId= await _walletService.GetWalledByIdAsync(walletId);
                if (userwithId == null)
                {
                    return NotFound("user not found");
                }
                var user = await _userService.GetUserById(userwithId.UserId);
                return Ok(user);
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
        [HttpPut("ForGotPassword")]
        public async Task<ActionResult<string>> ForgotPassword([FromForm]string email)
        {
            try
            {
                var user = await _userService.GetByEmail(email);
                if (user == null)
                {
                    return BadRequest("your email does not exist");
                }
                await _userLogic.ForgetPassword(email);
                var emailmsg = new EmailDto(email, "RESET PASSWORD", $"use this link to reset your password https://localhost:44396/api/User/ForGotPassword?={user.PasswordResetToken}", null);
                await _eSender.SendEmailAsync(emailmsg);
                return Ok($"use this token {user.PasswordResetToken} to reset your password");
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
       
        [HttpPut("ResetPassword")]
        //UPDATE USER PASSWORD
        public async Task<ActionResult<string>> ResetPassword([FromForm] ResetPasswordRequest request)
        {
            try
            {   
                var result =await _userLogic.ResetPassword(request);
                if (result == null)
                {
                    return StatusCode(500, "password could not be reset");
                }
                return Ok("your password has been reset");

            }
            catch (Exception ex)
            {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
                _logger.Equals($"{ex.TargetSite}");
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPatch("Deactivate-Wallet")]
        public async Task<ActionResult<string>> DeactivateWallet(string walletid)
        {
            try 
            {
                var status = await _walletService.DeactivateWalletAsync(walletid);
                if (status)
                {
                    return Ok($"wallet with with ID {walletid} has been deactivared");
                }
                return BadRequest("wallet not deactivated");
            }  
            catch (Exception ex)
            {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
                _logger.Equals($"{ex.TargetSite}");
            }
            return StatusCode(500, "process could not be completed");

        }
        [HttpPatch("Activate-Wallet")]
        public async Task<ActionResult<string>> ActivateWallet(string walletid)
        {
            try
            {
                var status = await _walletService.ActivateWalletAsync(walletid);
                if (status)
                {
                    return Ok($"wallet with with ID {walletid} has been deactivared");
                }
                return BadRequest("wallet not deactivated");
            }
            catch (Exception ex)
            {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
                _logger.Equals($"{ex.TargetSite}");
            }
            return StatusCode(500, "process could not be completed");

        }
        [HttpGet("Get-All-Wallet")]
        public async Task<ActionResult<string>> GetAllWallets([FromQuery] PaginationParameter pagin)
        {
            try
            {
                var allwallets = await _walletService.GetAllWalletAsync(pagin) ;
                if (allwallets != null)
                {
                    return Ok(allwallets);
                }
                return BadRequest("wallets not found");
            }
            catch (Exception ex)
            {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
                _logger.Equals($"{ex.TargetSite}");
            }
            return StatusCode(500, "process could not be completed");

        }
        [HttpGet("Get-All-Active-Wallet")]
        public async Task<ActionResult<string>> GetAllActiveWallets([FromQuery] PaginationParameter pagin)
        {
            try
            {
                var allwallets = await _walletService.GetAllActiveWalletAsync(pagin);
                if (allwallets != null)
                {
                    return Ok(allwallets);
                }
                return BadRequest("wallets not found");
            }
            catch (Exception ex)
            {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
                _logger.Equals($"{ex.TargetSite}");
            }
            return StatusCode(500, "process could not be completed");

        }
        //[HttpDelete]
        //public async Task<ActionResult<string>> EmptyDatabase(IEnumerable<User> users, IEnumerable<Wallet> wallets, IEnumerable<Transaction> transactions)
        //{
        //    try
        //    {
        //          _userService.DeleteAll(users, wallets, transactions);

        //        return Ok("database clared");
        //    }
        //    catch(Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
    }
}
