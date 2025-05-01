namespace QLKho_NCKH.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly QLKho_NCKHDbContext _context;

        public InitialHostDbBuilder(QLKho_NCKHDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
