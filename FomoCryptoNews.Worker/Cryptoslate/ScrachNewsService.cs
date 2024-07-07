using System;
using System.Threading;
using System.Threading.Tasks;
using FomoCryptoNews.Database;
using FomoCryptoNews.UseCase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FomoCryptoNews.Worker.Cryptoslate;

public class ScrachNewsService : BackgroundService
{
    private readonly ILogger<ScrachNewsService> _logger;

    private readonly IServiceProvider _serviceProvider;

    private readonly IUseCaseContainer _useCase;


    public ScrachNewsService(ILogger<ScrachNewsService> logger, IUseCaseContainer useCase, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _useCase = useCase;
        _serviceProvider = serviceProvider;
    }


    async protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PostgreSqlContext>();

            var newsRepo = context.Db.CryptoslateNewsRepository;


            await _useCase.CryptoslateUseCase.ParseNews(newsRepo, stoppingToken);

            _logger.LogInformation("\n\n\t ===== Cryptoslate news parsing completed. Start waiting");
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}