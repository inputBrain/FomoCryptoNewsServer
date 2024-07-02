using System.Threading;
using System.Threading.Tasks;
using FomoCryptoNews.Pooling.Configs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace FomoCryptoNews.Pooling;

public class TestParseWorkerService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly TelegramBotConfig _telegramConfig;
    
    public TestParseWorkerService(ITelegramBotClient botClient, IOptions<TelegramBotConfig> telegramConfig)
    {
        _botClient = botClient;
        _telegramConfig = telegramConfig.Value;
    }

    async protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        for (int i = 0; i < 2; i++)
        {
            var preparedMessage = $"Message: {i}";
            
            await _botClient.SendTextMessageAsync(_telegramConfig.AdminGroupId, preparedMessage, cancellationToken: stoppingToken, disableWebPagePreview: true);

            await Task.Delay(2_000, stoppingToken);
        }
    }
}