using QLKho_NCKH.Categories.Dto;
using System.Collections.Generic;

namespace QLKho_NCKH.Web.Models.Categories
{
	public class CategoryViewModel
	{
		public IReadOnlyList<CategoryListDto> Categories;

		public CategoryViewModel(IReadOnlyList<CategoryListDto> categories)
		{
			this.Categories = categories;
		}
	}
}
