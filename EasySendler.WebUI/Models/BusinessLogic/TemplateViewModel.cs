using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasySendler.Models.BusinessLogic
{
    public class TemplateViewModel
    {
        public int TemplateId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        //public HtmlString Body { get; set; }

        public byte[] Thumbnail { get; set; }
    }
}