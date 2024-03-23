using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EKtu.Persistence
{
    internal class DesignTime : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder();
            dbContextOptionsBuilder.UseSqlServer(SqlConnectionString.GetConnectionString);

            return new DatabaseContext(dbContextOptionsBuilder.Options);
        }
    }
}
