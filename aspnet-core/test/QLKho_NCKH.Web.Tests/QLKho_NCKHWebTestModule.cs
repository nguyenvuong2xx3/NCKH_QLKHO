using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using QLKho_NCKH.EntityFrameworkCore;
using QLKho_NCKH.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace QLKho_NCKH.Web.Tests
{
    [DependsOn(
        typeof(QLKho_NCKHWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class QLKho_NCKHWebTestModule : AbpModule
    {
        public QLKho_NCKHWebTestModule(QLKho_NCKHEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QLKho_NCKHWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(QLKho_NCKHWebMvcModule).Assembly);
        }
    }
}