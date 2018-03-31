using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMS.Models;

namespace CMS.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Settings
        //public ActionResult Index()
        //{
        //    return View(db.Settings.ToList());
        //}

        // GET: Settings/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Setting setting = db.Settings.Find(id);
        //    if (setting == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(setting);
        //}

        // GET: Settings/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Settings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,CompanyName,CompanyEmail,CompanyAddress,CompanyPhone,AboutCompany,FbUrl,ShowFb,TwUrl,ShowTw,GoUrl,ShowGo,YtUrl,ShowYt,LiUrl,ShowLi,InUrl,ShowIn")] Setting setting)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Settings.Add(setting);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(setting);
        //}

        // GET: Settings/Edit/5
        public ActionResult Edit()
        {
            Setting setting = db.Settings.SingleOrDefault();
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // POST: Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyName,CompanyEmail,CompanyAddress,CompanyPhone,AboutCompany,FbUrl,ShowFb,TwUrl,ShowTw,GoUrl,ShowGo,YtUrl,ShowYt,LiUrl,ShowLi,InUrl,ShowIn")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit","Settings");
            }
            return View(setting);
        }

        // GET: Settings/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Setting setting = db.Settings.Find(id);
        //    if (setting == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(setting);
        //}

        // POST: Settings/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Setting setting = db.Settings.Find(id);
        //    db.Settings.Remove(setting);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
