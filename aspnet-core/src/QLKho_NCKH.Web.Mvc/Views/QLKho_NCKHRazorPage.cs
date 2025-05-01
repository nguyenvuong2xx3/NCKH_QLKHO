using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace QLKho_NCKH.Web.Views
{
    public abstract class QLKho_NCKHRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected QLKho_NCKHRazorPage()
        {
            LocalizationSourceName = QLKho_NCKHConsts.LocalizationSourceName;
        }
    }
}
