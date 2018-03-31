using CMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Helpers
{
    public class SystemHelper
    {
        private ApplicationDbContext db;
        private NotificationHelper notification;

        public SystemHelper()
        {
            db = new ApplicationDbContext();
            notification = new NotificationHelper();
        }

        public Setting GetSettings()
        {
            return db.Settings.SingleOrDefault();
        }

        public List<Post> GetPostsOfCategory(string category)
        {
            return db.Posts.Where(p => p.category.Name.ToLower() == category.ToLower()).ToList();
        }

        public Post FindPostOfCategory(string category,int? id)
        {
            return db.Posts.Where(p => p.category.Name.ToLower() == category.ToLower()).Where(p => p.Id == id).FirstOrDefault();
        }

        public List<string> GetSystemPages()
        {
            return new List<string>()
            {
                "Home","About","Blog","Contact","Services","Projects"
            };
        }

        public List<Page> GetAllPages()
        {
            return db.Pages.ToList();
        }

        public List<Page> GetActivePages()
        {
            return db.Pages.Where(p => p.Activation == true).ToList();
        }

        public Page GetPageById(int? id)
        {
            return db.Pages.Find(id);
        }

        public bool AddMail(Mail mail)
        {
            db.Mails.Add(mail);
            db.SaveChanges();
            notification.PushNotification(mail);
            return true;
        }
       
        public List<NotificationViewModels> GetUserMessages(int pageIndex, int pageSize, int discard, string userId)
        {
            var notfications = notification.GetMessageNotifications().OrderByDescending(n => n.Id).Skip((pageIndex * pageSize) + discard).Take(pageSize).ToList();
            var userNotifications = db.UserNotifications.Where(n => n.UserId == userId).ToList();
            var messages = (from un in userNotifications
                           join n in notfications on un.NotificationId equals n.Id
                           orderby n.Id descending
                           select new NotificationViewModels
                           {
                               Title = n.Title,
                               Content = n.Content,
                               Date = n.Date,
                               ItemId = n.ItemId,
                               WasRead = un.WasRead,
                               WasSeen = un.WasSeen
                           }).ToList();
            return messages;
        }

        public List<NotificationViewModels> GetUserAlerts(int pageIndex, int pageSize, int discard, string userId)
        {
            var notfications = notification.GetAlertNotifications().OrderByDescending(n => n.Id).Skip((pageIndex * pageSize) + discard).Take(pageSize).ToList();
            var userNotifications = db.UserNotifications.Where(n => n.UserId == userId).ToList();
            var alerts = (from un in userNotifications
                            join n in notfications on un.NotificationId equals n.Id
                            orderby n.Id descending
                            select new NotificationViewModels
                            {
                                Title = n.Title,
                                Content = n.Content,
                                Date = n.Date,
                                ItemId = n.ItemId,
                                WasRead = un.WasRead,
                                WasSeen = un.WasSeen
                            }).ToList();
            return alerts;
        }

        public void MarkMessagesSeen(string userId)
        {
            var notfications = notification.GetMessageNotifications();
            var userNotifications = db.UserNotifications.Where(u => u.UserId == userId).ToList();
            var messages = (from un in userNotifications
                            join n in notfications on un.NotificationId equals n.Id
                            select un).ToList();
            messages.ForEach(n => n.WasSeen = true);
            db.SaveChanges();
        }

        public void MarkAlertsSeen(string userId)
        {
            var notfications = notification.GetAlertNotifications();
            var userNotifications = db.UserNotifications.Where(u => u.UserId == userId).ToList();
            var messages = (from un in userNotifications
                            join n in notfications on un.NotificationId equals n.Id
                            select un).ToList();
            messages.ForEach(n => n.WasSeen = true);
            db.SaveChanges();
        }

        public void MarkMessageRead(int? id,string userId)
        {
            Notification notify = db.Notifications.Where(n => n.ItemId == id).FirstOrDefault();
            UserNotification userNotification = db.UserNotifications.Where(u => u.UserId == userId && u.NotificationId == notify.Id).FirstOrDefault();
            userNotification.WasRead = true;
            userNotification.WasSeen = true; 
            db.Entry(userNotification).State = EntityState.Modified;
            db.SaveChanges();
        }

        public async Task<bool> SendMailToAdmins(string body)
        {
            var admins = (from user in db.Users
                          from userRole in user.Roles
                          join role in db.Roles on userRole.RoleId
                          equals role.Id
                          where role.Name.ToLower() == "admin"
                          select user.Id).ToList();
            foreach (var id in admins)
            {
                EmailService email = new EmailService();
                IdentityMessage message = new IdentityMessage();
                message.Destination = db.Users.Find(id).Email;
                message.Subject = "CMS";
                message.Body = body;
                await email.SendAsync(message);
            }
            return true;
        }

        public async Task<bool> SendEmailAsync(Mail mail)
        {
            var check = db.Mails.Where(m => m.Email == mail.Email).Where(m => m.Message == mail.Message).ToList();
            if (check.Count > 0)
            {
                return true;
            }
            if (AddMail(mail) && await SendMailToAdmins(mail.Message))
            {          
                return true;
            }
            return false;
        }
    }
}