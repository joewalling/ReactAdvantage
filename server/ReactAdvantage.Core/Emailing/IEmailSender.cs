using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace ReactAdvantage.Domain.Emailing
{
    public interface IEmailSender
    {
        void Send(string to, string subject, string body);
        void Send(MailMessage email);
    }
}
