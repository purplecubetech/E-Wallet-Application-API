using E_Wallet_App.Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Core.Interface
{
    public interface ITransLogic
    {
        Task<bool> Deposite(TransDto transDto);
        Task<bool> Withdrawal(TransDto transDto);
        Task<bool> Transfer(TransferDto transferDto);
        Task<BalanceDto> GetBalance(string walletid, string currency);
        Task<double> GetCurrencyApi(string currency);


    }
}
