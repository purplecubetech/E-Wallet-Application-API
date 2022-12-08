using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace E_WalletApp.CORE.Interface
{
    public interface IValidation
    {
        Task<bool> ValidateName(string name);
        Task<bool> ValidatePhoneNumber(string phoneNumber);
        Task<bool> ValidatePassword(string password);
        Task<bool> ValidateNumber(string amount);
    }
    
}
