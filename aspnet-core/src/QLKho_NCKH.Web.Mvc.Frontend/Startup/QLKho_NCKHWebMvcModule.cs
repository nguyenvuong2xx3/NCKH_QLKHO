using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using QLKho_NCKH.Configuration;

namespace QLKho_NCKH.Web.Startup
{
    [DependsOn(typeof(QLKho_NCKHWebCoreModule))]
    public class QLKho_NCKHWebMvcModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public QLKho_NCKHWebMvcModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<QLKho_NCKHNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QLKho_NCKHWebMvcModule).GetAssembly());
        }
    }
}
