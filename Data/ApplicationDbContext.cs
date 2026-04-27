using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Models;

namespace PhotocopySystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Tables that exist in PhotocopySystemDB
        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<InventoryStock> InventoryStocks { get; set; }
        public DbSet<PrintJob> PrintJobs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ReturnRecord> ReturnRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // InventoryStock: RowVersion is a timestamp (ROWVERSION in SQL)
            modelBuilder.Entity<InventoryStock>()
                .Property(i => i.RowVersion)
                .IsRowVersion();

            // Configure Triggers so EF Core knows about them
            modelBuilder.Entity<PrintJob>()
                .ToTable(tb => tb.HasTrigger("trg_ApplyCancellationFine"));

            modelBuilder.Entity<OrderItem>()
                .ToTable(tb => tb.HasTrigger("trg_AutoDeductStock"));
        }
    }
}
