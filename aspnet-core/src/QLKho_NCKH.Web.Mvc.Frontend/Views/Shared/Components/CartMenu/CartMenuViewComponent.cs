using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Carts;

namespace QLKho_NCKH.Web.Views.Shared.Components.CartMenu
{
	public class CartMenuViewComponent : QLKho_NCKHViewComponent
	{
		private readonly ICartFrontendAppService _cartFrontendAppService;

		public CartMenuViewComponent(ICartFrontendAppService cartFrontendAppService)
		{
			_cartFrontendAppService = cartFrontendAppService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var model = new CartMenuViewModel
			{
				CartItem = await _cartFrontendAppService.CountCart()
			};
			return View(model);
		}
	}
}
