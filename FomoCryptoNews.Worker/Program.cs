using System;
using FomoCryptoNews.Api.Service;
using FomoCryptoNews.Database;
using FomoCryptoNews.Parsers.Cryptoslate;
using FomoCryptoNews.UseCase;
using FomoCryptoNews.Worker.Cryptoslate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config => { config.AddJsonFile("appsettings.json").Build(); })
    .ConfigureServices(
        (hostContext, services) =>
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddDbContext<PostgreSqlContext>(opt => opt.UseNpgsql(hostContext.Configuration.GetConnectionString("PostgreSqlConnection")));

            services.AddHttpClient<IBaseParser, BaseParser>();

            services.AddSingleton<ICryptoslateParser, CryptoslateParser>();


            services.AddHostedService<ScrachNewsService>();


            services.AddSingleton<IUseCaseContainer>(sp => Factory.Create(sp.GetRequiredService<ILoggerFactory>(), sp.GetRequiredService<ICryptoslateParser>()));
        }
    )
    .Build();

host.Run();