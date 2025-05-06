using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using QLKho_NCKH.Products.Dtos;
using QLKho_NCKH.Products.Dtos.YourNamespace.Products.Dtos;

namespace QLKho_NCKH.Products
{
	public interface IProductAppService : IApplicationService
	{
		Task<ProductListDto> Create(CreateProductDto input);
		Task<PagedResultDto<ProductListDto>> GetAllProducts(ProductInput input);

		Task<ProductListDto> GetProductByCode(string code);

		//Task<ProductListDto> SearchProduct(SearchProductDto input);

		Task<ProductListDto> GetByIdProducts(EntityDto<int> input);

		Task Delete(EntityDto<int> input);

		Task<ProductListDto> Update(UpdateProductDto input);

		Task<PagedResultDto<ProductListDto>> SearchProducts(GetAllProductsInput input);

		Task<ProductListDto> GetProductById(int productId);



	}
}
