using FomoCryptoNews.Parsers.Cryptoslate;
using Microsoft.Extensions.Logging;

namespace FomoCryptoNews.UseCase;

public class CryptoslateUseCase : ICryptoslateUseCase
{
    private readonly ILogger<CryptoslateUseCase> _logger;
    private readonly ICryptoslateParser _parser;
    
    
    public CryptoslateUseCase(ILogger<CryptoslateUseCase> logger, ICryptoslateParser parser)
    {
        _logger = logger;
        _parser = parser;
    }
}