using E_Wallet_App.Core.Interface;
using E_Wallet_App.Domain.Dtos;
using E_Wallet_App.Domain.Models;
using E_Wallet_App.Entity.Dtos;
using E_WalletApp.CORE.Interface;
using E_WalletApp.CORE.Interface.RepoInterface;
using E_WalletApp.CORE.Service;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_WalletApp.CORE.Core
{
    public class UserLogic: IUserLogic
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork  _unitOfWork;
        private  readonly IWalletService _wallet;
        private readonly ILoggerManager _logger;
        User user = new User();

        public UserLogic(IUserService userService, ILoggerManager logger, IUserRepository userRepository, IUnitOfWork repositoryWrapper, IWalletService wallet)
        {
            _userService = userService;
            _userRepository = userRepository;
            _unitOfWork = repositoryWrapper;
            _wallet = wallet;
            _logger = logger;
        } 
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
                _userService.CreatepasswordHash(register.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.VerificationToken = await _userService.Generatetoken(register.EmailAddress, register.Role);
                user.ResetTokenExpires = DateTime.Now;
                user.PasswordResetToken = string.Empty;
                //var wallet = new Wallet();
                //wallet.WalletId = await _wallet.GenerateWallet();
                //wallet.Date = DateTime.Now;
                //wallet.Balance = 0;
                //wallet.UserId = user.UserId;

                //_unitOfWork.User.Create(user);
                //_unitOfWork.Wallet.Create(wallet);
                //_unitOfWork.Complete();
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
        public async Task<Wallet> CreateWallet()
        {
            try
            {
                var wallet = new Wallet();
                wallet.WalletId = await _wallet.GenerateWallet();
                wallet.Date = DateTime.Now;
                wallet.Balance = 0;
                wallet.UserId = user.UserId;
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

        public async Task<string> ForgetPassword(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmail(email);
                if (user == null)
                {
                    return null;
                }
                var result = await _userService.Generatetoken(email, user.Role);
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
        public async Task<bool> ResetPassword(string email, string password)
        {
            try
            {
                var user = await _unitOfWork.User.GetByEmail(email.ToLower());
                _userService.CreatepasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                if (user.PasswordHash == passwordHash && user.PasswordSalt == passwordSalt)
                {
                    _unitOfWork.User.Update(user);
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
        public async Task<bool> Login(string email, string password)
        {
            try
            {
                var userCheck = await _unitOfWork.User.GetByEmail(email.ToLower());
                var passCheck = await _userService.VerifypasswordHash(password, userCheck.PasswordHash, userCheck.PasswordSalt);
                if (userCheck != null)
                {
                    if (passCheck)
                    {
                        return true;
                    }
                    return false;
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
    }
}
