namespace MiniNavigator_DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsVisibleInObjectType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectTypes", "IsVisible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectTypes", "IsVisible");
        }
    }
}
