using System;
using System.Threading;
using System.Threading.Tasks;
using FomoCryptoNews.Pooling.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FomoCryptoNews.Pooling.Services
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly ILogger<UpdateHandler> _logger;
        private readonly TelegramBotConfig _telegramBotConfig;
        private readonly ITelegramBotClient _botClient;

        public UpdateHandler(
            ILogger<UpdateHandler> logger,
            IOptions<TelegramBotConfig> telegramBotConfig,
            ITelegramBotClient botClient
        )
        {
            _logger = logger;
            _telegramBotConfig = telegramBotConfig.Value;
            _botClient = botClient;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
        {
            if (update.Message != null)
            {
                await HandleIncomingMessage(update.Message, cancellationToken);
            }
            else if (update.CallbackQuery != null)
            {
                await HandleCallbackQuery(update.CallbackQuery, cancellationToken);
            }
        }

        private async Task HandleIncomingMessage(Message message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received a message from {Username} with Id {Id} | Text: {Text}", message.From.Username, message.From.Id, message.Text);

            if (message.Chat.Id == _telegramBotConfig.AdminGroupId)
            {
                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Approve", $"approve:{message.MessageId}"),
                        InlineKeyboardButton.WithCallbackData("Reject", $"reject:{message.MessageId}")
                    }
                });

                await _botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Please review the following message:",
                    replyToMessageId: message.MessageId,
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken
                );
            }
        }

        private async Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            if (callbackQuery.Data != null)
            {
                var parts = callbackQuery.Data.Split(':');
                var action = parts[0];
                var messageId = int.Parse(parts[1]);

                if (action == "approve")
                {
                    await ApproveMessage(callbackQuery.Message, messageId, cancellationToken);
                }
                else if (action == "reject")
                {
                    await RejectMessage(messageId, cancellationToken);
                }

                await _botClient.EditMessageReplyMarkupAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    messageId: callbackQuery.Message.MessageId,
                    replyMarkup: null,
                    cancellationToken: cancellationToken
                );
            }
        }

        private async Task ApproveMessage(Message callbackMessage, int messageId, CancellationToken cancellationToken)
        {
            await _botClient.SendTextMessageAsync(_telegramBotConfig.PublicChannelId, "Work!", cancellationToken: cancellationToken, disableWebPagePreview: true, parseMode: ParseMode.Html);

            if (callbackMessage.ReplyToMessage != null && callbackMessage.ReplyToMessage.MessageId == messageId)
            {
                var originalMessage = callbackMessage.ReplyToMessage;

                if (originalMessage.Photo != null && originalMessage.Photo.Length > 0)
                {
                    // await _botClient.SendPhotoAsync(
                    //     chatId: _telegramBotConfig.PublicChannelId,
                    //     photo: originalMessage.Photo[0].FileId,
                    //     caption: originalMessage.Caption,
                    //     cancellationToken: cancellationToken
                    // );
                }
                else if (!string.IsNullOrEmpty(originalMessage.Text))
                {
                    await _botClient.SendTextMessageAsync(
                        chatId: _telegramBotConfig.PublicChannelId,
                        text: originalMessage.Text,
                        cancellationToken: cancellationToken
                    );
                }

                await _botClient.SendTextMessageAsync(
                    chatId: _telegramBotConfig.AdminGroupId,
                    text: "Message approved and posted in the public group.",
                    cancellationToken: cancellationToken
                );
            }
            else
            {
                _logger.LogWarning("Original message not found or mismatch for messageId: {MessageId}", messageId);
            }
        }

        private async Task RejectMessage(int messageId, CancellationToken cancellationToken)
        {
            await _botClient.DeleteMessageAsync(
                chatId: _telegramBotConfig.AdminGroupId,
                messageId: messageId,
                cancellationToken: cancellationToken
            );

            await _botClient.SendTextMessageAsync(
                chatId: _telegramBotConfig.AdminGroupId,
                text: "Message rejected and deleted.",
                cancellationToken: cancellationToken
            );
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogError("HandleError: {ErrorMessage}", errorMessage);

            if (exception is RequestException)
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
    }
}
