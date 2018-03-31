namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class not : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notifications", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notifications", "Type", c => c.String());
        }
    }
}
