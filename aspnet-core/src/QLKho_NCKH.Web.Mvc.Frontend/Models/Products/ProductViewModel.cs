using QLKho_NCKH.Categories.Dto;
using QLKho_NCKH.Products.Dtos;
using QLKho_NCKH.Suppliers.Dtos;
using System;
using System.Collections.Generic;

namespace QLKho_NCKH.Web.Models.Products
{
	public class ProductViewModel
	{
		// Danh sách sản phẩm đã kết hợp thông tin từ Inventory và Product
		public List<ProductDisplayDto> Products { get; set; } = new List<ProductDisplayDto>();

		// Thông tin category
		public int? CategoryId { get; set; }
		public string CategoryName { get; set; }
		public List<CategoryListDto> Categories { get; set; } = new List<CategoryListDto>();

		// Phân trang
		public int CurrentPage { get; set; } = 1;
		public int PageSize { get; set; } = 20;
		public int TotalCount { get; set; }
		public int TotalPages { get; set; }

		// Sắp xếp
		public string SortOrder { get; set; }

		// Các thông tin khác
		public List<SupplierDto> Suppliers { get; set; } = new List<SupplierDto>();

		// Constructor
		public ProductViewModel() { }

		public ProductViewModel(List<ProductDisplayDto> products)
		{
			Products = products;
		}
	}

	public class ProductDisplayDto
	{
		// Thông tin từ Inventory
		public long InventoryId { get; set; }
		public decimal Price { get; set; }
		public int StockQuantity { get; set; }

		// Thông tin từ Product
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public int? CategoryId { get; set; }
		public string Barcode { get; set; }
		public string Unit { get; set; }
		public decimal? Weight { get; set; }
		public decimal? Volume { get; set; }
		public bool IsActive { get; set; }
		public int? SupplierId { get; set; }

		// Thông tin thêm nếu cần
		public string CategoryName { get; set; }
		public string SupplierName { get; set; }
	}
}