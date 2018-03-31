using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Models
{
    public class Setting
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Comapny Email")]
        public string CompanyEmail { get; set; }

        [Display(Name = "Company Address")]
        public string CompanyAddress { get; set; }

        [Display(Name = "Company Phone")]
        public string CompanyPhone { get; set; }

        [Display(Name = "About Company")]
        [DataType(DataType.MultilineText)]
        public string AboutCompany { get; set; }

        [Display(Name = "Facebook Url")]
        public string FbUrl { get; set; }

        [Display(Name = "Show Facebook")]
        public bool ShowFb { get; set; }

        [Display(Name = "Twitter Url")]
        public string TwUrl { get; set; }

        [Display(Name = "Show Twitter")]
        public bool ShowTw { get; set; }

        [Display(Name = "Google Url")]
        public string GoUrl { get; set; }

        [Display(Name = "Show G+")]
        public bool ShowGo { get; set; }

        [Display(Name = "Youtube Url")]
        public string YtUrl { get; set; }

        [Display(Name = "Show Youtube")]
        public bool ShowYt { get; set; }

        [Display(Name = "Linkedin Url")]
        public string LiUrl { get; set; }

        [Display(Name = "Show Linkedin")]
        public bool ShowLi { get; set; }

        [Display(Name = "Instagram Url")]
        public string InUrl { get; set; }

        [Display(Name = "Show Instagram")]
        public bool ShowIn { get; set; }
    }
}