using E_Wallet_App.Core.Interface;
using E_Wallet_App.Entity.Dtos;
using EmailService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace E_Wallet_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IESender _emailSender;
        private readonly ILoggerManager _logger;

        public EmailController([FromForm]IESender emailSender, ILoggerManager logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }
        [HttpPost("Send-Email")]
       // [Authorize(Roles = "admin")]

        public async Task<IActionResult> SendMail([FromForm] EmailRequest emailRequest)
        {
            try
            {
               var emailDto = new EmailDto(emailRequest.To, emailRequest.Subject, emailRequest.Body, emailRequest.Attachment);
               await  _emailSender.SendEmailAsync(emailDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
