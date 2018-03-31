using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Models
{
    public class Page
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Author { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        [Display(Name = "Image")]
        public int? MediaId { get; set; }

        public int Visitors { get; set; }

        public bool Activation { get; set; }

        public string Date { get; set; }

        public virtual Media image { get; set; }
    }
}