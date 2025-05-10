using Abp.AutoMapper;
using QLKho_NCKH.Roles.Dto;
using QLKho_NCKH.Web.Models.Common;

namespace QLKho_NCKH.Web.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class EditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool HasPermission(FlatPermissionDto permission)
        {
            return GrantedPermissionNames.Contains(permission.Name);
        }
    }
}
