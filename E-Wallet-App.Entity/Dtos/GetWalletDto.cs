using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Entity.Dtos
{
    public class GetWalletDto
    {
        public string WalletId { get; set; }
        [Required]
        public double Balance { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
