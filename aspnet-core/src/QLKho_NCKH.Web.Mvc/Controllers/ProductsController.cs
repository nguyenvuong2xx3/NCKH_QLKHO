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


		private void DeleteFile(string fileName)
		{
			if (string.IsNullOrEmpty(fileName)) return; // Kiểm tra tên file hợp lệ

			string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
			string fullPath = Path.Combine(uploadsFolder, fileName.TrimStart('/')); // Loại bỏ dấu `/` đầu nếu có

			if (System.IO.File.Exists(fullPath)) // Kiểm tra file có tồn tại không
			{
				System.IO.File.Delete(fullPath); // Xóa file
				Console.WriteLine($"Đã xóa file: {fullPath}");
			}
			else
			{
				Console.WriteLine("File không tồn tại!");
			}
		}


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

		public async Task<IActionResult> Update(UpdateProductDto model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var existingProduct = await _productAppService.GetByIdProducts(new EntityDto<int>(model.Id));
					if (existingProduct == null)
					{
						return Json(new { success = false, message = "Không tìm thấy sản phẩm." }); // Trả về lỗi nếu không tìm thấy
					}

					// Nếu có upload ảnh mới
					if (model.ImageFile != null && model.ImageFile.Length > 0)
					{
						// Danh sách các định dạng ảnh được phép tải lên
						string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".jfif" };
						string fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLower();

						// Kiểm tra xem ảnh có thuộc định dạng hợp lệ không
						if (!allowedExtensions.Contains(fileExtension))
						{
							return Json(new { success = false, message = "Định dạng ảnh không hợp lệ. Vui lòng chọn file .jpg, .png, .gif." });
						}

						// Upload ảnh mới và xóa ảnh cũ (nếu không phải ảnh mặc định)
						var oldImagePath = existingProduct.Image;
						string uniqueFileName = UploadImage(model.ImageFile);
						model.Image = uniqueFileName;

						// xóa ảnh cũ
						if (oldImagePath != null && !oldImagePath.Equals("/img/products/default.png"))
						{
							string oldImageFullPath = Path.Combine(webHostEnvironment.WebRootPath, oldImagePath.TrimStart('/'));
							if (System.IO.File.Exists(oldImageFullPath))
							{
								System.IO.File.Delete(oldImageFullPath);
							}
						}
					}

					await _productAppService.UpdateProducts(model);
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

	}
}