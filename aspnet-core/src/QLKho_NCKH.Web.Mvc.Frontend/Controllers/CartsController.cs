using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Carts;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.Products;
using QLKho_NCKH.Users;
using QLKho_NCKH.Web.Models.Carts;


namespace QLKho_NCKH.Web.Controllers
{
	[AbpMvcAuthorize]
	public class CartsController : QLKho_NCKHControllerBase
	{
		private readonly ICartAppService _cartAppService;
		private readonly IProductAppService _productAppService;
		private readonly IUserAppService _userAppService;

		public CartsController(ICartAppService cartAppService, IProductAppService productAppService, IUserAppService userAppService)
		{
			_cartAppService = cartAppService;
			_productAppService = productAppService;
			_userAppService = userAppService;
		}

		public async Task<ActionResult> Index()
		{
			ViewData["HideFooter"] = true;

			// Lấy UserId từ AbpSession
			var userId = AbpSession.UserId ?? 0; // Nếu null thì gán 0
			//var nameUser = await _userAppService.GetNameUser(userId);

			var cartItems = new CartViewListModel
			{
				UserId = userId,
				//NameUser = nameUser,
				Carts = new List<CartViewModel>() // Khởi tạo danh sách rỗng
			};

			// Lấy danh sách giỏ hàng
			var carts = await _cartAppService.GetAllCart();

			foreach (var item in carts)
			{
				//var product = await _productAppService.GetAsync(new Abp.Application.Services.Dto.
				//{
				//	Id = item.ProductId
				//});

				// Thêm sản phẩm vào danh sách giỏ hàng
				cartItems.Carts.Add(new CartViewModel
				{
					Id = item.Id,
					ProductId = item.ProductId,
					Name = item.Name,
					Price = item.Price,
					TotalPrice = item.Quantity * item.Price,
					Quantity = item.Quantity,
					Image = item.Image
				});
			}

			return View(cartItems);
		}



	}
}
