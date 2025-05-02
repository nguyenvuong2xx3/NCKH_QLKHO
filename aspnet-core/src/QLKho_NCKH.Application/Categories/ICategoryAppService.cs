using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QLKho_NCKH.Categories.Dto;

namespace QLKho_NCKH.Categories
{
	public interface ICategoryAppService : IApplicationService
	{
		Task<CategoryListDto> Create(CreateCategoryDto input);
		Task<PagedResultDto<CategoryListDto>> GetAllAsync(GetAllCategoriesInput input);
		Task<CategoryListDto> GetCategoryById(int? categoryId);
		Task<CategoryListDto> Update(UpdateCategoryDto input);
		Task Delete(int id);
		Task<List<CategoryListDto>> GetAllCategories();
	}
}
