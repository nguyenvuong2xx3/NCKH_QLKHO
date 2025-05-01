using Abp.Application.Services.Dto;

namespace QLKho_NCKH.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

