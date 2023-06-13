using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;

namespace MigraineDiary.Tests.Mocks.Database
{
    public static class InMemoryDatabase
    {
        public static ApplicationDbContext Instance()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
