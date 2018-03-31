namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class path : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mails", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Mails", "Email", c => c.String(nullable: false));
            DropColumn("dbo.Mails", "FromName");
            DropColumn("dbo.Mails", "FromEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Mails", "FromEmail", c => c.String(nullable: false));
            AddColumn("dbo.Mails", "FromName", c => c.String(nullable: false));
            DropColumn("dbo.Mails", "Email");
            DropColumn("dbo.Mails", "Name");
        }
    }
}
