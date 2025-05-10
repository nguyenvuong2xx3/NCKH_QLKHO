//using Abp.Configuration.Startup;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

//namespace QLKho_NCKH.Web.Views.Shared.Components.CategoryMenu
//{
//	public class CategoryMenuViewComponent : QLKho_NCKHViewComponent
//	{
//		private readonly ICategoryFontendAppService _categoryFontendAppService;

//		public CategoryMenuViewComponent(ICategoryFontendAppService categoryFontendAppService)
//		{
//			_categoryFontendAppService = categoryFontendAppService;
//		}

//		public async Task<IViewComponentResult> InvokeAsync()
//		{
//			var model = new CategoryMenuViewModel
//			{
//				Categories = await _categoryFontendAppService.GetCategory(new GetAllCategoriesInput())
//			};

//			return View(model);
//		}
//	}
//}
