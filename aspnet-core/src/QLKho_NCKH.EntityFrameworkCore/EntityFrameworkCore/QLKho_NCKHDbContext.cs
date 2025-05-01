using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using QLKho_NCKH.Authorization.Roles;
using QLKho_NCKH.Authorization.Users;
using QLKho_NCKH.MultiTenancy;

namespace QLKho_NCKH.EntityFrameworkCore
{
    public class QLKho_NCKHDbContext : AbpZeroDbContext<Tenant, Role, User, QLKho_NCKHDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public QLKho_NCKHDbContext(DbContextOptions<QLKho_NCKHDbContext> options)
            : base(options)
        {
        }
    }
}
