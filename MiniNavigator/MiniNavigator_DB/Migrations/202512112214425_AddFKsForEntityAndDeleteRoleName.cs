namespace MiniNavigator_DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKsForEntityAndDeleteRoleName : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ObjectRoles", "RoleName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ObjectRoles", "RoleName", c => c.String());
        }
    }
}
