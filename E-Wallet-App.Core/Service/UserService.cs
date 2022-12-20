using E_Wallet_App.Core.Interface;
using E_Wallet_App.Domain.Dtos;
using E_Wallet_App.Domain.Models;
using E_Wallet_App.Entity.Dtos;
using E_Wallet_App.Entity.Helper;
using E_WalletApp.CORE.Core;
using E_WalletApp.CORE.Interface;
using E_WalletApp.CORE.Interface.RepoInterface;
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

namespace E_WalletApp.CORE.Service
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        // private readonly IUserLogic _userLogic;
        private readonly IWalletLogic _walletLogic;
        private readonly ILoggerManager _logger;

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration, IWalletLogic walletLogic, ILoggerManager logger) 
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            //_userLogic = userLogic;
            _walletLogic = walletLogic;
            _logger = logger;
        }
        User user = new User();
        public async Task<User> RegisterUser(Register register)
        {
            try
            {
                user.UserId = Guid.NewGuid();
                user.DateCreated = DateTime.Now;
                user.FirstName = register.FirstName.ToLower();
                user.LastName = register.LastName.ToLower();
                user.PhoneNumber = register.PhoneNumber;
                user.EmailAddress = register.EmailAddress.ToLower();
                user.Role = register.Role.ToLower();
                user.DOB = register.DOB;
                user.Gender = register.Gender.ToLower();
                CreatepasswordHash(register.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.VerificationToken = await Generatetoken(register.EmailAddress, register.Role);
                //user.ResetTokenExpires = DateTime.Now;
                //user.PasswordResetToken = string.Empty;

                var wallet = new Wallet();
                wallet.WalletId = await _walletLogic.GenerateWallet();
                wallet.Date = DateTime.Now;
                wallet.Balance = 0;
                wallet.UserId = user.UserId;

                _unitOfWork.User.Create(user);
                _unitOfWork.Wallet.Create(wallet);
                _unitOfWork.Complete();
                return user;
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
        public async Task<PageList<GetUserDto>> GetAllUser(PaginationParameter pagin)
        {
            try
            {
                var alluser = await _unitOfWork.User.GetAll();
                if (alluser == null)
                {
                    return null;
                }
                var getAllUserDto = new List<GetUserDto>();
                foreach (var users in alluser)
                {
                    var user = new GetUserDto();
                    user.FirstName = users.FirstName;
                    user.LastName = users.LastName;
                    user.EmailAddress = users.EmailAddress;
                    user.DOB = users.DOB;
                    user.Gender = users.Gender;
                    user.DateCreated = users.DateCreated;
                    user.Role = users.Role;
                    getAllUserDto.Add(user);
                }
                return PageList<GetUserDto>.ToPageList(getAllUserDto, pagin.PageNumber, pagin.PageSize);

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
        public async Task<GetUserDto>  GetUserById(Guid id)
        {
            try
            {
                var user = await _unitOfWork.User.GetById(id);
                var getUser = new GetUserDto();

                if (user == null)
                {
                    return null;
                }
                getUser.FirstName = user.FirstName;
                getUser.LastName = user.LastName;
                getUser.EmailAddress = user.EmailAddress;
                getUser.DOB = user.DOB;
                getUser.Gender = user.Gender;
                getUser.DateCreated = user.DateCreated;
                getUser.Role = user.Role;
                
                return getUser;
            }
            catch (Exception ex)
            {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
                return  null;
            }
        }
        public async Task<User> GetByEmail(string email)
        {
            try
            {
                var allusers = await _unitOfWork.User.FindByCondition(x => x.EmailAddress == email.ToLower());
                var user = new User();

                if (allusers == null)
                {
                    return null;
                }
                foreach (var item in allusers)
                {
                    user.UserId = item.UserId;
                    user.FirstName = item.FirstName;
                    user.LastName = item.LastName;
                    user.EmailAddress = item.EmailAddress;
                    user.DOB = item.DOB;
                    user.Role = item.Role;
                    user.Gender = item.Gender;
                    user.DateCreated = item.DateCreated;
                    user.PhoneNumber = item.PhoneNumber;
                    user.PasswordHash = item.PasswordHash;
                    user.PasswordSalt = item.PasswordSalt;
                    user.VerificationToken = item.VerificationToken;
                    user.VerifiedAt = item.VerifiedAt;
                    user.PasswordResetToken = item.PasswordResetToken;
                    user.ResetTokenExpires = item.ResetTokenExpires;
                }
                return user;
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
        //public void DeleteAll(Guid id)
        //{
        //    try
        //    {
        //        var user = GetUserById(id);
        //        var wallet = 
        //        if (user != null) { }
        //        _unitOfWork.User.Delete(user);
        //        _unitOfWork.Wallet.DeleteRange(wallet);
        //        _unitOfWork.Transaction.DeleteRange(transaction);
        //        _unitOfWork.Complete();

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        public void CreatepasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
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
                new Claim(ClaimTypes.Role, role.ToLower()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };
                var token = new JwtSecurityToken(
                    claims: myclaims,
                    expires: DateTime.Now.AddMinutes(15),
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
    }
}