using Abp.Application.Services.Dto;
using QLKho_NCKH.Categories.Dto;

namespace QLKho_NCKH.Web.Views.Shared.Components.CategoryMenu
{
	public class CategoryMenuViewModel
	{
		public ListResultDto<CategoryListDto> Categories { get; set; }
	}
}
