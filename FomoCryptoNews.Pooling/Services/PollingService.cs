using System;
using FomoCryptoNews.Pooling.Abstracts;
using Microsoft.Extensions.Logging;

namespace FomoCryptoNews.Pooling.Services;

public class PollingService : PollingServiceBase<ReceiverService>
{
    public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger) : base(serviceProvider, logger)
    {
    }
}