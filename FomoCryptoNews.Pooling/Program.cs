using System;
using FomoCryptoNews.Pooling;
using FomoCryptoNews.Pooling.Configs;
using FomoCryptoNews.Pooling.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(
        config =>
        {
            config.AddJsonFile("appsettings.json")
                .Build();
        })
    .ConfigureServices(
        (hostContext, services) =>
        {
            var telegramConfig = hostContext.Configuration.GetSection("TelegramBot").Get<TelegramBotConfig>();
            if (telegramConfig == null)
            {
                throw new Exception("\n\n -----ERROR ATTENTION! ----- \n Config 'TelegramBot' is null or does not exist. \n\n");
            }
            services.Configure<TelegramBotConfig>(hostContext.Configuration.GetSection("TelegramBot"));

            
            var botClient = new TelegramBotClient(telegramConfig.BotToken);

            services.AddSingleton<ITelegramBotClient>(botClient);

            services.AddScoped<UpdateHandler>();
            services.AddScoped<ReceiverService>();
            services.AddHostedService<PollingService>();

            services.AddHostedService<TestParseWorkerService>();
        })
    .Build();

host.Run();