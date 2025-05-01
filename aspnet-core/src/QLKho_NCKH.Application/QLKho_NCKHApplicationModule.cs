using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using QLKho_NCKH.Authorization;

namespace QLKho_NCKH
{
    [DependsOn(
        typeof(QLKho_NCKHCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class QLKho_NCKHApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<QLKho_NCKHAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(QLKho_NCKHApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
