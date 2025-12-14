using MiniNavigator_DB.Model;
using System.Data.Entity;

namespace MiniNavigator_DB.Context
{
    [DbConfigurationType(typeof(MiniNavigatorDbConfiguration))]
    public class MiniNavigatorDbContext : DbContext
    {
        public DbSet<BaseObject> BaseObjects { get; set; }
        public DbSet<ObjectType> ObjectTypes { get; set; }
        public DbSet<ObjectAction> ObjectActions { get; set; }
        public DbSet<ObjectAttribute> ObjectAttributes { get; set; }
        public DbSet<ObjectAttributeValue> ObjectAttributeValues { get; set; }
        public DbSet<ObjectFile> ObjectFiles { get; set; }
        public DbSet<ObjectFileChunk> ObjectFileChunks { get; set; }
        public DbSet<ObjectRole> ObjectRoles { get; set; }
        public DbSet<ObjectUser> ObjectUsers { get; set; }

        public MiniNavigatorDbContext(string connectionString) : base(connectionString) { }
        public MiniNavigatorDbContext() : base("name=HomeDbConnection") { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            // PK

            modelBuilder.Entity<BaseObject>().HasKey(o => o.ID);
            
            modelBuilder.Entity<ObjectType>().HasKey(ot => ot.ID);

            modelBuilder.Entity<ObjectAction>().HasKey(oa => oa.ID);

            modelBuilder.Entity<ObjectAttribute>().HasKey(oa => oa.ID);

            modelBuilder.Entity<ObjectFile>().HasKey(of => of.ID);

            modelBuilder.Entity<ObjectConfig>().HasKey(oc => oc.ID);

            modelBuilder.Entity<ObjectUser>().HasKey(ou => ou.ID);

            modelBuilder.Entity<ObjectRole>().HasKey(or => or.ID);

            modelBuilder.Entity<ObjectAttributeValue>().HasKey(x => new { x.ObjectID, x.AttributeID });

            // Relations

            // BaseObject

            modelBuilder.Entity<BaseObject>()
                .HasOptional(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BaseObject>()
                .HasOptional(x => x.ObjectType)
                .WithMany()
                .HasForeignKey(x => x.ObjectTypeID)
                .WillCascadeOnDelete(false);

            // ObjectAttributeValue

            modelBuilder.Entity<ObjectAttributeValue>()
                .HasRequired(x => x.Object)
                .WithMany()
                .HasForeignKey(x => x.ObjectID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ObjectAttributeValue>()
                .HasRequired(x => x.Attribute)
                .WithMany()
                .HasForeignKey(x => x.AttributeID)
                .WillCascadeOnDelete(false);

            // ObjectType

            modelBuilder.Entity<ObjectType>()
                .HasRequired(x => x.Base)
                .WithMany()
                .HasForeignKey(x => x.Base_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ObjectType>()
                .HasMany(x => x.Actions)
                .WithMany(x => x.ObjectTypes)
                .Map(m =>
                {
                    m.ToTable("ObjectTypeActions");
                    m.MapLeftKey("TypeID");
                    m.MapRightKey("ActionID");
                });

            modelBuilder.Entity<ObjectType>()
                .HasMany(x => x.Attributes)
                .WithMany(x => x.ObjectTypes)
                .Map(m =>
                {
                    m.ToTable("ObjectTypeAttributes");
                    m.MapLeftKey("TypeID");
                    m.MapRightKey("AttributeID");
                });

            // ObjectAction

            modelBuilder.Entity<ObjectAction>()
                .HasRequired(x => x.Base)
                .WithMany()
                .HasForeignKey(x => x.Base_ID)
                .WillCascadeOnDelete(false);

            // ObjectAttribute

            modelBuilder.Entity<ObjectAttribute>()
                .HasRequired(x => x.Base)
                .WithMany()
                .WillCascadeOnDelete(false);

            // ObjectUser

            modelBuilder.Entity<ObjectUser>()
                .HasRequired(x => x.Base)
                .WithMany()
                .HasForeignKey(x => x.Base_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ObjectUser>()
                .HasRequired(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleID)
                .WillCascadeOnDelete(false);

            // ObjectRole

            modelBuilder.Entity<ObjectRole>()
                .HasRequired(x => x.Base)
                .WithMany()
                .HasForeignKey(x => x.Base_ID)
                .WillCascadeOnDelete(false);

            // ObjectFile

            modelBuilder.Entity<ObjectFile>()
                .HasRequired(x => x.Base)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ObjectFile>()
                .HasMany(x => x.Chunks)
                .WithRequired(x => x.ObjectFile)
                .HasForeignKey(x => x.ObjectFileID)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
