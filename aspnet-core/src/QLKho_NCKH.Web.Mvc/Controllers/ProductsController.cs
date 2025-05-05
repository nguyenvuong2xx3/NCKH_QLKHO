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
using Abp.Application.Services.Dto;
using QLKho_NCKH.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Abp.UI;

namespace QLKho_NCKH.Web.Controllers
{
	[AbpMvcAuthorize(PermissionNames.Pages_Products)]
	public class ProductsController : QLKho_NCKHControllerBase
	{
		private readonly IProductAppService _productAppService;
		private readonly ICategoryAppService _categoryAppService;
		private readonly ISupplierAppService _supplierAppService;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductsController(
				IProductAppService productAppService,
				ICategoryAppService categoryAppService,
				ISupplierAppService supplierAppService,
				IWebHostEnvironment webHostEnvironment)
		{
			_productAppService = productAppService;
			_categoryAppService = categoryAppService;
			_supplierAppService = supplierAppService;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<ActionResult> Index(GetAllProductsInput input)
		{
			var output = await _productAppService.GetAllProducts(input);
			var categories = await _categoryAppService.GetAllCategories(new GetAllCategoryDto());
			var suppliers = await _supplierAppService.GetAllSupplier();

			var model = new ProductViewModel(output.Items)
			{
				Categories = categories.Items.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList(),
				Suppliers = suppliers.Items.Select(s => new SelectListItem
				{
					Value = s.Id.ToString(),
					Text = s.Name
				}).ToList()
			};

			return View(model);
		}

		[HttpGet]
		public async Task<PartialViewResult> EditModal(int productId)
		{
			var product = await _productAppService.GetProductById(productId);
			var categories = await _categoryAppService.GetAllCategories(new GetAllCategoryDto());
			var supplier = product.SupplierId.HasValue
					? await _supplierAppService.GetByIdAsync(product.SupplierId.Value)
					: null;

			var model = new EditProductViewModel
			{
				Product = product,
				Categories = categories.Items.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name,
					Selected = c.Id == product.CategoryId
				}).ToList(),
				Category = await _categoryAppService.GetCategoryById(product.CategoryId),
				Supplier = supplier
			};

			return PartialView("_EditModal", model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateProductDto model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (model.ImageFile != null)
					{
						model.Image = UploadImage(model.ImageFile);
					}

					await _productAppService.Create(model);
					return Json(new { success = true, message = "Thêm sản phẩm thành công" });
				}

				var errors = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
						.ToList();

				return Json(new { success = false, errors });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

		[HttpPost]
		public async Task<IActionResult> Update(UpdateProductDto model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var existingProduct = await _productAppService.GetProductById(model.Id);
					if (existingProduct == null)
					{
						return Json(new { success = false, message = "Không tìm thấy sản phẩm." });
					}

					if (model.ImageFile != null && model.ImageFile.Length > 0)
					{
						// Xóa ảnh cũ nếu có
						if (!string.IsNullOrEmpty(existingProduct.Image))
						{
							DeleteFile(existingProduct.Image);
						}

						model.Image = UploadImage(model.ImageFile);
					}
					else
					{
						model.Image = existingProduct.Image;
					}

					await _productAppService.Update(model);
					return Json(new { success = true, message = "Cập nhật sản phẩm thành công" });
				}

				var errors = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
						.ToList();

				return Json(new { success = false, errors });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

		[HttpPost]
		public async Task<IActionResult> DeleteImage(int productId)
		{
			try
			{
				var product = await _productAppService.GetProductById(productId);
				if (product == null)
				{
					return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
				}

				if (string.IsNullOrEmpty(product.Image))
				{
					return Json(new { success = false, message = "Sản phẩm không có ảnh để xóa" });
				}

				DeleteFile(product.Image);

				// Cập nhật sản phẩm với Image = null
				var updateDto = new UpdateProductDto
				{
					Id = product.Id,
					Image = null
					// Các trường khác giữ nguyên
				};
				await _productAppService.Update(updateDto);

				return Json(new { success = true, message = "Xóa ảnh thành công" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var product = await _productAppService.GetProductById(id);
				if (product == null)
				{
					return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
				}

				// Xóa ảnh nếu có
				if (!string.IsNullOrEmpty(product.Image))
				{
					DeleteFile(product.Image);
				}

				await _productAppService.Delete(new EntityDto<int>(id));
				return Json(new { success = true, message = "Xóa sản phẩm thành công" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

		private string UploadImage(IFormFile imageFile)
		{
			if (imageFile == null || imageFile.Length == 0)
				return "/img/products/default.png";

			string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
			string fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

			if (!allowedExtensions.Contains(fileExtension))
			{
				throw new ArgumentException("Định dạng ảnh không hợp lệ. Vui lòng chọn ảnh có định dạng hợp lệ.");
			}

			string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img", "products");
			Directory.CreateDirectory(uploadsFolder);

			string uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{fileExtension}";
			string filePath = Path.Combine(uploadsFolder, uniqueFileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				imageFile.CopyTo(fileStream);
			}

			return $"/img/products/{uniqueFileName}";
		}

		private void DeleteFile(string imagePath)
		{
			if (string.IsNullOrEmpty(imagePath) || imagePath.Equals("/img/products/default.png"))
				return;

			string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
			if (System.IO.File.Exists(fullPath))
			{
				System.IO.File.Delete(fullPath);
			}
		}
	}
}