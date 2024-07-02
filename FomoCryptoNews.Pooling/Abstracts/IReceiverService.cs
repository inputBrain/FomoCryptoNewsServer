using System.Threading;
using System.Threading.Tasks;

namespace FomoCryptoNews.Pooling.Abstracts;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken stoppingToken);
}