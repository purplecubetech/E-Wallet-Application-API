using E_Wallet_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Core.Interface
{
    public interface IWalletLogic
    {
        Task<Wallet> CreateWallet();
    }
}
