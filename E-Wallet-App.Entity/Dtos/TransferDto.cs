using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Entity.Dtos
{
    public class TransferDto
    {
        [Required]
        public string FromWallet { get; set; }
        [Required]
        public string ToWallet { get; set; }
        [Required]
        public double Amount { get; set; }

    }
}
