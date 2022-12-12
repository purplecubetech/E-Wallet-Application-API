using E_Wallet_App.Core.Interface;
using E_Wallet_App.Entity.Dtos;
using EmailService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Wallet_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IESender _emailSender;

        public EmailController(IESender emailSender)
        {
            _emailSender = emailSender;
        }
        [HttpPost("Send-Email")]
        public async Task<IActionResult> SendMail([FromForm]EmailDto emailDto)
        {
            try
            {
               await  _emailSender.SendEmailAsync(emailDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
