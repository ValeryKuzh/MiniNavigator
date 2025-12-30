namespace MiniNavigator_DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCascadeDeleteOnAllTypes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ObjectFiles", "Base_ID", "dbo.BaseObjects");
            DropForeignKey("dbo.ObjectRoles", "Base_ID", "dbo.BaseObjects");
            DropForeignKey("dbo.ObjectUsers", "Base_ID", "dbo.BaseObjects");
            AddForeignKey("dbo.ObjectFiles", "Base_ID", "dbo.BaseObjects", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ObjectRoles", "Base_ID", "dbo.BaseObjects", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ObjectUsers", "Base_ID", "dbo.BaseObjects", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ObjectUsers", "Base_ID", "dbo.BaseObjects");
            DropForeignKey("dbo.ObjectRoles", "Base_ID", "dbo.BaseObjects");
            DropForeignKey("dbo.ObjectFiles", "Base_ID", "dbo.BaseObjects");
            AddForeignKey("dbo.ObjectUsers", "Base_ID", "dbo.BaseObjects", "ID");
            AddForeignKey("dbo.ObjectRoles", "Base_ID", "dbo.BaseObjects", "ID");
            AddForeignKey("dbo.ObjectFiles", "Base_ID", "dbo.BaseObjects", "ID");
        }
    }
}
