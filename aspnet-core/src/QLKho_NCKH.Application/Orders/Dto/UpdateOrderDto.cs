using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Orders.Dto
{
	public class UpdateOrderDto
	{
		public int OrderId { get; set; }
		public int PaymentMethod { get; set; }
		public int OrderStatus { get; set; }
	}
}
