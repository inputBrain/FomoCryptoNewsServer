using FomoCryptoNews.Database.Cryptoslate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FomoCryptoNews.Database;

public class PostgreSqlContext : DbContext
{
    public readonly IDatabaseFacade Db;

    public DbSet<CryptoslateNewsModel> CryptoslateNews { get; set; }


    public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options, ILoggerFactory loggerFactory) : base(options)
    {
        Db = new DatabaseFacade(this, loggerFactory);
    }
}