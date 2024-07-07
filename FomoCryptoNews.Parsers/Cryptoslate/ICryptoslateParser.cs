using FomoCryptoNews.ExternalDto.Cryptoslate;

namespace FomoCryptoNews.Parsers.Cryptoslate;

public interface ICryptoslateParser
{
    Task<List<CryptoslateNewsDto>> ParseNewsByAmountOfPages(int pageParseCount, CancellationToken cancellationToken);
}