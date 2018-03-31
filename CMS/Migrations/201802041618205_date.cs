namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Media", "FileName", c => c.String(nullable: false));
            AddColumn("dbo.Mails", "Date", c => c.String());
            DropColumn("dbo.Media", "Path");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Media", "Path", c => c.String(nullable: false));
            DropColumn("dbo.Mails", "Date");
            DropColumn("dbo.Media", "FileName");
        }
    }
}
