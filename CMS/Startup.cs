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
        SystemHelper system = new SystemHelper();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            system.CreateAdminstrator();
            system.CreateDefaultSettings();
            system.CreateDefaultPages();
            system.CreateDefaultCategory();
            system.CreateCustomPosts();
        }

    }
}
