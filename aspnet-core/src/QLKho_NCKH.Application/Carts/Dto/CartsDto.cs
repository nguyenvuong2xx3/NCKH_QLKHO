using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Carts.Dto
{
	public class CartsDto
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		public string Name { get; set; }
		public string Image { get; set; }
		public decimal UnitPrice { get; set; }
		public long UserId { get; set; }
		public int ProductId { get; set; }
		public int CartItem { get; set; }
		public decimal TotalPrice => Quantity * UnitPrice;
	}
}
