namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class star : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mails", "star", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mails", "star");
        }
    }
}
