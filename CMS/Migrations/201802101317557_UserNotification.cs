namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserNotification : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserNotifications", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserNotifications", "Notification_Id", "dbo.Notifications");
            DropIndex("dbo.ApplicationUserNotifications", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserNotifications", new[] { "Notification_Id" });
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        NotificationId = c.Int(nullable: false),
                        WasRead = c.Boolean(nullable: false),
                        WasSeen = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.NotificationId })
                .ForeignKey("dbo.Notifications", t => t.NotificationId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.NotificationId);
            
            DropTable("dbo.ApplicationUserNotifications");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserNotifications",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Notification_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Notification_Id });
            
            DropForeignKey("dbo.UserNotifications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications");
            DropIndex("dbo.UserNotifications", new[] { "NotificationId" });
            DropIndex("dbo.UserNotifications", new[] { "UserId" });
            DropTable("dbo.UserNotifications");
            CreateIndex("dbo.ApplicationUserNotifications", "Notification_Id");
            CreateIndex("dbo.ApplicationUserNotifications", "ApplicationUser_Id");
            AddForeignKey("dbo.ApplicationUserNotifications", "Notification_Id", "dbo.Notifications", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserNotifications", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
