using System.Collections.Generic;
using System.Linq;
using QLKho_NCKH.Roles.Dto;
using QLKho_NCKH.Users.Dto;

namespace QLKho_NCKH.Web.Models.Users
{
    public class EditUserModalViewModel
    {
        public UserDto User { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }

        public bool UserIsInRole(RoleDto role)
        {
            return User.RoleNames != null && User.RoleNames.Any(r => r == role.NormalizedName);
        }
    }
}
