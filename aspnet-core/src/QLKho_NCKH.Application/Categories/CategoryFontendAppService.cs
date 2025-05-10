using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using QLKho_NCKH;
using QLKho_NCKH.Categories;
using QLKho_NCKH.Categories.Dto;
using QLKho_NCKH.Products;

namespace MyProject.Categories
{
	public interface ICategoryFontendAppService
	{
		Task<PagedResultDto<CategoryListDto>> GetCategory(GetAllCategoriesInput input);
	}

	public class CategoryFontendAppService : QLKho_NCKHAppServiceBase, ICategoryFontendAppService
	{
		private readonly IRepository<Category> _categoryRepository;
		private readonly IRepository<Product> _productRepository;

		public CategoryFontendAppService(IRepository<Category> categoryRepository, IRepository<Product> productRepository)
		{
			_categoryRepository = categoryRepository;
			_productRepository = productRepository;
		}

		public async Task<PagedResultDto<CategoryListDto>> GetCategory(GetAllCategoriesInput input)
		{
			using var uow = UnitOfWorkManager.Begin();
			using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
			{
				try
				{
					var categories = _categoryRepository.GetAll();


					// Tính tổng số lượng sản phẩm phù hợp với điều kiện tìm kiếm
					var totalCount = await categories.CountAsync();

					// Lấy danh sách các sản phẩm phù hợp, phân trang và chuyển đổi thành DTO
					var categoryDtos = await categories.OrderBy(x => x.Name)  // 
																							 .PageBy(input)               // Phân trang theo input
																							 .Select(p => new CategoryListDto
																							 {
																								 Id = p.Id,
																								 Name = p.Name,
																								 Description = p.Description
																							 }).ToListAsync();  // Thực thi truy vấn và chuyển thành danh sách DTO

					// Trả về kết quả phân trang
					return new PagedResultDto<CategoryListDto>(totalCount, categoryDtos);
				}
				catch (Exception ex)
				{
					return null;
				}
				finally
				{
					await uow.CompleteAsync();
				}

			}
		}

	}
}
