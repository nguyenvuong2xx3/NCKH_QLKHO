using Abp.AutoMapper;
using QLKho_NCKH.Sessions.Dto;

namespace QLKho_NCKH.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}
