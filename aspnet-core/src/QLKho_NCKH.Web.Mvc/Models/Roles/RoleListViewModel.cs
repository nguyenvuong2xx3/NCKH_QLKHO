using System.Collections.Generic;
using QLKho_NCKH.Roles.Dto;

namespace QLKho_NCKH.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
