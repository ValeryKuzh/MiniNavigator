namespace MiniNavigator_DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedAndUIChanges : DbMigration
    {
        public override void Up()
        {
            // Переименовать колонку TypeID в ObjectTypeID
            RenameColumn("dbo.ObjectTypeAttributes", "TypeID", "ObjectTypeID");

            // Добавить новые колонки
            AddColumn("dbo.ObjectTypeAttributes", "IsRequired", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.ObjectTypeAttributes", "IsVisible", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.ObjectTypeAttributes", "Order", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.ObjectTypeAttributes", "ObjectAttribute_ID", c => c.Guid());

            // Индекс и FK для новой колонки
            CreateIndex("dbo.ObjectTypeAttributes", "ObjectAttribute_ID");
            AddForeignKey("dbo.ObjectTypeAttributes", "ObjectAttribute_ID", "dbo.ObjectAttributes", "ID");
        }


        public override void Down()
        {
            DropForeignKey("dbo.ObjectTypeAttributes", "ObjectAttribute_ID", "dbo.ObjectAttributes");
            DropIndex("dbo.ObjectTypeAttributes", new[] { "ObjectAttribute_ID" });

            DropColumn("dbo.ObjectTypeAttributes", "ObjectAttribute_ID");
            DropColumn("dbo.ObjectTypeAttributes", "Order");
            DropColumn("dbo.ObjectTypeAttributes", "IsVisible");
            DropColumn("dbo.ObjectTypeAttributes", "IsRequired");

            RenameColumn("dbo.ObjectTypeAttributes", "ObjectTypeID", "TypeID");
        }

    }
}
