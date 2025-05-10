using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Carts.Dto
{
	public class ProductListCartDto
	{

		public List<InventoryCartItemDto> Items { get; set; } = new List<InventoryCartItemDto>();
		public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

		public int CartItem { get; set; }

	}
}
