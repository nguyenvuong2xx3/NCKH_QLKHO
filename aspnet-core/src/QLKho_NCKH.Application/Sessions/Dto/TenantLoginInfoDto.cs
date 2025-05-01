using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using QLKho_NCKH.MultiTenancy;

namespace QLKho_NCKH.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
