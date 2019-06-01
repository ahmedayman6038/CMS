using CMS.Helpers;
using CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace CMS.Controllers
{
    public class HomeController : Controller
    {
        private SystemHelper system = new SystemHelper();
        private AccountController account = new AccountController();
            
        public ActionResult Index()
        {
            ViewBag.Projects = system.GetPostsOfCategory("Projects").OrderByDescending(x => x.Id).Take(3);
            LayoutDataView();
            return View();
        }

        public ActionResult About()
        {
            var about = system.GetPostsOfCategory("About").FirstOrDefault();
            LayoutDataView();
            return View(about);
        }

        public ActionResult Services()
        {
            LayoutDataView();
            return View();
        }

        public ActionResult Projects(int? id,int? page)
        {
            if (id == null)
            {
                int pageSize = 6;
                int pageNumber = (page ?? 1);
                var projects = system.GetPostsOfCategory("Projects");
                LayoutDataView();
                return View(projects.ToPagedList(pageNumber, pageSize));
            }
            Post post = system.FindPostOfCategory("Projects", id);
            if (post == null)
            {
                return HttpNotFound();
            }
            LayoutDataView();
            return View("Project", post);
        }


        public ActionResult Blog()
        {
            LayoutDataView();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Contact = system.GetSettings();
            LayoutDataView();
            return View();
        }

        public ActionResult Pages(string page, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page_show = system.GetPageById(id);
            if (page_show == null)
            {
                return HttpNotFound();
            }
            LayoutDataView();
            return View(page_show);
        }

        [HttpPost]
        public async Task<ActionResult> SendMail(Mail mail)
        {
            var success = "<div class=\"alert alert-success alert-dismissible fade show\" role=\"alert\">" +
                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\">" +
                "<span aria-hidden=\"true\">&times;</span></button>" +
                "<strong> Thanks!</strong> The message successfully send.</div>";
            var faild = "<div class=\"alert alert-danger alert-dismissible fade show\" role=\"alert\">" +
                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\">" +
                "<span aria-hidden=\"true\">&times;</span></button>" +
                "<strong> Oops!</strong> Some thing goes wrong please try again.</div>";
             if (ModelState.IsValid)
             {
                mail.Date = DateTime.Now.ToString();
                mail.Message = String.Format($"From: {mail.Email} <br> Message: {mail.Message}");
                bool check = await system.SendEmailAsync(mail);

                if (check)
                {
                    return Content(success);
                }
             }
            return Content(faild);
        }

        private void LayoutDataView()
        {
            ViewBag.defaultPages = system.GetSystemPages();
            ViewBag.Pages = system.GetActivePages();
            ViewBag.Settings = system.GetSettings();
        }

    }
}