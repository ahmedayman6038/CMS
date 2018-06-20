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

        public async Task<bool> SendMailToAdmins(Mail mail)
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
                message.Subject = mail.Name;
                message.Body = mail.Message;
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
            if (AddMail(mail) && await SendMailToAdmins(mail))
            {          
                return true;
            }
            return false;
        }

        public void CreateAdminstrator()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (!roleManager.RoleExists("Admin"))
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.Create(role);
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "ahmed",
                    Email = "ahmedayman6038@gmail.com",
                    EmailConfirmed = true
                };
                string password = "ahmed123";
                var result = userManager.Create(user, password);
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
        }

        public void CreateDefaultSettings()
        {
            var check = db.Settings.SingleOrDefault();
            if (check == null)
            {
                Setting setting = new Setting
                {
                    CompanyName = "Company Site"
                };
                db.Settings.Add(setting);
                db.SaveChanges();
            }
        }

        public void CreateDefaultPages()
        {
            var check = db.Pages.FirstOrDefault();
            if (check == null)
            {
                SystemHelper system = new SystemHelper();
                List<string> pages = new List<string>();
                pages = system.GetSystemPages();
                foreach (var item in pages)
                {
                    Page page = new Page
                    {
                        Name = item,
                        Author = "Admin",
                        Activation = true,
                        Date = DateTime.Now.ToString("yyyy-MM-dd"),
                        Visitors = 1
                    };
                    db.Pages.Add(page);
                    db.SaveChanges();
                }
            }
        }

        public void CreateDefaultCategory()
        {
            var check = db.Categories.FirstOrDefault();
            if (check == null)
            {
                Category category = new Category()
                {
                    Name = "Uncategorized",
                    Description = "This is a default category of item"
                };
                db.Categories.Add(category);
                db.SaveChanges();
            }
        }

        public void CreateCustomPosts()
        {
            var check1 = db.Categories.Where(x => x.Name.ToLower() == "about").FirstOrDefault();
            var check2 = db.Categories.Where(x => x.Name.ToLower() == "projects").FirstOrDefault();
            if (check1 == null)
            {
                Category category = new Category()
                {
                    Name = "about",
                    Description = "Posts that describe the company"
                };
                db.Categories.Add(category);
                db.SaveChanges();
                Post post = new Post()
                {
                    Title = "More About Us",
                    CategoryId = category.Id,
                    Content = "<p>test content test content</p>",
                    Date = DateTime.Now.ToString("yyyy-MM-dd")
            };
                db.Posts.Add(post);
                db.SaveChanges();
            }
            if (check2 == null)
            {
                Category category = new Category()
                {
                    Name = "projects",
                    Description = "Posts that show projects"
                };
                db.Categories.Add(category);
                db.SaveChanges();
                Post post1 = new Post()
                {
                    Title = "Project 1",
                    CategoryId = category.Id,
                    Content = "<p>test content test content</p>",
                    Date = DateTime.Now.ToString("yyyy-MM-dd")
                };
                Post post2 = new Post()
                {
                    Title = "Project 2",
                    CategoryId = category.Id,
                    Content = "<p>test content test content</p>",
                    Date = DateTime.Now.ToString("yyyy-MM-dd")
                };
                Post post3 = new Post()
                {
                    Title = "Project 3",
                    CategoryId = category.Id,
                    Content = "<p>test content test content</p>",
                    Date = DateTime.Now.ToString("yyyy-MM-dd")
                };
                db.Posts.Add(post1);
                db.Posts.Add(post2);
                db.Posts.Add(post3);
                db.SaveChanges();
            }
        }
    }
}