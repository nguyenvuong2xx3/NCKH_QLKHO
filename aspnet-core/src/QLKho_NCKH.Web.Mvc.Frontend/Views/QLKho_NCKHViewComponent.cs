using Abp.AspNetCore.Mvc.ViewComponents;

namespace QLKho_NCKH.Web.Views
{
    public abstract class QLKho_NCKHViewComponent : AbpViewComponent
    {
        protected QLKho_NCKHViewComponent()
        {
            LocalizationSourceName = QLKho_NCKHConsts.LocalizationSourceName;
        }
    }
}
