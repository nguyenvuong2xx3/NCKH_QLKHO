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
namespace QLKho_NCKH.Products
{
	[AbpMvcAuthorize(PermissionNames.Pages_Products)]

	public class ProductAppService : ApplicationService, IProductAppService
	{
		private readonly IRepository<Product> _productRepository;
		//private readonly IRepository<Category> _categoryRepository;
		private readonly IWebHostEnvironment _env;

		public ProductAppService(IRepository<Product> productRepository, IWebHostEnvironment env) // IRepository<Category> categoryRepository,
		{
			_productRepository = productRepository;
			//_categoryRepository = categoryRepository;
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

	}
}
