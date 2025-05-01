using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using QLKho_NCKH.Configuration;
using QLKho_NCKH.EntityFrameworkCore;
using QLKho_NCKH.Migrator.DependencyInjection;

namespace QLKho_NCKH.Migrator
{
    [DependsOn(typeof(QLKho_NCKHEntityFrameworkModule))]
    public class QLKho_NCKHMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public QLKho_NCKHMigratorModule(QLKho_NCKHEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(QLKho_NCKHMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                QLKho_NCKHConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QLKho_NCKHMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
