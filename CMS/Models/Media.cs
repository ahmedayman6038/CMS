using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class Media
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Choose File")]
        public string FileName { get; set; }

        public string Date { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}