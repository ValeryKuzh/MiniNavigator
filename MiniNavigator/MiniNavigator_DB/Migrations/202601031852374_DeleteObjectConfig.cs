namespace MiniNavigator_DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteObjectConfig : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BaseObjects", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BaseObjects", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
