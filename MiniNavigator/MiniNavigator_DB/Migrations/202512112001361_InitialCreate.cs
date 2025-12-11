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
                        ObjectTypeID = c.Guid(),
                        ParentID = c.Guid(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BaseObjects", t => t.ObjectTypeID)
                .ForeignKey("dbo.BaseObjects", t => t.ParentID)
                .Index(t => t.ObjectTypeID)
                .Index(t => t.ParentID);
            
            CreateTable(
                "dbo.ObjectActions",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Base_ID = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BaseObjects", t => t.Base_ID)
                .Index(t => t.Base_ID);
            
            CreateTable(
                "dbo.ObjectTypes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Base_ID = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BaseObjects", t => t.Base_ID)
                .Index(t => t.Base_ID);
            
            CreateTable(
                "dbo.ObjectAttributes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        ValueType = c.String(),
                        Base_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BaseObjects", t => t.Base_ID)
                .Index(t => t.Base_ID);
            
            CreateTable(
                "dbo.ObjectAttributeValues",
                c => new
                    {
                        ObjectID = c.Guid(nullable: false),
                        AttributeID = c.Guid(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.ObjectID, t.AttributeID })
                .ForeignKey("dbo.ObjectAttributes", t => t.AttributeID)
                .ForeignKey("dbo.BaseObjects", t => t.ObjectID)
                .Index(t => t.ObjectID)
                .Index(t => t.AttributeID);
            
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
                .ForeignKey("dbo.ObjectFiles", t => t.ObjectFileID, cascadeDelete: true)
                .Index(t => t.ObjectFileID);
            
            CreateTable(
                "dbo.ObjectFiles",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        FileExtension = c.String(),
                        Base_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BaseObjects", t => t.Base_ID)
                .Index(t => t.Base_ID);
            
            CreateTable(
                "dbo.ObjectRoles",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        RoleName = c.String(),
                        Base_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BaseObjects", t => t.Base_ID)
                .Index(t => t.Base_ID);
            
            CreateTable(
                "dbo.ObjectUsers",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        RoleID = c.Guid(nullable: false),
                        Base_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BaseObjects", t => t.Base_ID)
                .ForeignKey("dbo.ObjectRoles", t => t.RoleID)
                .Index(t => t.RoleID)
                .Index(t => t.Base_ID);
            
            CreateTable(
                "dbo.ObjectTypeActions",
                c => new
                    {
                        TypeID = c.Guid(nullable: false),
                        ActionID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.TypeID, t.ActionID })
                .ForeignKey("dbo.ObjectTypes", t => t.TypeID, cascadeDelete: true)
                .ForeignKey("dbo.ObjectActions", t => t.ActionID, cascadeDelete: true)
                .Index(t => t.TypeID)
                .Index(t => t.ActionID);
            
            CreateTable(
                "dbo.ObjectTypeAttributes",
                c => new
                    {
                        TypeID = c.Guid(nullable: false),
                        AttributeID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.TypeID, t.AttributeID })
                .ForeignKey("dbo.ObjectTypes", t => t.TypeID, cascadeDelete: true)
                .ForeignKey("dbo.ObjectAttributes", t => t.AttributeID, cascadeDelete: true)
                .Index(t => t.TypeID)
                .Index(t => t.AttributeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ObjectUsers", "RoleID", "dbo.ObjectRoles");
            DropForeignKey("dbo.ObjectUsers", "Base_ID", "dbo.BaseObjects");
            DropForeignKey("dbo.ObjectRoles", "Base_ID", "dbo.BaseObjects");
            DropForeignKey("dbo.ObjectFileChunks", "ObjectFileID", "dbo.ObjectFiles");
            DropForeignKey("dbo.ObjectFiles", "Base_ID", "dbo.BaseObjects");
            DropForeignKey("dbo.ObjectAttributeValues", "ObjectID", "dbo.BaseObjects");
            DropForeignKey("dbo.ObjectAttributeValues", "AttributeID", "dbo.ObjectAttributes");
            DropForeignKey("dbo.ObjectTypes", "Base_ID", "dbo.BaseObjects");
            DropForeignKey("dbo.ObjectTypeAttributes", "AttributeID", "dbo.ObjectAttributes");
            DropForeignKey("dbo.ObjectTypeAttributes", "TypeID", "dbo.ObjectTypes");
            DropForeignKey("dbo.ObjectAttributes", "Base_ID", "dbo.BaseObjects");
            DropForeignKey("dbo.ObjectTypeActions", "ActionID", "dbo.ObjectActions");
            DropForeignKey("dbo.ObjectTypeActions", "TypeID", "dbo.ObjectTypes");
            DropForeignKey("dbo.ObjectActions", "Base_ID", "dbo.BaseObjects");
            DropForeignKey("dbo.BaseObjects", "ParentID", "dbo.BaseObjects");
            DropForeignKey("dbo.BaseObjects", "ObjectTypeID", "dbo.BaseObjects");
            DropIndex("dbo.ObjectTypeAttributes", new[] { "AttributeID" });
            DropIndex("dbo.ObjectTypeAttributes", new[] { "TypeID" });
            DropIndex("dbo.ObjectTypeActions", new[] { "ActionID" });
            DropIndex("dbo.ObjectTypeActions", new[] { "TypeID" });
            DropIndex("dbo.ObjectUsers", new[] { "Base_ID" });
            DropIndex("dbo.ObjectUsers", new[] { "RoleID" });
            DropIndex("dbo.ObjectRoles", new[] { "Base_ID" });
            DropIndex("dbo.ObjectFiles", new[] { "Base_ID" });
            DropIndex("dbo.ObjectFileChunks", new[] { "ObjectFileID" });
            DropIndex("dbo.ObjectAttributeValues", new[] { "AttributeID" });
            DropIndex("dbo.ObjectAttributeValues", new[] { "ObjectID" });
            DropIndex("dbo.ObjectAttributes", new[] { "Base_ID" });
            DropIndex("dbo.ObjectTypes", new[] { "Base_ID" });
            DropIndex("dbo.ObjectActions", new[] { "Base_ID" });
            DropIndex("dbo.BaseObjects", new[] { "ParentID" });
            DropIndex("dbo.BaseObjects", new[] { "ObjectTypeID" });
            DropTable("dbo.ObjectTypeAttributes");
            DropTable("dbo.ObjectTypeActions");
            DropTable("dbo.ObjectUsers");
            DropTable("dbo.ObjectRoles");
            DropTable("dbo.ObjectFiles");
            DropTable("dbo.ObjectFileChunks");
            DropTable("dbo.ObjectAttributeValues");
            DropTable("dbo.ObjectAttributes");
            DropTable("dbo.ObjectTypes");
            DropTable("dbo.ObjectActions");
            DropTable("dbo.BaseObjects");
        }
    }
}
