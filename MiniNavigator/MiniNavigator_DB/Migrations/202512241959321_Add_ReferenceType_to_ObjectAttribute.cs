namespace MiniNavigator_DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ReferenceType_to_ObjectAttribute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectAttributes", "ReferenceObjectTypeID", c => c.Guid());
            CreateIndex("dbo.ObjectAttributes", "ReferenceObjectTypeID");
            AddForeignKey("dbo.ObjectAttributes", "ReferenceObjectTypeID", "dbo.ObjectTypes", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ObjectAttributes", "ReferenceObjectTypeID", "dbo.ObjectTypes");
            DropIndex("dbo.ObjectAttributes", new[] { "ReferenceObjectTypeID" });
            DropColumn("dbo.ObjectAttributes", "ReferenceObjectTypeID");
        }
    }
}
