using QLKho_NCKH.Categories.Dto;
using QLKho_NCKH.Products.Dtos;
using QLKho_NCKH.Suppliers.Dtos;
using System.Collections.Generic;

namespace QLKho_NCKH.Web.Models.Products
{
	public class ProductViewModel
	{
		public IReadOnlyList<ProductListDto> Products;

		public List<CategoryListDto> Categories { get; set; }	

		public ProductViewModel(IReadOnlyList<ProductListDto> products)
		{
			Products = products;
		}

		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int count { get; set; }

		public List<SupplierDto> Suppliers { get; set; }
	}
}
