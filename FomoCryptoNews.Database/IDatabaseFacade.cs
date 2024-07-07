using FomoCryptoNews.Database.Cryptoslate;

namespace FomoCryptoNews.Database;

public interface IDatabaseFacade
{
    ICryptoslateNewsRepository CryptoslateNewsRepository { get; }
}