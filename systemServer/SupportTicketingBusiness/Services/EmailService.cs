using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using SupportTicketingBusiness.Interface;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace SupportTicketingBusiness.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("TicketEase", _configuration["EmailSettings:SenderEmail"]));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = message };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"], 587, false);
                await client.AuthenticateAsync(_configuration["EmailSettings:SenderEmail"], _configuration["EmailSettings:SenderPassword"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

    }
}
