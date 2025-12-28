namespace MiniNavigator_DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddObjectActionDisplayName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectActions", "DisplayName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectActions", "DisplayName");
        }
    }
}
