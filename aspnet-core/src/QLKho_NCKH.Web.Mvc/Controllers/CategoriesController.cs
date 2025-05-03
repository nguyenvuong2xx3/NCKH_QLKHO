using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.Categories;
using QLKho_NCKH.Web.Models.Categories;
using Abp.AspNetCore.Mvc.Authorization;
using QLKho_NCKH.Authorization;

namespace QLKho_NCKH.Web.Controllers
{
	[AbpMvcAuthorize(PermissionNames.Pages_Categories)]

	public class CategoriesController : QLKho_NCKHControllerBase
	{
		private readonly ICategoryAppService _categoryAppService;
		public CategoriesController(ICategoryAppService categoryAppService)
		{
			_categoryAppService = categoryAppService;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<ActionResult> EditModal(int categoryId)
		{
			var category = await _categoryAppService.GetCategoryById(categoryId);

			var model = new EditCategoryViewModel
			{
				Category = category
			};

			return PartialView("_EditModal", model);
		}

		public async Task<ActionResult> DetailModal(int categoryId)
		{
			var category = await _categoryAppService.GetCategoryById(categoryId);

			var model = new CategoryDetailViewModel
			{
				Category = category
			};


			return PartialView("_DetailModal", model);
		}

	}
}
