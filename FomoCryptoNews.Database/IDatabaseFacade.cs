using FomoCryptoNews.Database.TestNews;

namespace FomoCryptoNews.Database;

public interface IDatabaseFacade
{
    ITestNewsRepository NewsRepository { get; }
}

