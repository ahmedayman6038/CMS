using Microsoft.Owin;
using Owin;
using CMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System;
using CMS.Helpers;
using System.Collections.Generic;

[assembly: OwinStartupAttribute(typeof(CMS.Startup))]
namespace CMS
{
    public partial class Startup
    {
        ApplicationDbContext db = new ApplicationDbContext();
       
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            CreateAdminstrator();
            CreateDefaultSettings();
            CreateDefaultPages();
            CreateDefaultCategory();
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
                    Email = "ahmedayman6038@gmail.com"
                };
                string password = "@7maD101";
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
            if(check == null)
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
            if(check == null)
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
            if(check == null)
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
    }
}
