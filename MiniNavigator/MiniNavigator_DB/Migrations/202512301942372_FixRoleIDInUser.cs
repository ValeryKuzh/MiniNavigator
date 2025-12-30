namespace MiniNavigator_DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRoleIDInUser : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ObjectUsers", new[] { "RoleID" });
            AlterColumn("dbo.ObjectUsers", "RoleID", c => c.Guid());
            CreateIndex("dbo.ObjectUsers", "RoleID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ObjectUsers", new[] { "RoleID" });
            AlterColumn("dbo.ObjectUsers", "RoleID", c => c.Guid(nullable: false));
            CreateIndex("dbo.ObjectUsers", "RoleID");
        }
    }
}
