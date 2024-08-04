using System.Threading.Tasks;
using FomoCryptoNews.Database;
using FomoCryptoNews.Database.Cryptoslate;
using FomoCryptoNews.Host.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace FomoCryptoNews.Host.Controllers.Client;

public class NewsController : AbstractController<NewsController>
{
    private readonly ITelegramBotClient _botClient;

    private readonly TelegramBotConfig _telegramBotConfig;


    public NewsController(
        PostgreSqlContext context,
        ILoggerFactory loggerFactory,
        ITelegramBotClient botClient,
        IOptions<TelegramBotConfig> telegramBotConfig
    ) : base(context, loggerFactory)
    {
        _botClient = botClient;
        _telegramBotConfig = telegramBotConfig.Value;
    }


    // [HttpPost]
    // public async Task<IActionResult> CreateModel(string? title, string description, string cover)
    // {
    //     var model = await Db.CryptoslateNewsRepository.CreateModel(title, description, cover);
    //
    //     return SendOk(model);
    // }


    [HttpGet]
    public async Task<IActionResult> GetAllByStatus(Status status)
    {
        var collection = await Db.CryptoslateNewsRepository.ListAllByStatus(status);

        return SendOk(collection);
    }


    [HttpGet]
    public async Task<IActionResult> GetAll(int pageIndex, int pageSize)
    {
        var collection = await Db.CryptoslateNewsRepository.ListAll(pageIndex, pageSize);

        return SendOk(collection);
    }


    [HttpPost]
    public async Task<IActionResult> ApproveNews(int newsId)
    {
        var model = await Db.CryptoslateNewsRepository.GetOne(newsId);

        var preparedMessage = $"<b>{model.Title}</b>\n\n{model.Description}";

        await _botClient.SendTextMessageAsync(_telegramBotConfig.PublicChannelId, preparedMessage, disableWebPagePreview: true, parseMode: ParseMode.Html);

        await Db.CryptoslateNewsRepository.UpdateStatus(model, Status.Approved);

        return Ok();
    }
}