using FomoCryptoNews.Database.TestNews;
using Microsoft.Extensions.Logging;

namespace FomoCryptoNews.Database;

public class DatabaseFacade : IDatabaseFacade
{
    public ITestNewsRepository NewsRepository { get; set; }

    public DatabaseFacade(PostgreSqlContext context, ILoggerFactory loggerFactory)
    {
        NewsRepository = new TestNewsRepository(context, loggerFactory);
    }
}

