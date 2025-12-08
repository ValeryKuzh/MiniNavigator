namespace MiniNavigator_DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateObjectActions : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Object_ObjectAction_m2m", name: "ObjectId", newName: "ObjectTypeId");
            RenameColumn(table: "dbo.Object_ObjectAttribute_m2m", name: "ObjectId", newName: "ObjectTypeId");
            RenameIndex(table: "dbo.Object_ObjectAction_m2m", name: "IX_ObjectId", newName: "IX_ObjectTypeId");
            RenameIndex(table: "dbo.Object_ObjectAttribute_m2m", name: "IX_ObjectId", newName: "IX_ObjectTypeId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Object_ObjectAttribute_m2m", name: "IX_ObjectTypeId", newName: "IX_ObjectId");
            RenameIndex(table: "dbo.Object_ObjectAction_m2m", name: "IX_ObjectTypeId", newName: "IX_ObjectId");
            RenameColumn(table: "dbo.Object_ObjectAttribute_m2m", name: "ObjectTypeId", newName: "ObjectId");
            RenameColumn(table: "dbo.Object_ObjectAction_m2m", name: "ObjectTypeId", newName: "ObjectId");
        }
    }
}
