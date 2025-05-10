using QLKho_NCKH.Web.Models.Categories;
using System.Collections.Generic;

namespace QLKho_NCKH.Web.Models.Products
{
	public class ProductCategoryViewModel
	{
		public List<CategoryProductViewModel> CategoryProducts { get; set; }
		public int TotalCount { get; set; }
		public int ActiveCount { get; set; }
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
	}
}
