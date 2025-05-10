using System.Collections.Generic;
using QLKho_NCKH.OrderDetails.Dto;
using QLKho_NCKH.Orders.Dto;
using QLKho_NCKH.Products.Dtos;

namespace QLKho_NCKH.Web.Models.Orders
{
	public class OrderViewSuccess
	{
		public OrderListDto Order {  get; set; }
		public List<OrderDetailDto> OrderListDetail { get; set; }
		public List<ProductListDto> ProductList { get; set; }
	}
}
