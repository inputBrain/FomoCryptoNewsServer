using System.Threading;
using System.Threading.Tasks;

namespace FomoCryptoNews.Api.Service;

public interface IBaseParser
{
    Task<T> ParseGet<T>(string url, CancellationToken cancellationToken);
}