namespace MiniNavigator_DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaseObjects",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        ObjectTypeID = c.Guid(nullable: false),
                        FileExtension = c.String(),
                        Role = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Role_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ObjectTypes", t => t.ObjectTypeID, cascadeDelete: true)
                .ForeignKey("dbo.BaseObjects", t => t.Role_ID)
                .Index(t => t.ObjectTypeID)
                .Index(t => t.Role_ID);
            
            CreateTable(
                "dbo.ObjectTypes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ObjectActions",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ObjectAttributes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        ValueType = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ObjectFileChunks",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        ObjectFileID = c.Guid(nullable: false),
                        NumberInSequence = c.Long(nullable: false),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BaseObjects", t => t.ObjectFileID, cascadeDelete: true)
                .Index(t => t.ObjectFileID);
            
            CreateTable(
                "dbo.Object_ObjectAction_m2m",
                c => new
                    {
                        ObjectId = c.Guid(nullable: false),
                        ActionId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ObjectId, t.ActionId })
                .ForeignKey("dbo.ObjectTypes", t => t.ObjectId, cascadeDelete: true)
                .ForeignKey("dbo.ObjectActions", t => t.ActionId, cascadeDelete: true)
                .Index(t => t.ObjectId)
                .Index(t => t.ActionId);
            
            CreateTable(
                "dbo.Object_ObjectAttribute_m2m",
                c => new
                    {
                        ObjectId = c.Guid(nullable: false),
                        AttributeId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ObjectId, t.AttributeId })
                .ForeignKey("dbo.ObjectTypes", t => t.ObjectId, cascadeDelete: true)
                .ForeignKey("dbo.ObjectAttributes", t => t.AttributeId, cascadeDelete: true)
                .Index(t => t.ObjectId)
                .Index(t => t.AttributeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BaseObjects", "Role_ID", "dbo.BaseObjects");
            DropForeignKey("dbo.ObjectFileChunks", "ObjectFileID", "dbo.BaseObjects");
            DropForeignKey("dbo.BaseObjects", "ObjectTypeID", "dbo.ObjectTypes");
            DropForeignKey("dbo.Object_ObjectAttribute_m2m", "AttributeId", "dbo.ObjectAttributes");
            DropForeignKey("dbo.Object_ObjectAttribute_m2m", "ObjectId", "dbo.ObjectTypes");
            DropForeignKey("dbo.Object_ObjectAction_m2m", "ActionId", "dbo.ObjectActions");
            DropForeignKey("dbo.Object_ObjectAction_m2m", "ObjectId", "dbo.ObjectTypes");
            DropIndex("dbo.Object_ObjectAttribute_m2m", new[] { "AttributeId" });
            DropIndex("dbo.Object_ObjectAttribute_m2m", new[] { "ObjectId" });
            DropIndex("dbo.Object_ObjectAction_m2m", new[] { "ActionId" });
            DropIndex("dbo.Object_ObjectAction_m2m", new[] { "ObjectId" });
            DropIndex("dbo.ObjectFileChunks", new[] { "ObjectFileID" });
            DropIndex("dbo.BaseObjects", new[] { "Role_ID" });
            DropIndex("dbo.BaseObjects", new[] { "ObjectTypeID" });
            DropTable("dbo.Object_ObjectAttribute_m2m");
            DropTable("dbo.Object_ObjectAction_m2m");
            DropTable("dbo.ObjectFileChunks");
            DropTable("dbo.ObjectAttributes");
            DropTable("dbo.ObjectActions");
            DropTable("dbo.ObjectTypes");
            DropTable("dbo.BaseObjects");
        }
    }
}
