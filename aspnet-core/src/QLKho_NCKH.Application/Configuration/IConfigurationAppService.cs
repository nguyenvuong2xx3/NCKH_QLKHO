using System.Threading.Tasks;
using QLKho_NCKH.Configuration.Dto;

namespace QLKho_NCKH.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
