using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Domain.Models
{
    [Table("Wallets")]

    public class Wallet
    {
  
            [Key]
            public string WalletId { get; set; }
            [Required]
            public double Balance { get; set; }
            [Required]
            public DateTime Date { get; set; }
            [ForeignKey(nameof(User))]
            public Guid UserId { get; set; }
            public User User { get; set; }  
    }
}
