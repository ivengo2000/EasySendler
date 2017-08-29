using System.Net.Mail;
using BuisenessLogicLayer.Models;

namespace BuisenessLogicLayer.Abstractions
{
    public interface IMailService
    {
        string SendMail(MailMessage message);

        void SetSmtpSettings(MailServerInfo mailServerInfo);
    }
}