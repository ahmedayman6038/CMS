namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class play : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications");
            DropForeignKey("dbo.UserNotifications", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserNotifications", new[] { "UserId" });
            DropIndex("dbo.UserNotifications", new[] { "NotificationId" });
            DropTable("dbo.UserNotifications");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        NotificationId = c.Int(nullable: false),
                        WasRead = c.Boolean(nullable: false),
                        WasSeen = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.NotificationId });
            
            CreateIndex("dbo.UserNotifications", "NotificationId");
            CreateIndex("dbo.UserNotifications", "UserId");
            AddForeignKey("dbo.UserNotifications", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications", "Id", cascadeDelete: true);
        }
    }
}
