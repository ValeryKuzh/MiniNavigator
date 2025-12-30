namespace MiniNavigator_DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsTitleInAttribute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectTypeAttributes", "IsTitle", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectTypeAttributes", "IsTitle");
        }
    }
}
