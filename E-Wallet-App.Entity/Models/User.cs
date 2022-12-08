using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Domain.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public string VerificationToken { get; set; }
        public DateTime VerifiedAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime ResetTokenExpires { get; set; }
    }
}
