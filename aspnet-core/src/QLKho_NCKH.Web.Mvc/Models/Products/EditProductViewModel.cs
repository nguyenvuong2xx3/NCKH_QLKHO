using QLKho_NCKH.Categories.Dto;
using QLKho_NCKH.Products.Dtos;
using QLKho_NCKH.Suppliers.Dtos;

namespace QLKho_NCKH.Web.Models.Products
{
	public class EditProductViewModel
	{
		public ProductListDto Product { get; set; }
		public CategoryListDto Category { get; set; }
		public SupplierDto Supplier { get; set; }
	}
}
