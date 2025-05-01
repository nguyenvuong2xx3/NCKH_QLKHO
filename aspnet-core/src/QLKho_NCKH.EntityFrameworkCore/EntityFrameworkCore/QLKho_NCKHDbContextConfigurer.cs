using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace QLKho_NCKH.EntityFrameworkCore
{
    public static class QLKho_NCKHDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<QLKho_NCKHDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<QLKho_NCKHDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
