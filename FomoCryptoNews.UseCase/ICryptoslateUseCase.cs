using System.Threading;
using System.Threading.Tasks;
using FomoCryptoNews.Database.Cryptoslate;

namespace FomoCryptoNews.UseCase;

public interface ICryptoslateUseCase
{
    Task<bool> ParseNews(ICryptoslateNewsRepository newsRepository, CancellationToken cancellationToken);
}