using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Domain.Dtos
{
    public class Register
    {
        [Required]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string LastName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "password must contain an UPPERCASE, LOWERCASE, NUMBER  characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "password must contain an UPPERCASE, LOWERCASE, NUMBER  characters")]
        public string ComfirmPassword { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public string Gender { get; set; }
    }
}
