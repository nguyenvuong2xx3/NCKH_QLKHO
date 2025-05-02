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
using Abp.Collections.Extensions;
//using Abp.Authorization;
//using Acme.SimpleTaskApp.Authorization;
namespace QLKho_NCKH.Products
{
	//[AbpAuthorize]
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

			 await  _productRepository.InsertAsync(product);

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

		public Task<ProductListDto> UpdateProducts(UpdateProductDto input)
		{
			throw new NotImplementedException();
		}

		public Task<PagedResultDto<ProductListDto>> SearchProducts(GetAllProductsInput input)
		{
			throw new NotImplementedException();
		}

		public Task DeleteProducts(EntityDto<int> input)
		{
			throw new NotImplementedException();
		}

		public async Task<PagedResultDto<ProductListDto>> GetAllProducts(ProductInput input)
		{
			var query = _productRepository.GetAll()
				.WhereIf(!input.Filter.IsNullOrEmpty(), x => x.Name.Contains(input.Filter) || x.Description.Contains(input.Filter));

			//.WhereIf(input.CategoryId.HasValue, x => x.CategoryId == input.CategoryId.Value)
			//.WhereIf(input.State.HasValue, x => x.State == input.State.Value);
			query = query.OrderByDescending(x => x.CreationTime).OrderBy(input.Sorting).ThenBy(x => x.Id);
			var items = await query.PageBy(input).ToListAsync();
			var itemsCount = await query.CountAsync();
			var result = new List<ProductListDto>();
			foreach (var item in items)
			{
				var product = new ProductListDto
				{
					Id = item.Id,
					Code = item.Code,
					Name = item.Name,
					Description = item.Description,
					CategoryName = item.Category.Name,
					Barcode = item.Barcode,
					IsActive = item.IsActive,
					SupplierName = item.Supplier.Name,
					CreationTime = item.CreationTime,
				};
				result.Add(product);
			}
			return new PagedResultDto<ProductListDto>()
			{
				Items = result,
				TotalCount = itemsCount,
			};
		}
	}
}
