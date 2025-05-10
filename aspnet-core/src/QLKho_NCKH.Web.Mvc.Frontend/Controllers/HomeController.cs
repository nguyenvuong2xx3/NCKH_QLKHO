using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.Products;
using QLKho_NCKH.Products.Dtos;
using System.Threading.Tasks;
using System.Linq;
using System;
using QLKho_NCKH.Web.Models.Products;
using QLKho_NCKH.Web.Models.Categories;
using System.Collections.Generic;
using QLKho_NCKH.Categories;
using QLKho_NCKH.InventoryItems;
using QLKho_NCKH.InventoryItems.Dto;

namespace QLKho_NCKH.Web.Controllers
{
	[AbpMvcAuthorize]
	public class HomeController : QLKho_NCKHControllerBase
	{
		private readonly IProductAppService _productAppService;
		private readonly ICategoryAppService _categoryAppService;

		private readonly IInventoryItemAppService _inventoryItemAppService;

		public HomeController(IProductAppService productAppService, ICategoryAppService categoryAppService , IInventoryItemAppService inventoryItemAppService)
		{
			_productAppService = productAppService;
			_categoryAppService = categoryAppService;
			_inventoryItemAppService = inventoryItemAppService;
		}

		//public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		//{
		//	var categories = await _categoryAppService.GetAllCategories();

		//	// Tạo danh sách view model cho từng danh mục và sản phẩm
		//	var categoryProducts = new List<CategoryProductViewModel>();

		//	foreach (var category in categories)
		//	{
		//		// Lấy sản phẩm theo danh mục
		//		var result = await _inventoryItemAppService.GetInventoryItems(new GetInventoryItemsInput
		//		{

		//			CategoryId = category.Id,
		//			Filter = "",
		//			MaxResultCount = 20,
		//			Sorting = "CreationTime DESC"
		//		});

		//		categoryProducts.Add(new CategoryProductViewModel
		//		{
		//			CategoryId = category.Id,
		//			CategoryName = category.Name,
		//			Products = result.Items,
		//		});
		//	}

		//	// Thống kê tổng quan
		//	var allProducts = await _productAppService.GetAllProducts(new ProductInput());
		//	var activeProducts = allProducts.Items.Count(p => p.IsActive);

		//	var model = new ProductSearchViewModel()
		//	{
		//		//CurrentPage = page,
		//		TotalCount = result.TotalCount,
		//		//TotalCount = allProducts.TotalCount,
		//		//ActiveCount = activeProducts,
		//		CategoryProducts = categoryProducts
		//	};

		//	return View(model);
		//	//var result = await _inventoryItemAppService.GetInventoryItems(new GetInventoryItemsInput
		//	//{
		//	//	Filter = "",
		//	//	MaxResultCount = 20,
		//	//	Sorting = "CreationTime DESC"
		//	//});

		//	var model = new ProductSearchViewModel
		//	{
		//		Items = result.Items,
		//		TotalCount = result.TotalCount,
		//		Keyword = ""
		//	};
		//	return View(model);
		//	// Lấy danh sách tất cả danh mục
		//	//var categories = await _categoryAppService.GetAllCategories();

		//	//// Tạo danh sách view model cho từng danh mục và sản phẩm
		//	//var categoryProducts = new List<CategoryProductViewModel>();

		//	//foreach (var category in categories)
		//	//{
		//	//	// Lấy sản phẩm theo danh mục
		//	//	var productsResult = await _productAppService.GetAllProducts(new ProductInput
		//	//	{
		//	//		CategoryId = category.Id,
		//	//		MaxResultCount = 10, // Lấy 10 sản phẩm mỗi danh mục
		//	//		Sorting = "CreationTime DESC"
		//	//	});

		//	//	categoryProducts.Add(new CategoryProductViewModel
		//	//	{
		//	//		CategoryId = category.Id,
		//	//		CategoryName = category.Name,
		//	//		Products = productsResult.Items.ToList()
		//	//	});
		//	//}

		//	//// Thống kê tổng quan
		//	//var allProducts = await _productAppService.GetAllProducts(new ProductInput());
		//	//var activeProducts = allProducts.Items.Count(p => p.IsActive);

		//	//var model = new ProductViewModel(allProducts.Items.ToList())
		//	//{
		//	//	CurrentPage = page,
		//	//	TotalPages = (int)Math.Ceiling((double)allProducts.TotalCount / pageSize),
		//	//	//TotalCount = allProducts.TotalCount,
		//	//	//ActiveCount = activeProducts,
		//	//	CategoryProducts = categoryProducts
		//	//};

		//	//return View(model);
		//}
		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			var categories = await _categoryAppService.GetAllCategories();

			var categoryProducts = new List<CategoryProductViewModel>();

			foreach (var category in categories)
			{
				var result = await _inventoryItemAppService.GetInventoryItems(new GetInventoryItemsInput
				{
					CategoryId = category.Id,
					MaxResultCount = 20,
					Sorting = "CreationTime DESC"
				});

				categoryProducts.Add(new CategoryProductViewModel
				{
					CategoryId = category.Id,
					CategoryName = category.Name,
					Products = result.Items.ToList()
				});
			}

			var allProducts = await _productAppService.GetAllProducts(new ProductInput());
			var activeProducts = allProducts.Items.Count(p => p.IsActive);

			var model = new ProductCategoryViewModel
			{
				CategoryProducts = categoryProducts,
				TotalCount = allProducts.TotalCount,
				ActiveCount = activeProducts,
				CurrentPage = page,
				TotalPages = (int)Math.Ceiling((double)allProducts.TotalCount / pageSize)
			};

			return View(model);
		}

		public async Task<IActionResult> SearchProducts(string keyword)
		{

			//GetInventoryItems
			//var result = await _productAppService.GetAllProducts(new ProductInput
			//{
			//	Filter = keyword,
			//	MaxResultCount = 20
			//});
			var result = await _inventoryItemAppService.GetInventoryItems(new GetInventoryItemsInput
			{
				Filter = keyword,
				MaxResultCount = 20,
				Sorting = "CreationTime DESC"
			});

			var model = new ProductSearchViewModel
			{
				Items = result.Items,
				TotalCount = result.TotalCount,
				Keyword = keyword
			};

			return View("_ProductSearchResults", model);
		}

		public async Task<IActionResult> GetProductDetail(int id)
		{
			var product = await _productAppService.GetProductById(id);
			var model = new ProductDetailModel
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description,
				Code = product.Code,
				Image = product.Image,
				Unit = product.Unit,
				Weight = product.Weight,
				Volume = product.Volume,
				Barcode = product.Barcode
			};

			return PartialView("_DetailProductWeb", model);
		}

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