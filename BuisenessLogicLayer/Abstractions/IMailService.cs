using System.Net.Mail;

namespace BuisenessLogicLayer.Abstractions
{
    public interface IMailService
    {
        void SendMail(MailMessage message);
    }
}