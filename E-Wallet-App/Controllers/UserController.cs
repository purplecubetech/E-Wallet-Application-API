using E_Wallet_App.Core.Interface;
using E_Wallet_App.Domain.Dtos;
using E_Wallet_App.Domain.Models;
using E_Wallet_App.Entity.Dtos;
using E_Wallet_App.Entity.Helper;
using E_WalletApp.CORE.Core;
using E_WalletApp.CORE.Interface;
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
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserLogic _userLogic;
        private readonly IWalletService _walletService;

        public UserController(IUserService userService, ILoggerManager logger, IWalletService walletService, IUnitOfWork unitOfWork, IUserLogic userLogic)
        {
            _userService = userService;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userLogic = userLogic;
            _walletService = walletService;
        }
        [HttpPost("RegisterUser")]
        public async Task<ActionResult<Register>> RegisterUser([FromForm] Register register)
        {
            //var userName = new UserDto();
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
                var user = await _unitOfWork.User.FindByCondition(u => u.VerificationToken == token);
                if (user == null)
                {
                    return BadRequest("user not verified");
                }
                foreach (var item in user)
                {
                    item.VerifiedAt = DateTime.Now;
                }
                _unitOfWork.Complete();

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
        //[Authorize(Roles = "admin")]

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
                var userwithId= await _walletService.GetWalledById(walletId);
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
        [HttpGet("getTrans")]
        public async Task<IActionResult> Getall()
        {
            //IUnitOfWork unitOfWork = new IUnitOfWork();
            var wallet = _unitOfWork.Wallet.GetAll();
            return Ok( wallet );
        }
        [HttpPut("ResetPassword")]
        //UPDATE USER PASSWORD
        public async Task<ActionResult<string>> ResetPassword([FromForm] ResetPasswordRequest request)
        {
            try
            {
                var user = await _unitOfWork.User.FindByCondition(u => u.PasswordResetToken == request.Token);
                bool check = true;
                if(user == null)
                {
                    return NotFound("wrong token");
                    check= false;
                }
                foreach (var item in user)
                {
                    if (DateTime.Now > item.ResetTokenExpires)
                    {
                            return BadRequest("token has expired");
                    }
                    break;
                }
                var result =await _userLogic.ResetPassword(request.email, request.Password);
                if (!result)
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
    }
}
