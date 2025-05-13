using System.Threading.Tasks;
using Abp.Configuration.Startup;
using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Sessions;

namespace QLKho_NCKH.Web.Views.Shared.Components.HeaderMenu
{
	public class HeaderMenuViewComponent : QLKho_NCKHViewComponent
	{
		private readonly ISessionAppService _sessionAppService;
		private readonly IMultiTenancyConfig _multiTenancyConfig;
		//private readonly ICategoryFontendAppService _categoryFontendAppService;

		public HeaderMenuViewComponent(
				ISessionAppService sessionAppService,
				IMultiTenancyConfig multiTenancyConfig
				//,ICategoryFontendAppService categoryFontendAppService
			)
		{
			_sessionAppService = sessionAppService;
			_multiTenancyConfig = multiTenancyConfig;
			//_categoryFontendAppService = categoryFontendAppService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var model = new HeaderMenuViewModel
			{
				LoginInformations = await _sessionAppService.GetCurrentLoginInformations(),
				IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
			};

			return View(model);
		}
	}
}
