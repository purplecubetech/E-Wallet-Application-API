using E_WalletApp.CORE.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace E_WalletApp.CORE.Helper
{

    public class Validation: IValidation
    {
        public async Task<bool> ValidateName(string name)
        {
            var regex = @"^[\p{L} \.\-]+$";
            Regex newRegex = new Regex(regex);
            if (!newRegex.IsMatch(name))
                return false;
            return true;
        }
        public async Task<bool> ValidatePhoneNumber(string phoneNumber)
        {
            var regex = @"^[-+]?[0-9]*\.?[0-9]+$";
            Regex newRegex = new Regex(regex);
            if (!newRegex.IsMatch(phoneNumber) || phoneNumber.Length != 11)
                return false;
            return true;
        }
        public async Task<bool> ValidatePassword(string password)
        {
            //var regex = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$";
            Regex regex1 = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$");
            if (regex1.IsMatch(password))
                return true;
            return false;
        }
        public async Task<bool> ValidateNumber(string amount)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if(regex.IsMatch(amount))
            {
                return true;
            }
            return false;
        }

    }
   
}
