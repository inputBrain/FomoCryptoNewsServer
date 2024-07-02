using FomoCryptoNews.Pooling.Abstracts;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace FomoCryptoNews.Pooling.Services;

public class ReceiverService : ReceiverServiceBase<UpdateHandler>
{
    public ReceiverService(ITelegramBotClient botClient, UpdateHandler updateHandler, ILogger<ReceiverServiceBase<UpdateHandler>> logger) : base(botClient, updateHandler, logger)
    {
    }
}
