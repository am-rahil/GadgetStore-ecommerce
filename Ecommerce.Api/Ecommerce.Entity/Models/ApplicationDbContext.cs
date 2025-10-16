using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entity.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, int>

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        { }

        // DbSets (tables)

        public DbSet<Supplier> SuppliersSet { get; set; }
        public DbSet<Category> CategoriesSet { get; set; }
        public DbSet<Product> ProductsSet { get; set; }
        public DbSet<CartItem> CartsSet { get; set; }
        public DbSet<Order> OrdersSet { get; set; }
        public DbSet<OrderDetail> OrderDetailsSet { get; set; }
        public DbSet<Payment> PaymentsSet { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           

            // One-to-many: Category -> Product
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-many: Supplier -> Product
            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.Products)
                .WithOne(p => p.Supplier)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

           

            // One-to-many: User -> Orders
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-many: User -> Payments
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Payments)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-many: Order -> OrderDetails
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many: Product -> OrderDetails
            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderDetails)
                .WithOne(od => od.Product)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
