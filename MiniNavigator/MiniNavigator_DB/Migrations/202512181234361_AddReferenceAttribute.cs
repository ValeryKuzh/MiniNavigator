namespace MiniNavigator_DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReferenceAttribute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectAttributes", "IsReference", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectAttributes", "IsReference");
        }
    }
}
