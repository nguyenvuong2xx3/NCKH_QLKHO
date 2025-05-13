using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.Castle.Logging.Log4Net;
using QLKho_NCKH.Authentication.JwtBearer;
using QLKho_NCKH.Configuration;
using QLKho_NCKH.Identity;
using QLKho_NCKH.Web.Resources;
using Abp.AspNetCore.SignalR.Hubs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.WebEncoders;
using System;

namespace QLKho_NCKH.Web.Startup
{
	public class Startup
	{
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly IConfigurationRoot _appConfiguration;

		public Startup(IWebHostEnvironment env)
		{
			_hostingEnvironment = env;
			_appConfiguration = env.GetAppConfiguration();
		}

        public void ConfigureServices(IServiceCollection services)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

            // MVC
            services.AddControllersWithViews(
                    options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                        options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
                    }
                );

			IdentityRegistrar.Register(services);
			AuthConfigurer.Configure(services, _appConfiguration);

			services.Configure<WebEncoderOptions>(options =>
			{
				options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
			});

			services.AddScoped<IWebResourceManager, WebResourceManager>();

			services.AddSignalR();

			// Configure Abp and Dependency Injection
			services.AddAbpWithoutCreatingServiceProvider<QLKho_NCKHWebMvcModule>(
					// Configure Log4Net logging
					options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
							f => f.UseAbpLog4Net().WithConfig(
									_hostingEnvironment.IsDevelopment()
											? "log4net.config"
											: "log4net.Production.config"
									)
					)
			);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			app.UseAbp(); // Initializes ABP framework.

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseStaticFiles();

			// Thêm hỗ trợ truy cập thư mục ảnh bên ngoài
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(@"E:\UploadImgKho\"),
				RequestPath = "/products"
			});

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(@"E:\UploadImgKho\"),
				RequestPath = "/sliders"
			});

			app.UseRouting();

			app.UseAuthentication();

			app.UseJwtTokenMiddleware();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHub<AbpCommonHub>("/signalr");
				endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
