namespace MiniNavigator_DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCascadeDeleteToObj : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ObjectAttributeValues", "ObjectID", "dbo.BaseObjects");
            AddForeignKey("dbo.ObjectAttributeValues", "ObjectID", "dbo.BaseObjects", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ObjectAttributeValues", "ObjectID", "dbo.BaseObjects");
            AddForeignKey("dbo.ObjectAttributeValues", "ObjectID", "dbo.BaseObjects", "ID");
        }
    }
}
