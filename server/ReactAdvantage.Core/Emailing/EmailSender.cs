using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ReactAdvantage.Domain.Emailing
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string to, string subject, string body)
        {
            var email = new MailMessage
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            email.To.Add(new MailAddress(to));
            Send(email);
        }

        public void Send(MailMessage email)
        {
            int port = 25;
            int.TryParse(_configuration["Smtp:Port"], out port);

            var smtpServer = new SmtpClient(_configuration["Smtp:Domain"])
            {
                Port = port,
                Credentials = new System.Net.NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = true
            };
            
            email.Subject = _configuration["Smtp:SubjectPrefix"] + email.Subject?.Replace("\r", "").Replace("\n", "");
            email.From = email.From ?? new MailAddress(_configuration["Smtp:From"]);
            
            try
            {
                smtpServer.Send(email);
                //_logger.Info($"An email sent successfully to {mail.To} with subject {mail.Subject}");
            }
            catch (SmtpException ex)
            {
                //_logger.Error(ex);
                throw;
            }
        }
    }
}
