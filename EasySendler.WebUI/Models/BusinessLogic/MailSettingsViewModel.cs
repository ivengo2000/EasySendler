﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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

        [Required]
        [StringLength(200)]
        public string Pop3 { get; set; }

        [Required]
        [StringLength(200)]
        public string Pop3Port { get; set; }

        [Required]
        [StringLength(200)]
        public string EnableSSL { get; set; }

        [Required]
        [StringLength(200)]
        public string Imap { get; set; }

        [Required]
        [StringLength(200)]
        public string ImapPort { get; set; }
    }
}