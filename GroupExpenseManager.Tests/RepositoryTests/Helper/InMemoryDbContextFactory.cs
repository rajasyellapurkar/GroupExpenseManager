using GroupExpenseManager.API.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace GroupExpenseManager.Tests.RepositoryTests.Helper
{
    public class InMemoryDbContextFactory
    {
        public DataContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                            .UseInMemoryDatabase(databaseName: "InMemoryArticleDatabase")
                            .Options;
            var dbContext = new DataContext(options);

            return dbContext;
        }
    }
}