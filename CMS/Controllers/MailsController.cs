using CMS.Helpers;
using CMS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class MailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private SystemHelper system = new SystemHelper(); 
        // GET: Mails
        public ActionResult Index()
        {
            return View(db.Mails.OrderByDescending(m => m.Id).ToList());
        }

        public ActionResult Create(string email)
        {
            Mail mail = new Mail();
            mail.Email = email;
            return View(mail);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mail mail = db.Mails.Find(id);
            if (mail == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            system.MarkMessageRead(id, userId);
            return View(mail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Mail mail)
        {
            if (ModelState.IsValid)
            {
                EmailService email = new EmailService();
                IdentityMessage message = new IdentityMessage();
                message.Destination = mail.Email;
                message.Subject = mail.Name;
                message.Body = mail.Message;
                await email.SendAsync(message);
                return RedirectToAction("Index");
            }
            return View(mail);
        }

        // GET: Pages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mail mail = db.Mails.Find(id);
            if (mail == null)
            {
                return HttpNotFound();
            }
            return View(mail);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mail mail = db.Mails.Find(id);
            db.Mails.Remove(mail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}