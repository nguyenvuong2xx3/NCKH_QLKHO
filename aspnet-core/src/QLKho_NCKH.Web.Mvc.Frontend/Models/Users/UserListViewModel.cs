using System.Collections.Generic;
using QLKho_NCKH.Roles.Dto;

namespace QLKho_NCKH.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
