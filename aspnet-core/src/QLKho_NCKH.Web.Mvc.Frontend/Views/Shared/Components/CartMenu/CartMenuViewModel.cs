using Abp.Application.Services.Dto;
using QLKho_NCKH.Carts.Dto;

namespace QLKho_NCKH.Web.Views.Shared.Components.CartMenu
{
	public class CartMenuViewModel
	{

		public ListResultDto<CartsDto> Carts { get; set; }
		public int CartItem { get; set; }

		public int UserId { get; set; }
	}
}
