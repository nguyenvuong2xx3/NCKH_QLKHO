using QLKho_NCKH.InventoryItems.Dto;
using System.Collections.Generic;

namespace QLKho_NCKH.Web.Models.Products
{
	public class ProductSearchViewModel
	{
		public IReadOnlyList<InventoryItemListDto> Items { get; set; } = new List<InventoryItemListDto>();
		public int TotalCount { get; set; }
		public string Keyword { get; set; }
	}

}
