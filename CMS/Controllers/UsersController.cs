using CMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = (from user in db.Users
                         select new
                         {
                             Id = user.Id,
                             Name = user.UserName,
                             Email = user.Email,
                             Role = (from userRole in user.Roles
                                          join role in db.Roles on userRole.RoleId
                                          equals role.Id
                                          select role.Name).ToList()
                         }).ToList().Select(p => new UserViewModels()
                         {
                             Id = p.Id,
                             Name = p.Name,
                             Email = p.Email,
                             Role = string.Join(",", p.Role)
                         });

            return View(users);
        }

        //// GET: Users/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Users/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Users/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var roles = (from userRole in user.Roles
                         join role in db.Roles on userRole.RoleId
                         equals role.Id
                         select role.Name).FirstOrDefault();
            UserViewModels userView = new UserViewModels()
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Role = roles
            };
            ViewBag.Role = new SelectList(db.Roles.Where(r => !r.Name.Contains("Admin")).ToList(), "Name", "Name", roles);
            return View(userView);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(UserViewModels model)
        {
            // TODO: Add update logic here
            if (ModelState.IsValid)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var user = userManager.FindById(model.Id);
                var oldRoleId = user.Roles.SingleOrDefault().RoleId;
                var oldRoleName = db.Roles.SingleOrDefault(r => r.Id == oldRoleId).Name;
                user.UserName = model.Name;
                user.Email = model.Email;
                var result = userManager.Update(user);
                if (result.Succeeded)
                {
                    db.SaveChanges();
                    if (oldRoleName != model.Role)
                    {
                        userManager.RemoveFromRole(user.Id, oldRoleName);
                        userManager.AddToRole(user.Id, model.Role);
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Name Or Email Alerady Taken");
            }
            ViewBag.Role = new SelectList(db.Roles.Where(r => !r.Name.Contains("Admin")).ToList(), "Name", "Name", model.Role);
            return View(model);

        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var roles = (from userRole in user.Roles
                         join role in db.Roles on userRole.RoleId
                         equals role.Id
                         select role.Name).ToList();
            UserViewModels userView = new UserViewModels()
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Role = string.Join(",",roles)
            };
            return View(userView);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
                // TODO: Add delete logic here
                ApplicationUser user = db.Users.Find(id);
                db.Users.Remove(user);
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
