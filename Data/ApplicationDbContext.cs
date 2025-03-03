using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Models;
using System.Net;

namespace QuanLyBanHang.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        
        // Bảng liên quan đến người dùng
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        
        // Bảng liên quan đến sản phẩm
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ImageProduct> ImageProducts { get; set; }
        
        // Bảng liên quan đến đơn hàng
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrdersDetail> OrderDetails { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
        
        // Bảng liên quan đến giỏ hàng
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartDetails> ShoppingCartDetails { get; set; }
        
        // Bảng liên quan đến thanh toán
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        
        // Bảng liên quan đến voucher
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<CustomerVoucher> CustomerVouchers { get; set; }
        public DbSet<SidebarItem>           SidebarItems            { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình bảng người dùng
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().HasKey(u => u.UserID);
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            
            // Cấu hình bảng địa chỉ
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Cấu hình bảng đơn hàng
            modelBuilder.Entity<Orders>().ToTable("Orders");
            modelBuilder.Entity<OrdersDetail>().ToTable("OrdersDetail");
            modelBuilder.Entity<OrderStatusHistory>().ToTable("OrderStatusHistory");
            
            // Cấu hình bảng sản phẩm
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<ProductAttribute>().ToTable("Product_attribute");
            modelBuilder.Entity<ImageProduct>().ToTable("ImageProduct");
            
            // Cấu hình bảng giỏ hàng
            modelBuilder.Entity<ShoppingCart>().ToTable("ShoppingCart");
            modelBuilder.Entity<ShoppingCartDetails>().ToTable("ShoppingCartDetails");
            
            // Cấu hình bảng voucher
            modelBuilder.Entity<Voucher>().ToTable("Vouchers");
            modelBuilder.Entity<CustomerVoucher>().ToTable("CustomerVouchers");
            
            // Cấu hình bảng thanh toán và hóa đơn
            modelBuilder.Entity<Payment>().ToTable("Payment");
            modelBuilder.Entity<Invoice>().ToTable("Invoice");
        }
    }
}
