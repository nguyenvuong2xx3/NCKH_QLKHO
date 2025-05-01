using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using QLKho_NCKH.Configuration;
using QLKho_NCKH.Web;

namespace QLKho_NCKH.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class QLKho_NCKHDbContextFactory : IDesignTimeDbContextFactory<QLKho_NCKHDbContext>
    {
        public QLKho_NCKHDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<QLKho_NCKHDbContext>();
            
            /*
             You can provide an environmentName parameter to the AppConfigurations.Get method. 
             In this case, AppConfigurations will try to read appsettings.{environmentName}.json.
             Use Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") method or from string[] args to get environment if necessary.
             https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#args
             */
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            QLKho_NCKHDbContextConfigurer.Configure(builder, configuration.GetConnectionString(QLKho_NCKHConsts.ConnectionStringName));

            return new QLKho_NCKHDbContext(builder.Options);
        }
    }
}
