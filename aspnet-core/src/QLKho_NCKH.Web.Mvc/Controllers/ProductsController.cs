using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.Products.Dtos;
using QLKho_NCKH.Products;
using QLKho_NCKH.Web.Models.Products;
using QLKho_NCKH.Categories;
using QLKho_NCKH.Suppliers;
using Abp.AspNetCore.Mvc.Authorization;
using QLKho_NCKH.Authorization;

namespace QLKho_NCKH.Web.Controllers
{
	[AbpMvcAuthorize(PermissionNames.Pages_Products)]

	public class ProductsController : QLKho_NCKHControllerBase
	{
		private readonly IProductAppService _productAppService;
		private readonly ICategoryAppService _categoryAppService;
		private readonly ISupplierAppService _supplierAppService;
		public ProductsController(IProductAppService productAppService, ICategoryAppService categoryAppService, ISupplierAppService supplierAppService)
		{
			_productAppService = productAppService;
			_categoryAppService = categoryAppService;
			_supplierAppService = supplierAppService;
		}
		public async Task<ActionResult> Index(ProductInput input)
		{

			var output = await _productAppService.GetAllProducts(input);
			var Categories = await _categoryAppService.GetAllCategories();
			var Suppliers = await _supplierAppService.GetAllSupplier();
			var model = new ProductViewModel(output.Items);
			model.Categories = Categories;
			model.Suppliers = Suppliers;
			return View(model);
		}//Test

		public async Task<IActionResult> Create(CreateProductDto model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					// Upload ảnh và lấy tên file duy nhất
					string uniqueFileName = UploadImage(model.ImageFile);

					// Gán đường dẫn file vào model
					model.Image = uniqueFileName;

					// Gọi service để tạo mới sản phẩm
					await _productAppService.Create(model);

					TempData["SuccessMessage"] = "Thêm sản phẩm thành công";
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}

			var errors = ModelState.Values.SelectMany(v => v.Errors)
																		 .Select(e => e.ErrorMessage)
																		 .ToList();
			return Json(new { success = false, errors });
		}



		private string UploadImage(IFormFile ImageFile)
		{
			if (ImageFile != null && ImageFile.Length > 0)
			{
				// Kiểm tra định dạng ảnh
				string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
				string fileExtension = Path.GetExtension(ImageFile.FileName).ToLower();
				if (!allowedExtensions.Contains(fileExtension))
				{
					throw new ArgumentException("Định dạng ảnh không hợp lệ. Vui lòng chọn ảnh có định dạng hợp lệ.");
				}

				string uploadsFolder = @"E:\Uploads\";
				Directory.CreateDirectory(uploadsFolder); // Tạo thư mục nếu chưa có

				string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N") + fileExtension;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					ImageFile.CopyTo(fileStream);
				}

				return "/products/" + uniqueFileName;
			}

			return "/products/default.png"; // Trả về ảnh mặc định nếu không có ảnh upload
		}
	}
}
