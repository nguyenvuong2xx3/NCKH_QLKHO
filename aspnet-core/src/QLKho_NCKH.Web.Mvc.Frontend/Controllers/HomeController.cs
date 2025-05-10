using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.Products;
using QLKho_NCKH.Products.Dtos;
using System.Threading.Tasks;
using System.Linq;
using System;
using QLKho_NCKH.Web.Models.Products;

namespace QLKho_NCKH.Web.Controllers
{
	[AbpMvcAuthorize]
	public class HomeController : QLKho_NCKHControllerBase
	{
		private readonly IProductAppService _productAppService;

		public HomeController(IProductAppService productAppService)
		{
			_productAppService = productAppService;
		}

		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			// Lấy danh sách sản phẩm
			var productsResult = await _productAppService.GetAllProducts(new ProductInput
			{
				MaxResultCount = pageSize,
				SkipCount = (page - 1) * pageSize,
				Sorting = "CreationTime DESC"
			});

			var model = new ProductViewModel(productsResult.Items.ToList())
			{
				CurrentPage = page,
				TotalPages = (int)Math.Ceiling((double)productsResult.TotalCount / pageSize)
			};

			return View(model);
		}

		public async Task<IActionResult> SearchProducts(string keyword)
		{
			var result = await _productAppService.GetAllProducts(new ProductInput
			{
				Filter = keyword,
				MaxResultCount = 20
			});

			var model = new ProductViewModel(result.Items.ToList())
			{
				count = result.Items.Count
			};

			return PartialView("_ProductSearchResults", model);
		}

		//public async Task<IActionResult> GetProductDetail(int id)
		//{
		//	var product = await _productAppService.GetProductById(id);
		//	var model = new ProductDetailModel
		//	{
		//		Id = product.Id,
		//		Name = product.Name,
		//		Description = product.Description,
		//		Code = product.Code,
		//		Image = product.Image,
		//		Unit = product.Unit,
		//		Weight = product.Weight,
		//		Volume = product.Volume,
		//		Barcode = product.Barcode
		//	};

		//	return PartialView("_ProductDetail", model);
		//}

		public async Task<IActionResult> LoadMoreProducts(int page, int pageSize = 10)
		{
			var result = await _productAppService.GetAllProducts(new ProductInput
			{
				MaxResultCount = pageSize,
				SkipCount = (page - 1) * pageSize,
				Sorting = "CreationTime DESC"
			});

			if (result.Items.Any())
			{
				return PartialView("_ProductListPartial", new ProductViewModel(result.Items.ToList()));
			}

			return NoContent();
		}
	}
}