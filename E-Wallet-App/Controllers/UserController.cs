using E_Wallet_App.Core.Interface;
using E_Wallet_App.Domain.Dtos;
using E_Wallet_App.Domain.Models;
using E_Wallet_App.Entity.Dtos;
using E_WalletApp.CORE.Core;
using E_WalletApp.CORE.Interface;
using E_WalletApp.CORE.Interface.RepoInterface;
using E_WalletRepository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace E_Wallet_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserLogic _userLogic;
        private readonly IWalletLogic _wallet;
        private readonly IWalletRepository _walletRepository;

        public UserController(IUserService userService, IWalletRepository walletRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, IUserLogic userLogic, IWalletLogic wallet)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userLogic = userLogic;
            _wallet = wallet;
            _walletRepository = walletRepository;
        }
        [HttpPost("RegisterUser")]
        public async Task<ActionResult<Register>> RegisterUser([FromForm] Register register)
        {
            //var userName = new UserDto();
            try
            {
                var user = await _userRepository.GetByEmail(register.EmailAddress);
                if (user != null)
                {
                    return BadRequest($"{register.EmailAddress} already exixts");
                }
                else if(register.Password != register.ComfirmPassword)
                {
                    return BadRequest("password does not match");
                }
                else
                {
                    var output = await _userLogic.RegisterUser(register);
                    var newwallet = await _userLogic.CreateWallet();
                    _unitOfWork.User.Create(output);
                    _unitOfWork.Wallet.Create(newwallet);
                    _unitOfWork.Complete();
                    return Ok($"{output}/n account registered");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("verifyUser")]
        public async Task<ActionResult> VerifyUser([FromForm]string token)
        {
            var user = await _unitOfWork.User.FindByCondition(u => u.VerificationToken== token);
            if(user == null)
            {
                return BadRequest("user not verified");
            }
            foreach(var item in user)
            {
                item.VerifiedAt= DateTime.Now;
            }
            _unitOfWork.Complete();

            return Ok("user verified");
        }
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login([FromForm] Login user)
        {
            try
            {
                var currentuser = await _userRepository.GetByEmail(user.Email);

                if (currentuser != null)
                {
                    var check = await _userLogic.Login(user.Email, user.Password);
                    if (check)
                    {
                        return Ok($"you are logged in");
                    }
                    return BadRequest("wrong password");
                }
                return BadRequest("wrong email");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetAllUser")]
        public async Task<ActionResult> GetAllUser()
        {
            try
            {
                var alluser = await _unitOfWork.User.GetAll();
                if(alluser == null)
                {
                    return NotFound("no user was found");
                }
                var getAllUserDto = new List<GetUserDto>();
                foreach(var users in alluser)
                {
                    var user = new GetUserDto();
                    user.FirstName = users.FirstName;
                    user.LastName = users.LastName;
                    user.EmailAddress = users.EmailAddress;
                    user.DOB = users.DOB;
                    user.Gender = users.Gender;
                    user.DateCreated = users.DateCreated;
                    user.Role= users.Role;
                    getAllUserDto.Add(user);
                }
                
                return Ok(getAllUserDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetUserById")]
        public async Task<ActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = _unitOfWork.User.GetById(id);
                if(user == null)
                {
                    return NotFound("user not found");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetUserByWalletId")]
        public async Task<ActionResult> GetUserByWalletId(string walletId)
        {
            try
            {
                var userwithId= await _walletRepository.GetByWalletId(walletId);
                if (userwithId == null)
                {
                    return NotFound("user not found");
                }
                var user = await _unitOfWork.Wallet.FindByCondition(x => x.UserId == userwithId.UserId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("ForGotPassword")]
        public async Task<ActionResult<string>> ForgotPassword([FromForm]string email)
        {
            try
            {
                var user = await _userRepository.GetByEmail(email);
                if (user == null)
                {
                    return BadRequest("your email does not exist");
                }
                await _userLogic.ForgetPassword(email);
                return Ok($"use this token {user.PasswordResetToken} to reset your password");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
                return StatusCode(500, ex.Message);
            }
        }
    }
}
