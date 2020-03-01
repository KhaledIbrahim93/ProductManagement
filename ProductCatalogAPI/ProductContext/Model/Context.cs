using Microsoft.EntityFrameworkCore;
namespace ProductDAL.Model
{
    public class Context : DbContext
    {
        public Context()
        { }

        public Context(DbContextOptions<Context> options)
        : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=SmartTech;Trusted_Connection=True;");
            }
       }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(e => {
                e.ToTable("Product");
                e.HasKey(s => s.Id);
                e.Property(p => p.Name).HasColumnName("Name");
                e.Property(p => p.Photo).HasColumnName("Photo");
                e.Property(p => p.Price).HasColumnName("Price");
                e.Property(p => p.CreateDate).HasColumnName("CreateDate");
                e.Property(p => p.LastUpdate).HasColumnName("LastUpdate");
            });
        }
        public DbSet<Product> Products { get; set; }

    }
}
