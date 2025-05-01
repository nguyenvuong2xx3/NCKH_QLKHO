using Abp.MultiTenancy;
using QLKho_NCKH.Authorization.Users;

namespace QLKho_NCKH.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
