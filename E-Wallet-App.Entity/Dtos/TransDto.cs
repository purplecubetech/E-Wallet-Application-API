using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Entity.Dtos
{
    public class TransDto
    {
        [Required]
        public string WalletId { get; set; }
        [Required]
        public double amount { get; set; }
    }
}
