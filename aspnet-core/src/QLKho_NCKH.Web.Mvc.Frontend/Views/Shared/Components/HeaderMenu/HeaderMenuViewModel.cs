using Abp.Application.Navigation;
using Abp.Application.Services.Dto;
using QLKho_NCKH.Sessions.Dto;

namespace QLKho_NCKH.Web.Views.Shared.Components.HeaderMenu
{
	public class HeaderMenuViewModel
	{
		public GetCurrentLoginInformationsOutput LoginInformations { get; set; }


		public bool IsMultiTenancyEnabled { get; set; }

		public string GetShownLoginName()
		{
			if (LoginInformations.User != null)
			{
				var userName = LoginInformations.User.Name;

				if (!IsMultiTenancyEnabled)
				{
					return userName;
				}

				return userName;
			}
			else
			{
				return "";
			}
		}
	}
}
