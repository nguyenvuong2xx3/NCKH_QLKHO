using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System;
using QLKho_NCKH.Products.Dtos;
using System.Collections.Generic;
using QLKho_NCKH.Authorization;
using Abp.Collections.Extensions;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Http;
using QLKho_NCKH.Suppliers;
using QLKho_NCKH.Categories;

namespace QLKho_NCKH.Products
{
	[AbpMvcAuthorize(PermissionNames.Pages_Products)]

	public class ProductAppService : ApplicationService, IProductAppService
	{
		private readonly IRepository<Product> _productRepository;
		private readonly IRepository<Category> _categoryRepository;
		private readonly IRepository<Supplier> _supplierRepository;
		private readonly IWebHostEnvironment _env;

		public ProductAppService(IRepository<Product> productRepository, 
			IWebHostEnvironment env,
			IRepository<Category> categoryRepository
			, IRepository<Supplier> supplierRepository
			) // IRepository<Category> categoryRepository,
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
			_supplierRepository = supplierRepository;
			_env = env;
		}

		[AbpAuthorize(PermissionNames.Pages_Products_Create)]
		public async Task<ProductListDto> Create(CreateProductDto input)
		{
			var product = new Product
			{
				Code = input.Code,
				Name = input.Name,
				Description = input.Description,
				Image = input.Image,
				CategoryId = input.CategoryId,
				Barcode = input.Barcode,
				Unit = input.Unit,
				Weight = input.Weight,
				Volume = input.Volume,
				IsActive = input.IsActive,
				SupplierId = input.SupplierId,
			};

			await _productRepository.InsertAsync(product);

			return new ProductListDto
			{
				Id = product.Id,
				Code = product.Code,
				Name = product.Name,
				Description = product.Description,
				Image = product.Image,
				CategoryId = product.CategoryId,
				Barcode = product.Barcode,
				Unit = product.Unit,
				Weight = product.Weight,
				Volume = product.Volume,
				IsActive = product.IsActive,
				SupplierId = product.SupplierId,
			};
		}

		public Task<ProductListDto> GetProductByCode(string code)
		{
			throw new NotImplementedException();
		}

		Task<ProductListDto> IProductAppService.GetByIdProducts(EntityDto<int> input)
		{
			throw new NotImplementedException();
		}

		[AbpAuthorize(PermissionNames.Pages_Products_Edit)]
		public async Task<ProductListDto> Update(UpdateProductDto input)
		{
			var product = await _productRepository.GetAsync(input.Id);
			if (product == null)
			{
				throw new UserFriendlyException("Product not found");
			}
			product.Code = input.Code;
			product.Name = input.Name;
			product.Description = input.Description;
			product.Image = input.Image;
			product.CategoryId = input.CategoryId;
			product.Barcode = input.Barcode;
			product.Unit = input.Unit;
			product.Weight = input.Weight;
			product.Volume = input.Volume;
			product.IsActive = input.IsActive;
			product.SupplierId = input.SupplierId;

			await _productRepository.UpdateAsync(product);

			return new ProductListDto
			{
				Id = product.Id,
				Code = product.Code,
				Name = product.Name,
				Description = product.Description,
				Image = product.Image,
				CategoryId = product.CategoryId,
				Barcode = product.Barcode,
				Unit = product.Unit,
				Weight = product.Weight,
				Volume = product.Volume,
				IsActive = product.IsActive,
				SupplierId = product.SupplierId,
			};
		}

		public Task<PagedResultDto<ProductListDto>> SearchProducts(GetAllProductsInput input)
		{
			throw new NotImplementedException();
		}

		[AbpAuthorize(PermissionNames.Pages_Products_Delete)]
		public async Task Delete(EntityDto<int> input)
		{
			var product = _productRepository.GetAll().FirstOrDefault(x => x.Id == input.Id);
			if (product == null)
			{
				throw new UserFriendlyException("Product not found");
			}
			_productRepository.Delete(product);


		}

		public async Task<PagedResultDto<ProductListDto>> GetAllProducts(ProductInput input)
		{
			// Xây dựng truy vấn cơ sở dữ liệu
			var query = _productRepository
				.GetAllIncluding(x => x.Category, x => x.Supplier)
				.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
								 x => x.Name.Contains(input.Filter) || x.Description.Contains(input.Filter))
				// only add this WHERE if SupplierId > 0
				.WhereIf(input.SupplierId > 0,
								 x => x.SupplierId == input.SupplierId)
				.WhereIf(input.CategoryId > 0,
								 x => x.CategoryId == input.CategoryId);


			// … ordering, paging, projection, etc.

			//.WhereIf(input.CategoryId.HasValue,
			//				 x => x.CategoryId == input.CategoryId.Value)
			//.WhereIf(input.State.HasValue,
			//				 x => x.State == input.State.Value);

			// Sắp xếp dữ liệu
			query = query.OrderBy(input.Sorting ?? "CreationTime DESC");

			// Đếm tổng số bản ghi
			var totalCount = await query.CountAsync();

			// Phân trang
			var items = await query.PageBy(input).ToListAsync();

			// Chuyển đổi sang DTO
			var result = items.Select(item => new ProductListDto
			{
				Id = item.Id,
				Code = item.Code,
				Name = item.Name,
				Unit = item.Unit,
				Weight = item.Weight,
				Volume = item.Volume,
				Description = item.Description,
				CategoryName = item.Category?.Name,
				Barcode = item.Barcode,
				IsActive = item.IsActive,
				SupplierName = item.Supplier?.Name,
				CreationTime = item.CreationTime,
				Image = item.Image
			}).ToList();

			// Trả về kết quả
			return new PagedResultDto<ProductListDto>
			{
				Items = result,
				TotalCount = totalCount
			};
		}

		public async Task<ProductListDto> GetProductById(int productId)
		{
			var product = await _productRepository.GetAllIncluding(x => x.Category, x => x.Supplier)
				.FirstOrDefaultAsync(x => x.Id == productId);
			if (product == null)
			{
				throw new UserFriendlyException("Product not found");
			}
			return new ProductListDto
			{
				Id = product.Id,
				Code = product.Code,
				Name = product.Name,
				Description = product.Description,
				Image = product.Image,
				CategoryId = product.CategoryId,
				Barcode = product.Barcode,
				Unit = product.Unit,
				Weight = product.Weight,
				Volume = product.Volume,
				IsActive = product.IsActive,
				SupplierId = product.SupplierId
			};
		}

		public async Task<Dictionary<int, ProductListDto>> GetProductsByIds(List<int> productIds)
		{
			var products = await _productRepository.GetAllIncluding(x => x.Category, x => x.Supplier)
					.Where(x => productIds.Contains(x.Id))
					.ToListAsync();

			return products.ToDictionary(
					p => p.Id,
					p => new ProductListDto
					{
						Id = p.Id,
						Code = p.Code,
						Name = p.Name,
						Description = p.Description,
						Image = p.Image,
						CategoryId = p.CategoryId,
						Barcode = p.Barcode,
						Unit = p.Unit,
						Weight = p.Weight,
						Volume = p.Volume,
						IsActive = p.IsActive,
						SupplierId = p.SupplierId
						// Thêm các trường khác nếu cần
					});
		}
		public async Task<byte[]> ExportProductsToExcel(ProductInput input)
		{
			var products = await GetAllProducts(input);


			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Products");

				// Tiêu đề cột
				worksheet.Cells[1, 1].Value = "Mã sản phẩm";
				worksheet.Cells[1, 2].Value = "Tên sản phẩm";
				worksheet.Cells[1, 3].Value = "Mô tả";
				worksheet.Cells[1, 4].Value = "Danh mục";
				worksheet.Cells[1, 5].Value = "Barcode";
				worksheet.Cells[1, 6].Value = "Đơn vị";
				worksheet.Cells[1, 7].Value = "Trọng lượng";
				worksheet.Cells[1, 8].Value = "Thể tích";
				worksheet.Cells[1, 9].Value = "Nhà cung cấp";
				worksheet.Cells[1, 10].Value = "Kích hoạt";
				worksheet.Cells[1, 11].Value = "Ảnh sản phẩm";

				// Đổ dữ liệu
				for (int i = 0; i < products.Items.Count; i++)
				{
					var row = i + 2;
					var product = products.Items[i];

					worksheet.Cells[row, 1].Value = product.Code;
					worksheet.Cells[row, 2].Value = product.Name;
					worksheet.Cells[row, 3].Value = product.Description;
					worksheet.Cells[row, 4].Value = product.CategoryName;
					worksheet.Cells[row, 5].Value = product.Barcode;
					worksheet.Cells[row, 6].Value = product.Unit;
					worksheet.Cells[row, 7].Value = product.Weight;
					worksheet.Cells[row, 8].Value = product.Volume;
					worksheet.Cells[row, 9].Value = product.SupplierName;
					worksheet.Cells[row, 10].Value = product.IsActive ? "Có" : "Không";

					// Thêm ảnh nếu có
					// Thêm ảnh nếu có
					if (!string.IsNullOrEmpty(product.Image))
					{
						try
						{
							var imagePath = Path.Combine(@"E:\UploadImgKho\", Path.GetFileName(product.Image));
							if (File.Exists(imagePath))
							{
								using var image = Image.FromFile(imagePath);
								using var imageStream = new MemoryStream();
								image.Save(imageStream, image.RawFormat);
								imageStream.Position = 0;

								var picture = worksheet.Drawings.AddPicture($"img_{i}", imageStream);
								picture.SetPosition(row - 1, 0, 10, 0);
								picture.SetSize(100, 100);
							}
						}
						catch
						{
							// Bỏ qua nếu không load được ảnh
						}
					}
				}

				// AutoFit columns
				worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

				return package.GetAsByteArray();
			}
		}
		public async Task<List<ImportProductResultDto>> ImportProductsFromExcel(IFormFile file)
		{
			var getAllCategories = await _categoryRepository.GetAllListAsync();
			var getAllSuppliers = await _supplierRepository.GetAllListAsync();
			// Thiết lập license
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			var results = new List<ImportProductResultDto>();
			var uploadsFolder = @"E:\UploadImgKho\";

			// Kiểm tra và tạo thư mục
			Directory.CreateDirectory(uploadsFolder);

			// Validate file
			if (file == null || file.Length == 0)
				throw new Exception("File không tồn tại");

			if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
				throw new Exception("Chỉ hỗ trợ file Excel (.xlsx)");

			using (var stream = new MemoryStream())
			{
				await file.CopyToAsync(stream);

				using (var package = new ExcelPackage(stream))
				{
					if (package.Workbook.Worksheets.Count == 0)
						throw new Exception("File Excel không có sheet nào");

					var worksheet = package.Workbook.Worksheets[0];
					int rowCount = worksheet.Dimension?.Rows ?? 0;

					for (int row = 2; row <= rowCount; row++)
					{
						var result = new ImportProductResultDto
						{
							RowNumber = row,
							Code = worksheet.Cells[row, 1]?.Text?.Trim(),
							Name = worksheet.Cells[row, 2]?.Text?.Trim()
						};

						try
						{
							var existingProduct = await _productRepository.FirstOrDefaultAsync(p => p.Code == result.Code);
							if (existingProduct != null)
							{
								// Cập nhật thông tin sản phẩm
								existingProduct.Name = worksheet.Cells[row, 2]?.Text?.Trim();
								// ... các thông tin khác

								await _productRepository.UpdateAsync(existingProduct);
								result.IsSuccess = true;
								result.Message = "Cập nhật sản phẩm thành công";
								results.Add(result);
								continue;
							}
								string categoryName = worksheet.Cells[row, 4]?.Text?.Trim();
							string supplierName = worksheet.Cells[row, 9]?.Text?.Trim();
							var category = getAllCategories
											.FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

							if (category == null)
								throw new Exception($"Không tìm thấy danh mục: {categoryName}");

							var supplier = getAllSuppliers
									.FirstOrDefault(s => s.Name.Equals(supplierName, StringComparison.OrdinalIgnoreCase));

							if (supplier == null)
								throw new Exception($"Không tìm thấy nhà cung cấp: {supplierName}");
							var productDto = new CreateProductDto
							{
								Code = result.Code,
								Name = result.Name,
								CategoryId = category.Id,
								SupplierId = supplier.Id,
								Description = worksheet.Cells[row, 3]?.Text?.Trim(),
								Barcode = worksheet.Cells[row, 5]?.Text?.Trim(),
								Unit = worksheet.Cells[row, 6]?.Text?.Trim(),
								Weight = decimal.TryParse(worksheet.Cells[row, 7]?.Text, out var weight) ? weight : 0,
								Volume = decimal.TryParse(worksheet.Cells[row, 8]?.Text, out var volume) ? volume : 0,
								//IsActive = worksheet.Cells[row, 10]?.Text?.Trim().Equals("Có", StringComparison.OrdinalIgnoreCase) ?? false
								IsActive = bool.TryParse(worksheet.Cells[row, 10]?.Text, out var isActive) && isActive
							};

							// Xử lý ảnh
							//foreach (var drawing in worksheet.Drawings)
							//{
							//	if (drawing is ExcelPicture picture)
							//	{
							//		var imageName = $"{Guid.NewGuid()}.jpg"; // Mặc định là jpg
							//		var imagePath = Path.Combine(uploadsFolder, imageName);

							//		// Lưu trực tiếp bytes ra file
							//		File.WriteAllBytes(imagePath, picture.Image.ImageBytes);

							//		productDto.Image = $"/products/{imageName}";
							//	}
							//}
							foreach (var drawing in worksheet.Drawings)
							{
								if (drawing is ExcelPicture picture)
								{
									// Ánh xạ hình vào đúng dòng
									if (picture.From.Row + 1 == row)
									{
										var imageName = $"{Guid.NewGuid()}.jpg";
										var imagePath = Path.Combine(uploadsFolder, imageName);

										File.WriteAllBytes(imagePath, picture.Image.ImageBytes);
										productDto.Image = $"/products/{imageName}";
									}
								}
							}
							await Create(productDto);
							result.IsSuccess = true;
							result.Message = "Thành công";
						}
						catch (Exception ex)
						{
							result.IsSuccess = false;
							result.Message = $"Dòng {row}: {ex.Message}";
						}

						results.Add(result);
					}
				}
			}

			return results;
		}
	}
}
