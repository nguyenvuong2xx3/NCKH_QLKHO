using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using QLKho_NCKH.Categories;
using QLKho_NCKH.Products.Dtos;
using QLKho_NCKH.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace QLKho_NCKH.Products
{
	public interface IProductFontendAppService
	{
		Task<PagedResultDto<ProductListDto>> GetAllProducts(ProductInput input);
		Task<Dictionary<int, ProductListDto>> GetProductsByIds(List<int> productIds);
	}

	public class ProductFontendAppService : QLKho_NCKHAppServiceBase, IProductFontendAppService
	{
		private readonly IRepository<Product> _productRepository;
		private readonly IRepository<Category> _categoryRepository;
		private readonly IRepository<Supplier> _supplierRepository;
		private readonly IWebHostEnvironment _env;

		public ProductFontendAppService(
			IRepository<Product> productRepository,
			IRepository<Category> categoryRepository,
			IRepository<Supplier> supplierRepository,
			IWebHostEnvironment env)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
			_supplierRepository = supplierRepository;
			_env = env;
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

	}
}
