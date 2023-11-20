namespace HPIT.Flat.Data.Entitys
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FlatContext : DbContext
    {

        public static FlatContext Instance = new FlatContext();
        public FlatContext()
            : base("name=FlatModel")
        {
        }

        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Dictionary> Dictionary { get; set; }
        public virtual DbSet<Dorm> Dorm { get; set; }
        public virtual DbSet<DormAssign> DormAssign { get; set; }
        public virtual DbSet<DormFileAttach> DormFileAttach { get; set; }
        public virtual DbSet<DormSwitch> DormSwitch { get; set; }
        public virtual DbSet<FileMaterial> FileMaterial { get; set; }
        public virtual DbSet<Menus> Menus { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<PayMentFileAttach> PayMentFileAttach { get; set; }
        public virtual DbSet<PayRequest> PayRequest { get; set; }
        public virtual DbSet<RoleMenus> RoleMenus { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        public virtual DbSet<AuditLog> AuditLog { get; set; }

        public virtual DbSet<RoomFeedBack> RoomFeedBack { get; set; }

        public virtual DbSet<RoomDeduct> RoomDeduct { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .Property(e => e.DepartName)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.DepartDesc)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm>()
                .Property(e => e.DID)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm>()
                .Property(e => e.DormNo)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<DormAssign>()
                .Property(e => e.AID)
                .IsUnicode(false);

            modelBuilder.Entity<DormAssign>()
                .Property(e => e.DID)
                .IsUnicode(false);

            modelBuilder.Entity<DormAssign>()
                .Property(e => e.DormNo)
                .IsUnicode(false);

            modelBuilder.Entity<DormAssign>()
                .Property(e => e.StuNo)
                .IsUnicode(false);

            modelBuilder.Entity<DormFileAttach>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<DormFileAttach>()
                .Property(e => e.FID)
                .IsUnicode(false);

            modelBuilder.Entity<DormFileAttach>()
                .Property(e => e.DID)
                .IsUnicode(false);

            modelBuilder.Entity<DormSwitch>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<DormSwitch>()
                .Property(e => e.StuNo)
                .IsUnicode(false);

            modelBuilder.Entity<DormSwitch>()
                .Property(e => e.SecondStuNo)
                .IsUnicode(false);

            modelBuilder.Entity<FileMaterial>()
                .Property(e => e.FID)
                .IsUnicode(false);

            modelBuilder.Entity<FileMaterial>()
                .Property(e => e.FileType)
                .IsUnicode(false);

            modelBuilder.Entity<Menus>()
                .Property(e => e.MenuName)
                .IsUnicode(false);

            modelBuilder.Entity<Menus>()
                .Property(e => e.MenuType)
                .IsUnicode(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.PID)
                .IsUnicode(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.MID)
                .IsUnicode(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.StuNo)
                .IsUnicode(false);

            modelBuilder.Entity<PayMentFileAttach>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<PayMentFileAttach>()
                .Property(e => e.FID)
                .IsUnicode(false);

            modelBuilder.Entity<PayMentFileAttach>()
                .Property(e => e.MID)
                .IsUnicode(false);

            modelBuilder.Entity<PayRequest>()
                .Property(e => e.PID)
                .IsUnicode(false);

            modelBuilder.Entity<PayRequest>()
                .Property(e => e.DormNo)
                .IsUnicode(false);

            modelBuilder.Entity<PayRequest>()
                .Property(e => e.StuNo)
                .IsUnicode(false);

            //modelBuilder.Entity<PayRequest>()
            //    .HasMany(e => e.Payment)
            //    .WithRequired(e => e.PayRequest)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Roles>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<Roles>()
                .Property(e => e.RoleDesc)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.NickName)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);
        }
    }
}
