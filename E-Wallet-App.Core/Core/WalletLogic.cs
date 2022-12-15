using E_Wallet_App.Core.Interface;
using E_Wallet_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Core.Core
{
    public class WalletLogic: IWalletLogic
    {
        public async Task<string> GenerateWallet()
        {
            Guid guid = Guid.NewGuid();
            string uniqueString = Convert.ToBase64String(guid.ToByteArray());
            uniqueString = uniqueString.Replace("=", "");
            uniqueString = uniqueString.Replace("+", "");
            return uniqueString;
        }
    }
}
