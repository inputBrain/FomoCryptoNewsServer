using FomoCryptoNews.Database.Cryptoslate;
using Microsoft.Extensions.Logging;

namespace FomoCryptoNews.Database;

public class DatabaseFacade : IDatabaseFacade
{
    public DatabaseFacade(PostgreSqlContext context, ILoggerFactory loggerFactory)
    {
        CryptoslateNewsRepository = new CryptoslateNewsRepository(context, loggerFactory);
    }


    public ICryptoslateNewsRepository CryptoslateNewsRepository { get; set; }
}