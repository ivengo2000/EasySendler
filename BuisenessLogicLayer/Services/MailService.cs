using System;
using System.Net;
using System.Net.Mail;
using BuisenessLogicLayer.Abstractions;
using BuisenessLogicLayer.Models;

namespace BuisenessLogicLayer.Services
{
    public class MailService : IMailService
    {
        private readonly MailServerInfo _mailServerInfo;

        public MailService(MailServerInfo mailServerInfo)
        {
            this._mailServerInfo = mailServerInfo;
        }

        public void SendMail(MailMessage message)
        {
            try
            {
                using (var smtpClient = CreateSmtpClient(_mailServerInfo))
                {
                    smtpClient.Send(message);
                }
            }
            catch (Exception ex)
            {
               // Log.Error("MailService.SendMail: ", ex);
            }
        }

        private static SmtpClient CreateSmtpClient(MailServerInfo mailServerInfo)
        {
            string mailServer = mailServerInfo.Name;

            SmtpClient smtpClient;

            if (string.IsNullOrEmpty(mailServer))
            {
                smtpClient = new SmtpClient();
            }
            else
            {
                int mailServerPort = mailServerInfo.Port;
                smtpClient = mailServerPort <= 0 ? new SmtpClient(mailServer) : new SmtpClient(mailServer, mailServerPort);
            }

            smtpClient.UseDefaultCredentials = true;

            string mailServerUserName = mailServerInfo.UserName;
            if (mailServerUserName.Length > 0)
            {
                smtpClient.Credentials = new NetworkCredential(mailServerUserName, mailServerInfo.Password);
            }

            smtpClient.EnableSsl = mailServerInfo.EnableSsl;

            return smtpClient;
        }
    }
}