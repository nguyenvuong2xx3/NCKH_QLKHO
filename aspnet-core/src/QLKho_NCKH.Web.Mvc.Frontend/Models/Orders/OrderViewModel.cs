using System.Collections.Generic;
using QLKho_NCKH.Carts.Dto;
using QLKho_NCKH.Products.Dtos;
using QLKho_NCKH.Web.Models.Carts;

namespace QLKho_NCKH.Web.Models.Orders
{
	public class OrderViewModel
	{
		 public long UserId { get; set; }
		//public List<CartViewModel> Carts { get; set; }
		public List<ProductDetailDto> Products { get; set; }
		//public string PaymentMethod { get; set; }

	}
}
