namespace FomoCryptoNews.Pooling.Configs;

public class TelegramBotConfig
{
    public string BotToken { get; set; }
    public long AdminGroupId { get; set; }
    public long PublicChannelId { get; set; }
}