using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Entity.Dtos
{
    public class EmailDto
    {
        [Required]
        public string To { get; set; }
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public IFormFileCollection Attachment { get; set; }

        public EmailDto(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
            //Attachment = attachment;
        }
    }
}
