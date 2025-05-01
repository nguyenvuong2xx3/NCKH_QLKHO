using Abp.Application.Services;
using QLKho_NCKH.MultiTenancy.Dto;

namespace QLKho_NCKH.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

