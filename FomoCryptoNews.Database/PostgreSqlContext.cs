using FomoCryptoNews.Database.TestNews;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FomoCryptoNews.Database;

public class PostgreSqlContext : DbContext
{
    public readonly IDatabaseFacade Db;
    
    public DbSet<TestNewsModel> TestNewsModel { get; set; }

    
    public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options, ILoggerFactory loggerFactory) : base(options)
    {
        Db = new DatabaseFacade(this, loggerFactory);
    }
}
