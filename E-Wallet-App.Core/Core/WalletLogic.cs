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
        private IWalletService _wallet;

        public WalletLogic(IWalletService wallet) 
        {
            _wallet = wallet;
        }
        public async Task<Wallet> CreateWallet()
        {
            var wallet = new Wallet();
            wallet.WalletId = await _wallet.GenerateWallet();
            wallet.Date = DateTime.Now;
            wallet.Balance = 0;
            return wallet;
        }
    }
}
