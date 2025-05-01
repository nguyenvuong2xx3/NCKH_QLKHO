using System.Threading.Tasks;
using Abp.Application.Services;
using QLKho_NCKH.Sessions.Dto;

namespace QLKho_NCKH.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
