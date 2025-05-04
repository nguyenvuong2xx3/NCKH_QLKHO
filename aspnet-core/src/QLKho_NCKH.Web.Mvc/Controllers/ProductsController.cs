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

		public async Task<ActionResult> EditModal(int productId)
		{
			var product = await _productAppService.GetProductById(productId);

			var category = await _categoryAppService.GetCategoryById(product.CategoryId);
			var supplier = product.SupplierId.HasValue
					? await _supplierAppService.GetByIdAsync(product.SupplierId.Value)
					: null;

			var model = new EditProductViewModel
			{
				Product = product
			};
			return PartialView("_EditModal", model);
		}

		public async Task<IActionResult> AddProduct()
		{
			return PartialView("_AddProductModal");
		}

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

				// Đường dẫn thư mục lưu trữ ảnh trong dự án
				string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Products");
				Directory.CreateDirectory(uploadsFolder); // Tạo thư mục nếu chưa có

				// Tạo tên file duy nhất
				string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N") + fileExtension;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);

				// Lưu file vào thư mục
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					ImageFile.CopyTo(fileStream);
				}

				// Trả về đường dẫn để hiển thị trên giao diện
				return "/img/Products/" + uniqueFileName;
			}

			return "/img/Products/default.png"; // Trả về ảnh mặc định nếu không có ảnh upload
		}

		public async Task<IActionResult> EditAndUploadDeleteImage(UpdateProductDto model)
		{
			try
			{
				// Kiểm tra xem dữ liệu đầu vào có hợp lệ không
				if (!ModelState.IsValid)
				{
					// Lấy danh sách lỗi nếu có
					var errors = ModelState.Values.SelectMany(v => v.Errors)
																				.Select(e => e.ErrorMessage)
																				.ToList();
					return Json(new { success = false, errors }); // Trả về lỗi dưới dạng JSON
				}

				// Kiểm tra xem sản phẩm có tồn tại trong hệ thống không
				var existingProduct = await _productAppService.GetProductById(model.Id);
				if (existingProduct == null)
				{
					return Json(new { success = false, message = "Không tìm thấy sản phẩm." }); // Trả về lỗi nếu không tìm thấy
				}

				// Kiểm tra xem người dùng có tải lên ảnh mới không
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

					// Nếu sản phẩm đã có ảnh trước đó, xóa ảnh cũ trước khi cập nhật ảnh mới
					if (!string.IsNullOrEmpty(existingProduct.Image))
					{
						DeleteFile(existingProduct.Image); // Gọi hàm xóa ảnh cũ
					}

					// Upload ảnh mới và cập nhật đường dẫn vào model
					model.Image = UploadImage(model.ImageFile);
				}
				else
				{
					// Nếu người dùng không chọn ảnh mới, giữ nguyên ảnh cũ
					model.Image = existingProduct.Image;
				}

				// Gọi service để cập nhật thông tin sản phẩm trong database
				await _productAppService.Update(model);

				// Trả về kết quả thành công kèm theo đường dẫn ảnh mới (nếu có thay đổi)
				return Json(new { success = true, message = "Cập nhật sản phẩm thành công", imagePath = model.Image });
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu có lỗi xảy ra trong quá trình cập nhật
				return Json(new { success = false, message = ex.Message });
			}
		}


		private void DeleteFile(string fileName)
		{
			if (string.IsNullOrEmpty(fileName)) return; // Kiểm tra tên file hợp lệ

			string folderPath = @"E:\Uploads"; // Thư mục chứa ảnh
			string fullPath = Path.Combine(folderPath, fileName); // Đường dẫn đầy đủ

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


		//public async Task<IActionResult> DeleteImage(int productId)
		//{
		//	var existingProduct = await _productAppService.GetAsync(new EntityDto<int>(productId));

		//	if (existingProduct == null)
		//	{
		//		return Json(new { success = false, message = "Sản phẩm không tồn tại." });
		//	}

		//	if (string.IsNullOrEmpty(existingProduct.Image))
		//	{
		//		return Json(new { success = false, message = "Sản phẩm này không có ảnh để xóa." });
		//	}

		//	try
		//	{
		//		// Xóa file ảnh trên server
		//		DeleteFile(existingProduct.Image);

		//		// Cập nhật lại sản phẩm trong DB (xóa đường dẫn ảnh)
		//		var updateProductDto = new UpdateProductDto()
		//		{
		//			Id = existingProduct.Id,
		//			Name = existingProduct.Name,
		//			Description = existingProduct.Description,
		//			Price = existingProduct.Price,
		//			State = existingProduct.State,
		//			CategoryId = existingProduct.CategoryId,
		//			Image = null,
		//		};
		//		await _productAppService.Update(updateProductDto);

		//		return Json(new { success = true, message = "Ảnh đã được xóa thành công." });
		//	}
		//	catch (Exception)
		//	{
		//		return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa ảnh. Vui lòng thử lại." });
		//	}
		//}

	}
}
