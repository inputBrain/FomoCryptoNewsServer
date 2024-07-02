using FomoCryptoNews.Api.Service;
using FomoCryptoNews.Database;
using FomoCryptoNews.Host.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Telegram.Bot;

namespace FomoCryptoNews.Host;

public class Startup
{
    public IConfiguration Configuration { get; }
    
    private readonly ILoggerFactory _loggerFactory;
    
    
    public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        Configuration = configuration;
        _loggerFactory = loggerFactory;
    }
    

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<TelegramBotConfig>(Configuration.GetSection("TelegramBot"));
        
        var telegramConfig = Configuration.GetSection("TelegramBot").Get<TelegramBotConfig>();

        var botClient = new TelegramBotClient(telegramConfig.BotToken);

        services.AddSingleton<ITelegramBotClient>(botClient);
        
        services.AddControllers();
        
        
        var typeOfContent = typeof(Startup);
        services.AddDbContext<PostgreSqlContext>(
            opt => opt.UseNpgsql(
                Configuration.GetConnectionString("PostgreSqlConnection"),
                b => b.MigrationsAssembly(typeOfContent.Assembly.GetName().Name)
            )
        );
        
        services.AddHttpClient<IBaseParser, BaseParser>();
        
        services.AddScoped<IDatabaseFacade, DatabaseFacade>();

        ConfigureSwagger(services);
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApplianceX.Host v1"));
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    public void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(
            c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ApplianceX.Host", Version = "v1"});
            }
        );
    }
}