using MiniNavigator_DB.Model;
using System.Data.Entity;

namespace MiniNavigator_DB.Context
{
    public class MiniNavigatorDbContext : DbContext
    {
        public DbSet<BaseObject> Object { get; set; }
        public DbSet<ObjectType> ObjectType { get; set; }
        public DbSet<ObjectAction> ObjectAction { get; set; }
        public DbSet<ObjectAttribute> ObjectAttribute { get; set; }
        public DbSet<ObjectFile> ObjectFile { get; set; }
        public DbSet<ObjectConfig> ObjectConfig { get; set; }
        public DbSet<ObjectUser> ObjectUser { get; set; }
        public DbSet<ObjectRole> ObjectRole { get; set; }
        public DbSet<ObjectFileChunk> ObjectFileChunks { get; set; }

        public MiniNavigatorDbContext(string connectionString) : base(connectionString) { }
        public MiniNavigatorDbContext() : base("name=InterMechDbConnection") { }
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

            // Relations
            modelBuilder.Entity<BaseObject>()
                .HasRequired(o => o.ObjectType)
                .WithMany(ot => ot.Objects)
                .HasForeignKey(o => o.ObjectTypeID);

            modelBuilder.Entity<ObjectType>()
                .HasMany(o => o.Actions)
                .WithMany(oa => oa.ObjectTypes)
                .Map(o_oa =>
                {
                    o_oa.ToTable("Object_ObjectAction_m2m");
                    o_oa.MapLeftKey("ObjectId");
                    o_oa.MapRightKey("ActionId");
                });

            modelBuilder.Entity<ObjectType>()
                .HasMany(o => o.Attributes)
                .WithMany(oa => oa.ObjectTypes)
                .Map(o_oa =>
                {
                    o_oa.ToTable("Object_ObjectAttribute_m2m");
                    o_oa.MapLeftKey("ObjectId");
                    o_oa.MapRightKey("AttributeId");
                });

            modelBuilder.Entity<ObjectFileChunk>()
                .HasRequired(ch => ch.ObjectFile)
                .WithMany(f => f.Chunks)
                .HasForeignKey(ch => ch.ObjectFileID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
