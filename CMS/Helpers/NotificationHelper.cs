using CMS.Hubs;
using CMS.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db;

        public NotificationHelper()
        {
            db = new ApplicationDbContext();
        }

        public int AddNotification(Notification notification)
        {
            db.Notifications.Add(notification);
            db.SaveChanges();
            return notification.Id;
        }

        public bool RemoveNotification(int id)
        {
            IEnumerable<UserNotification> list = db.UserNotifications.Where(x => x.NotificationId == id).ToList();
            db.UserNotifications.RemoveRange(list);
            Notification notification = db.Notifications.Find(id);
            db.Notifications.Remove(notification);
            db.SaveChanges();
            return true;
        }

        public bool AddUserNotification(UserNotification userNotification)
        {
            db.UserNotifications.Add(userNotification);
            db.SaveChanges();
            return true;
        }

        public List<Notification> GetMessageNotifications()
        {
            return db.Notifications.Where(n => n.Type == (int)NotificationType.Message).ToList();
        }

        public List<Notification> GetAlertNotifications()
        {
            return db.Notifications.Where(n => n.Type == (int)NotificationType.Alert).ToList();
        }

        public void PushNotification(Object obj)
        {
            Notification notification = new Notification();
            if(obj is Mail)
            {
                Mail mail = new Mail();
                mail = (Mail)obj;
                notification.ItemId = mail.Id;
                notification.Title = mail.Name;
                notification.Content = mail.Message;
                notification.Type = (int)NotificationType.Message;
            }
            else if(obj is Alert)
            {
                Alert alert = new Alert();
                alert = (Alert)obj;
                notification.Title = alert.Title;
                notification.Content = alert.Message;
                notification.Type = (int)NotificationType.Alert;
            }
            else
            {
                return;
            }
            notification.Date = DateTime.Now.ToString();
            int notificationId = AddNotification(notification);
            var users = db.Users.Select(u => u.Id).ToList();
            foreach (var id in users)
            {
                UserNotification userNotification = new UserNotification();
                userNotification.UserId = id;
                userNotification.NotificationId = notificationId;
                AddUserNotification(userNotification);
            }
            Notify(notification);
        }

        public void Notify(Notification notification)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            switch (notification.Type)
            {
                case 0:
                    hubContext.Clients.All.sendMessageNotification(notification.ItemId,
                        notification.Title,
                        notification.Content,
                        notification.Date);
                    break;
                case 1:
                    hubContext.Clients.All.sendAlertNotification(notification.ItemId,
                        notification.Title,
                        notification.Content,
                        notification.Date);
                    break;
                default:
                    break;
            }

        }
    }
}