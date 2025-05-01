using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using QLKho_NCKH.Authorization.Users;
using QLKho_NCKH.Editions;

namespace QLKho_NCKH.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager,
                featureValueStore)
        {
        }
    }
}
