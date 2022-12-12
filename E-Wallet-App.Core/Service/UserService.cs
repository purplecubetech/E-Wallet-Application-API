using E_Wallet_App.Domain.Dtos;
using E_Wallet_App.Domain.Models;
using E_WalletApp.CORE.Interface;
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
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration) 
        {
            _configuration = configuration;
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
                    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    return computedHash.SequenceEqual(passwordHash);
                }
            }
            catch (Exception ex)
            {
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
                return ex.Message;
            }
        }
    }
}
