using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMS.Models;

namespace CMS.Controllers
{
    [Authorize]
    public class MediaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Media
        public ActionResult Index()
        {
            return View(db.Medias.ToList());
        }

        [HttpPost]
        public ActionResult Index(List<HttpPostedFileBase> files)
        {
            var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
            int numOfUploaded = 0;
            int numOfFaild = 0;
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var fileExtension = extension.ToLower();
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        var uniqe = Guid.NewGuid();
                        string path = Path.Combine(Server.MapPath("~/Uploads"), uniqe + extension);
                        file.SaveAs(path);
                        Media image = new Media();
                        image.FileName = uniqe + extension;
                        image.Date = DateTime.Now.ToString("yyyy-MM-dd");
                        db.Medias.Add(image);
                        db.SaveChanges();
                        numOfUploaded++;
                    }
                    else
                    {
                        numOfFaild++;
                    }
                }
            }
            if (numOfUploaded > 0)
            {
                ViewBag.SuccessUpload = numOfUploaded.ToString() + " File Uploaded Succesfully";
            }
            if (numOfFaild > 0)
            {
                ViewBag.ErrorUpload = numOfFaild.ToString() + " File Not Uploaded Please Select .jpg or .jpeg or .png Files Only";
            }
            return View(db.Medias.ToList());
        }

        // GET: Media/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Media media = db.Medias.Find(id);
            if (media == null)
            {
                return HttpNotFound();
            }
            string ImagePath = Server.MapPath("~/Uploads/" + media.FileName);
            FileInfo fileInfo = new FileInfo(ImagePath);
            decimal size = fileInfo.Length / 1024;
            ViewBag.ImageSize = size;
            Image image = Image.FromFile(ImagePath);
            ViewBag.ImageWidth = image.Width;
            ViewBag.ImageHeight = image.Height;
            return View(media);
        }

        // GET: Media/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Media/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Path,Date")] Media media)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Medias.Add(media);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(media);
        //}

        // GET: Media/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Media media = db.Medias.Find(id);
        //    if (media == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(media);
        //}

        // POST: Media/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Path,Date")] Media media)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(media).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(media);
        //}

        // GET: Media/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Media media = db.Medias.Find(id);
        //    if (media == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(media);
        //}

        // POST: Media/Delete/5
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Media image = db.Medias.Find(id);
            string ImagePath = Server.MapPath("~/Uploads/" + image.FileName);
            if (System.IO.File.Exists(ImagePath) && image != null)
            {
                db.Posts.Where(x => x.MediaId == image.Id).ToList().ForEach(x => x.MediaId = null);
                db.Pages.Where(x => x.MediaId == image.Id).ToList().ForEach(x => x.MediaId = null);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                System.IO.File.Delete(ImagePath);
                db.Medias.Remove(image);
                db.SaveChanges();
            }
            return RedirectToAction("Index","Media");
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
