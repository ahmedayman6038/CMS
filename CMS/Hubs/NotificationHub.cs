using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace CMS.Hubs
{
    public class NotificationHub : Hub
    {
        public void MessageNotify(int itemId, string title, string content, string date)
        {
            Clients.All.sendMessageNotification(itemId, title, content, date);
        }

        public void AlertNotify(int itemId, string title, string content, string date)
        {
            Clients.All.sendAlertNotification(itemId, title, content, date);
        }
    }
}