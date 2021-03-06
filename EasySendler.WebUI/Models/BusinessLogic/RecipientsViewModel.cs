﻿using System.ComponentModel.DataAnnotations;

namespace EasySendler.Models.BusinessLogic
{
    public class RecipientsViewModel
    {
        public int rId { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string SureName { get; set; }
    }
}