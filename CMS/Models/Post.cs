using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Author { get; set; }

        [Required][AllowHtml]
        public string Content { get; set; }

        [Display(Name = "category")]
        public int? CategoryId { get; set; }

        [Display(Name = "Image")]
        public int? MediaId { get; set; }

        public string Date { get; set; }

        public virtual Category category { get; set; }

        public virtual Media image { get; set; }
    }
}