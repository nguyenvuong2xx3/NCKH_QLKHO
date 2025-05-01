using System.Threading.Tasks;
using Abp.Application.Services;
using QLKho_NCKH.Authorization.Accounts.Dto;

namespace QLKho_NCKH.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
