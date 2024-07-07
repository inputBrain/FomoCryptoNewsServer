using FomoCryptoNews.Parsers.Cryptoslate;
using Microsoft.Extensions.Logging;

namespace FomoCryptoNews.UseCase;

public static class Factory
{
    public static UseCaseContainer Create(ILoggerFactory loggerFactory, ICryptoslateParser cryptoslateParser)
    {
        return new UseCaseContainer(
            new CryptoslateUseCase(loggerFactory.CreateLogger<CryptoslateUseCase>(), cryptoslateParser)
            );
    }
}