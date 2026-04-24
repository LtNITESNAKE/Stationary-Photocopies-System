using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Models;

namespace PhotocopySystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Add all your tables here so Entity Framework knows about them!
        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<InventoryStock> InventoryStocks { get; set; }
        public DbSet<ServerNode> ServerNodes { get; set; }
        public DbSet<PrintJob> PrintJobs { get; set; }
        public DbSet<ProxyLog> ProxyLogs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
