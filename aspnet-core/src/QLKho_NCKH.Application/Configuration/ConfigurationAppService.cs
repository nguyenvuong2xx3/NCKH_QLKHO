using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using QLKho_NCKH.Configuration.Dto;

namespace QLKho_NCKH.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : QLKho_NCKHAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
