namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        NotificationId = c.Int(nullable: false),
                        WasRead = c.Boolean(nullable: false),
                        WasSeen = c.Boolean(nullable: false),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.NotificationId })
                .ForeignKey("dbo.Notifications", t => t.NotificationId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.NotificationId)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.ApplicationUserNotifications",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Notification_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Notification_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Notifications", t => t.Notification_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Notification_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserNotifications", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications");
            DropForeignKey("dbo.ApplicationUserNotifications", "Notification_Id", "dbo.Notifications");
            DropForeignKey("dbo.ApplicationUserNotifications", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserNotifications", new[] { "Notification_Id" });
            DropIndex("dbo.ApplicationUserNotifications", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.UserNotifications", new[] { "user_Id" });
            DropIndex("dbo.UserNotifications", new[] { "NotificationId" });
            DropTable("dbo.ApplicationUserNotifications");
            DropTable("dbo.UserNotifications");
        }
    }
}
