namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usernotify : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Notifications", "WasSeen");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "WasSeen", c => c.Boolean(nullable: false));
        }
    }
}
