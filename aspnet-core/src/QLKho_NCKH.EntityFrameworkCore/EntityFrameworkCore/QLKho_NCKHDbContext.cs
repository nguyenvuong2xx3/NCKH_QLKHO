using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using QLKho_NCKH.Authorization.Roles;
using QLKho_NCKH.Authorization.Users;
using QLKho_NCKH.MultiTenancy;
using QLKho_NCKH.Products;
using QLKho_NCKH.Categories;
using QLKho_NCKH.Warehouses;
using QLKho_NCKH.Inventory;
using QLKho_NCKH.Suppliers;
using QLKho_NCKH.StockTransactions;
using YourProject.Domain.Transactions;
using QLKho_NCKH.Customers;

namespace QLKho_NCKH.EntityFrameworkCore
{
	public class QLKho_NCKHDbContext : AbpZeroDbContext<Tenant, Role, User, QLKho_NCKHDbContext>
	{
		/* Define a DbSet for each entity of the application */
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Warehouse> Warehouses { get; set; }
		public DbSet<StorageLocation> StorageLocations { get; set; }
		public DbSet<InventoryItem> InventoryItems { get; set; }
		public DbSet<Supplier> Suppliers { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<StockTransactionDetail> StockTransactionDetails { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<StockTransaction> StockTransactions { get; set; }

		public QLKho_NCKHDbContext(DbContextOptions<QLKho_NCKHDbContext> options)
				: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Cấu hình Category  
			builder.Entity<Category>(b =>
			{
				b.ToTable("AppCategories");
				b.HasMany(x => x.Products)
							.WithOne(x => x.Category)
							.HasForeignKey(x => x.CategoryId);
			});

			// Cấu hình Product  
			builder.Entity<Product>(b =>
			{
				b.ToTable("AppProducts");
				b.HasIndex(x => x.Code).IsUnique();
			});

			// Cấu hình Warehouse  
			builder.Entity<Warehouse>(b =>
			{
				b.ToTable("AppWarehouses");
				b.HasMany(x => x.StorageLocations)
							.WithOne(x => x.Warehouse)
							.HasForeignKey(x => x.WarehouseId);
			});

			// Cấu hình InventoryItem  
			builder.Entity<InventoryItem>(b =>
			{
				b.ToTable("AppInventoryItems");
				b.HasIndex(x => new { x.ProductId, x.StorageLocationId }).IsUnique();
			});

			builder.Entity<StockTransactionDetail>(b =>
			{
				b.ToTable("AppStockTransactionDetails");

				// Tạo composite key nếu cần  
				b.HasKey(x => new { x.StockTransactionId, x.ProductId, x.StorageLocationId });

				// Cấu hình quan hệ  
				b.HasOne(x => x.StockTransaction)
							.WithMany(x => x.Details)
							.HasForeignKey(x => x.StockTransactionId)
							.OnDelete(DeleteBehavior.Cascade);

				b.HasOne(x => x.Product)
							.WithMany()
							.HasForeignKey(x => x.ProductId)
							.OnDelete(DeleteBehavior.Restrict);

				b.HasOne(x => x.StorageLocation)
							.WithMany()
							.HasForeignKey(x => x.StorageLocationId)
							.OnDelete(DeleteBehavior.Restrict);

				// Index cho hiệu suất truy vấn  
				b.HasIndex(x => x.ProductId);
				b.HasIndex(x => x.StorageLocationId);
				b.HasIndex(x => x.BatchNumber);
			});
		}
	}
}
