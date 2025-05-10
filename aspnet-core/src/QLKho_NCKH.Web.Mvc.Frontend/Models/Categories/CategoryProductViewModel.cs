using QLKho_NCKH.InventoryItems.Dto;
using QLKho_NCKH.Products.Dtos;
using System.Collections.Generic;

namespace QLKho_NCKH.Web.Models.Categories
{
	//public class CategoryProductViewModel
	//{
	//	public int CategoryId { get; set; }
	//	public string CategoryName { get; set; }
	//	public List<ProductListDto> Products { get; set; } = new List<ProductListDto>();
	//}
	public class CategoryProductViewModel
	{
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public List<InventoryItemListDto> Products { get; set; }
	}

}
