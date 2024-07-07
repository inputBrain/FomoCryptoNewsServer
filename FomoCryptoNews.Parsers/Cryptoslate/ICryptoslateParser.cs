using FomoCryptoNews.ExternalDto.Cryptoslate;

namespace FomoCryptoNews.Parsers.Cryptoslate;

public interface ICryptoslateParser
{
    Task<CryptoslateNewsDto> ParseAllNews(CancellationToken cancellationToken);

}