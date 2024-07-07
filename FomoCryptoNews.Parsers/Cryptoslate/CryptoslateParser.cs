using FomoCryptoNews.Api.Service;
using FomoCryptoNews.ExternalDto.Cryptoslate;
using Microsoft.Extensions.Logging;

namespace FomoCryptoNews.Parsers.Cryptoslate;

public class CryptoslateParser : ICryptoslateParser
{
    private const string BaseUrl = "https://cryptoslate.com";
    
    private readonly ILogger<CryptoslateParser> _logger;
    private readonly IBaseParser _baseParser;
    
    public CryptoslateParser(ILogger<CryptoslateParser> logger, IBaseParser baseParser)
    {
        _logger = logger;
        _baseParser = baseParser;
    }


    public Task<CryptoslateNewsDto> ParseAllNews(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}