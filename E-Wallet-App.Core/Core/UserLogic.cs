using E_Wallet_App.Core.Interface;
using E_Wallet_App.Domain.Dtos;
using E_Wallet_App.Domain.Models;
using E_Wallet_App.Entity.Dtos;
using E_WalletApp.CORE.Interface;
using E_WalletApp.CORE.Interface.RepoInterface;
using E_WalletApp.CORE.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace E_WalletApp.CORE.Core
{
    public class UserLogic: IUserLogic
    {
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public UserLogic(IUserService userService, ILoggerManager logger, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _logger = logger;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        } 
        public async Task<string> ForgetPassword(string email)
        {
            try
            {
                var user = await _userService.GetByEmail(email);
                if (user == null)
                {
                    return null;
                }
                var result = await Generatetoken(email, user.Role);
                user.PasswordResetToken = result;
                user.ResetTokenExpires = DateTime.Now.AddMinutes(15);
                user.VerifiedAt = DateTime.Now;
                _unitOfWork.User.Update(user);
                _unitOfWork.Complete();
                return (result);
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
        public async Task<string> ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                var checktoken = await _unitOfWork.User.FindByCondition(u => u.PasswordResetToken == request.Token);
                var user = await _userService.GetByEmail(request.email.ToLower());

                if (checktoken == null)
                {
                    return "invalid token";
                }
                foreach (var item in checktoken)
                {
                    if (DateTime.Now > item.ResetTokenExpires)
                    {
                        return "token has expired";
                    }
                    break;
                }
                CreatepasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                if (user.PasswordHash == passwordHash && user.PasswordSalt == passwordSalt)
                {
                    _unitOfWork.User.Update(user);
                    _unitOfWork.Complete();
                    return "password has been reset";
                }
                return "could notr reset password";
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
        public async Task<string> Login(string email, string password)
        {
            try
            {
                var userCheck = await _userService.GetByEmail(email.ToLower());
                var passCheck = await VerifypasswordHash(password, userCheck.PasswordHash, userCheck.PasswordSalt);
                if (userCheck != null)
                {
                    if (passCheck)
                    {
                        var token = await Generatetoken(userCheck.EmailAddress, userCheck.Role);

                        return token;
                    }
                    return "wrong password";
                }
                return "email does not exist";
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
        public async Task<bool> VerifyUser(string token)
        {
            try
            {
                var user = await _unitOfWork.User.FindByCondition(u => u.VerificationToken == token);
                if (user == null)
                {
                    return false;
                }
                foreach (var item in user)
                {
                    item.VerifiedAt = DateTime.Now;
                    item.IsVerified = true;
                }
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
                return false;
            }
        }
        public async Task<string> Generatetoken(string email, string role)
        {
            try
            {
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration
                .GetSection("Jwt:key").Value));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                List<Claim> myclaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role.ToLower() ),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
                var token = new JwtSecurityToken(
                    claims: myclaims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials);
                var jwt = new JwtSecurityTokenHandler().WriteToken(token); 
                return jwt;
            }
            catch (Exception ex)
            {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
                return ex.Message;
            }
        }
        public void CreatepasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public async Task<bool> VerifypasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            try
            {
                using (var hmac = new HMACSHA512(passwordSalt))
                {
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return computedHash.SequenceEqual(passwordHash);
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
                return false;
            }
        }
    }
}