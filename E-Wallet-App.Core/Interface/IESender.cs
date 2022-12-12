using E_Wallet_App.Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Core.Interface
{
    public interface IESender
    {
        Task SendEmailAsync(EmailDto emailDto);
    }
}
