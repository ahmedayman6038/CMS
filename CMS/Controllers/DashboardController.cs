using CMS.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private SystemHelper system = new SystemHelper();
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMessages(int pageIndex, int pageSize, int discard)
        {
            //System.Threading.Thread.Sleep(2000);
            var userId = User.Identity.GetUserId();
            var messages = system.GetUserMessages(pageIndex, pageSize, discard, userId);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public void SeenMessages()
        {
            var userId = User.Identity.GetUserId();
            system.MarkMessagesSeen(userId);
        }

        public ActionResult GetAlerts(int pageIndex, int pageSize, int discard)
        {
            //System.Threading.Thread.Sleep(2000);
            var userId = User.Identity.GetUserId();
            var alerts = system.GetUserAlerts(pageIndex, pageSize, discard, userId);
            return Json(alerts, JsonRequestBehavior.AllowGet);
        }

        public void SeenAlerts()
        {
            var userId = User.Identity.GetUserId();
            system.MarkAlertsSeen(userId);
        }

    }
}