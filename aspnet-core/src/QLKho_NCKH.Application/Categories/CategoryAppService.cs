using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using QLKho_NCKH.Authorization;
using QLKho_NCKH.Categories.Dto;

namespace QLKho_NCKH.Categories
{
	[AbpAuthorize(PermissionNames.Pages_Categories)]

	public class CategoryAppService : QLKho_NCKHAppServiceBase, ICategoryAppService
	{
		private readonly IRepository<Category> _categoryRepository;
		public CategoryAppService(IRepository<Category> categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public async Task<PagedResultDto<CategoryListDto>> GetAllAsync(GetAllCategoriesInput input)
		{
			var categories =  _categoryRepository.GetAll();

			if (!string.IsNullOrWhiteSpace(input.Filter))
			{
				categories = categories.Where(c => c.Name.Contains(input.Filter) || c.Description.Contains(input.Filter));
			}

			var totalCount = await categories.CountAsync();

			var categoryList = await categories.PageBy(input).OrderByDescending(c => c.CreationTime).Select(c => new CategoryListDto
			{
				Id = c.Id,
				Name = c.Name,
				Description = c.Description,
				ParentId = c.ParentId,
			}).ToListAsync();

			return new PagedResultDto<CategoryListDto>(totalCount, categoryList);

		}

		public async Task<CategoryListDto> Create(CreateCategoryDto input)
		{
			var newCategory = new Category
			{
				Id = input.Id,
				Name = input.Name,
				Description = input.Description,
				ParentId = input.ParentId,
			};

			var category = await _categoryRepository.InsertAsync(newCategory);
			return new CategoryListDto
			{
				Id = category.Id,
				Name = category.Name,
				Description = category.Description,
				ParentId = category.ParentId,
			};
		}

		public async Task<CategoryListDto> Update(UpdateCategoryDto input)
		{
			var category = await _categoryRepository.GetAsync(input.Id);

			category.Name = input.Name;
			category.Description = input.Description;

			await _categoryRepository.UpdateAsync(category);

			return new CategoryListDto
			{
				Id = category.Id,
				Name = category.Name,
				Description = category.Description
			};
		}

		public async Task<CategoryListDto> GetCategoryById(int? categoryId)
		{
			var category = _categoryRepository.FirstOrDefault(x => x.Id == categoryId);

			if (category == null)
			{
				return null;
			}

			return new CategoryListDto
			{
				Id = category.Id,
				Name = category.Name,
				Description = category.Description,
				CreateTime = category.CreationTime,
			};
		}

		public async Task Delete(int id)
		{
			var category = await _categoryRepository.GetAsync(id);
			await _categoryRepository.DeleteAsync(category);
		}

		public async Task<List<CategoryListDto>> GetAllCategories()
		{
			var categories = await _categoryRepository.GetAllListAsync();
			return categories.Select(c => new CategoryListDto
			{
				Id = c.Id,
				Name = c.Name,
				Description = c.Description,
				CreateTime = c.CreationTime,
			}).ToList();
		}

	}
}
