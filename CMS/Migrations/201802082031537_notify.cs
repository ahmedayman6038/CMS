namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notify : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "ItemId", c => c.Int(nullable: false));
            AddColumn("dbo.Notifications", "WasSeen", c => c.Boolean(nullable: false));
            AddColumn("dbo.Notifications", "Type", c => c.String());
            DropColumn("dbo.Notifications", "Readed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "Readed", c => c.Boolean(nullable: false));
            DropColumn("dbo.Notifications", "Type");
            DropColumn("dbo.Notifications", "WasSeen");
            DropColumn("dbo.Notifications", "ItemId");
        }
    }
}
