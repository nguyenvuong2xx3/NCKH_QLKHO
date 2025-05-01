using Abp.Authorization;
using QLKho_NCKH.Authorization.Roles;
using QLKho_NCKH.Authorization.Users;

namespace QLKho_NCKH.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
