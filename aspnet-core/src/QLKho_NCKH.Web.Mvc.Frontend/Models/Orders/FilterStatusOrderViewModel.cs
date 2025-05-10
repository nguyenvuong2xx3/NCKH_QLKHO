using System.Collections.Generic;
using QLKho_NCKH.OrderDetails.Dto;
using QLKho_NCKH.Orders.Dto;
using QLKho_NCKH.Products.Dtos;

namespace QLKho_NCKH.Web.Models.Orders
{
	public class FilterStatusOrderViewModel
	{
		public List<OrderDetailDto> ListOrder { get; set; }
		public int? OrderStatus { get; set; }
		public List<ProductListDto> Products { get; set; }
	}
}
