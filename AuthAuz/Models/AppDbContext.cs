namespace AuthAuz.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=AppDbContext")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<MProduct> MProducts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductA> ProductAs { get; set; }
        public virtual DbSet<tblProduct> tblProducts { get; set; }
        public virtual DbSet<tblRoles> TblRoles { get; set; }
        public virtual DbSet<tblUsers> TblUsers { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .Property(e => e.Mobile)
                .IsUnicode(false);

            modelBuilder.Entity<ProductA>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<ProductA>()
                .Property(e => e.Price)
                .IsUnicode(false);

            modelBuilder.Entity<ProductA>()
                .Property(e => e.OrderDate)
                .IsUnicode(false);

            modelBuilder.Entity<ProductA>()
                .Property(e => e.ImagePath)
                .IsUnicode(false);

            modelBuilder.Entity<tblProduct>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<tblProduct>()
                .Property(e => e.Price)
                .IsUnicode(false);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
