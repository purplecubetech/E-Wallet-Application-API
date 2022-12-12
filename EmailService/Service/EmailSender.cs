using E_Wallet_App.Core.Interface;
using E_Wallet_App.Entity.Dtos;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace EmailService.Service
{
    public class EmailSender: IESender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public async Task SendEmailAsync(EmailDto emailDto)
        {
            var emailMessage = CreateEmailMessage(emailDto);
            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailDto emailDto)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_emailConfig.From));
            emailMessage.To.Add(MailboxAddress.Parse(emailDto.To));
            emailMessage.Subject = emailDto.Subject;
            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format("<h2 style='color:red;'>{0}</h2>", emailDto.Body) };
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
            return emailMessage;
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
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
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
