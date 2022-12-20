using E_Wallet_App.Core.Interface;
using E_Wallet_App.Entity.Dtos;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace EmailService.Service
{
    public class EmailSender: IESender
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly ILoggerManager _logger;

        public EmailSender(EmailConfiguration emailConfig, ILoggerManager logger)
        {
            _emailConfig = emailConfig;
            _logger = logger;
        }
        public async Task SendEmailAsync(EmailDto emailDto)
        {
            var emailMessage = await CreateEmailMessage(emailDto);
            await SendAsync(emailMessage);
        }

        private async Task<MimeMessage> CreateEmailMessage(EmailDto emailDto)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(MailboxAddress.Parse(_emailConfig.UserName));
                emailMessage.To.Add(MailboxAddress.Parse(emailDto.To));
                emailMessage.Subject = emailDto.Subject;
                //for text format
                //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format("<h2 style='color:red;'>{0}</h2>", emailDto.Body) };
                
                //for html messages format
                var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", emailDto.Body) };
                if (emailDto.Attachment != null && emailDto.Attachment.Any())
                {
                    byte[] fileBytes;
                    foreach (var attachment in emailDto.Attachment)
                    {
                        using (var memorystream = new MemoryStream())
                        {
                            attachment.CopyTo(memorystream);
                            fileBytes = memorystream.ToArray();
                        }
                        bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                    }
                }
                emailMessage.Body = bodyBuilder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                await smtp.SendAsync(emailMessage);
                smtp.Disconnect(true);
                //smtp.Dispose();
                return emailMessage;
            }
            catch(Exception ex)
            {
                _logger.Debug($"{ex.Message}");
                _logger.Debug($"{ex.StackTrace}");
                _logger.Error($"{ex.InnerException}");
                _logger.Info($"{ex.GetBaseException}");
                _logger.Warn($"{ex.GetObjectData}");
                _logger.Fatal($"{ex.GetHashCode}");
                _logger.Equals($"{ex.TargetSite}");
                return null;
            }
        }
        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                }
                catch(Exception ex)
                {
                    _logger.Debug($"{ex.Message}");
                    _logger.Debug($"{ex.StackTrace}");
                    _logger.Error($"{ex.InnerException}");
                    _logger.Info($"{ex.GetBaseException}");
                    _logger.Warn($"{ex.GetObjectData}");
                    _logger.Fatal($"{ex.GetHashCode}");
                    _logger.Equals($"{ex.TargetSite}");
                    
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }

}
