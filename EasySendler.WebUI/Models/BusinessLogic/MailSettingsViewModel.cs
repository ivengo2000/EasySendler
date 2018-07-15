using System.ComponentModel.DataAnnotations;

namespace EasySendler.Models.BusinessLogic
{
    public class MailSettingViewModel
    {
        public int MailSettingsId { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string Password { get; set; }

        [Required]
        [StringLength(200)]
        public string SMTP { get; set; }

        [Required]
        [StringLength(200)]
        public string SMTPPort { get; set; }

        [StringLength(200)]
        public string Pop3 { get; set; }

        [StringLength(200)]
        public string Pop3Port { get; set; }

        public bool EnableSsl { get; set; }

        [StringLength(200)]
        public string Imap { get; set; }

        [StringLength(200)]
        public string ImapPort { get; set; }

        public string Description { get; set; }
    }
}