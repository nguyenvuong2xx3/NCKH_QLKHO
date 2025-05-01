using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace QLKho_NCKH.Controllers
{
    public abstract class QLKho_NCKHControllerBase: AbpController
    {
        protected QLKho_NCKHControllerBase()
        {
            LocalizationSourceName = QLKho_NCKHConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
