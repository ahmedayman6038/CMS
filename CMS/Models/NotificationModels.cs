using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public string Date { get; set; }
        public virtual ICollection<UserNotification> userNotification { get; set; }
    }

    public class UserNotification
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }
        [Key, Column(Order = 1)]
        public int NotificationId { get; set; }
        public bool WasRead { get; set; }
        public bool WasSeen { get; set; }
        public virtual ApplicationUser user { get; set; }
        public virtual Notification notification { get; set; }
    }

    public class NotificationViewModels
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
        public int ItemId { get; set; }
        public bool WasRead { get; set; }
        public bool WasSeen { get; set; }
    }

    public enum NotificationType
    {
        Message,
        Alert
    }

    public class Alert
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}