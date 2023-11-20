using System;
using System.Collections.Generic;
using FoodShop_SWP.Models.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FoodShop_SWP.Models
{
    public partial class ShopFoodWebContext : DbContext
    {
        public ShopFoodWebContext() { }
        public ShopFoodWebContext(DbContextOptions<ShopFoodWebContext> options) : base(options) {
        }
        public DbSet<ThongKe> ThongKes { get; set; } = null;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;

        public DbSet<Adv> Advs { get; set; } = null!; 
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<News> News { get; set; } = null!;
        public DbSet<SystemSetting> SystemSettings { get; set; } = null!;
        public DbSet<Posts> Posts { get; set; } = null!;
        public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public DbSet<Subscribe> Subscribes {  get; set; } = null!;

        public DbSet<Feedback> Feedbacks { get; set; } = null!;
        public DbSet<Favourite> Favourites { get; set; } = null !;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Thiết lập quan hệ giữa Product và ProductCategory
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany(pc => pc.Products)
                .HasForeignKey(p => p.ProductCategoryId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                string ConnectionStr = config.GetConnectionString("DefaultConnection");

                optionsBuilder.UseSqlServer(ConnectionStr);
            }

        }
    }
}
