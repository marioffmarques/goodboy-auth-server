using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace Authorization.Email
{
    public class EmailSender
    {
        public static void SendRecoverPasswordEmail(string emailTo, RecoverPasswordEmailSettings recoverPasswordEmailSettings, SmtpSettings smtpSettings)
        {
            BodyBuilder bodyBuilder = null;
            if (!string.IsNullOrEmpty(recoverPasswordEmailSettings.Body)) 
            {
                bodyBuilder = new BodyBuilder { HtmlBody = recoverPasswordEmailSettings.Body };
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtpSettings.Email));
            message.To.Add(new MailboxAddress(emailTo));
            message.Subject = recoverPasswordEmailSettings.Subject;
            message.Body = bodyBuilder.ToMessageBody();

            var c = new SmtpClient();
            c.Connect(smtpSettings.Host, smtpSettings.Port);
            c.Authenticate(smtpSettings.Email, smtpSettings.Password);
            c.Send(message);
            //c.Disconnect(true);
        }
    }
}
