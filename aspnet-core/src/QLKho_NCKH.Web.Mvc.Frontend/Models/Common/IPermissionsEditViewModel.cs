using System.Collections.Generic;
using QLKho_NCKH.Roles.Dto;

namespace QLKho_NCKH.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}