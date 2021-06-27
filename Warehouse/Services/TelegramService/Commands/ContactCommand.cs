using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Warehouse.Services.TelegramService.Abstractions;

namespace Warehouse.Services.TelegramService
{
    internal class ContactCommand : CommandBase
    {
        public override string Name => "Contact";

        public async override void Execute(Message message, TelegramBotClient client)
        {
            var ChatId = message.Chat.Id;
            var MessageId = message.MessageId;
            var removeKeyboard = new ReplyKeyboardRemove();
            await client.SendTextMessageAsync(ChatId, "Ожидайте подтверждения авторизации от администратора системы", Telegram.Bot.Types.Enums.ParseMode.Default, false, true, replyToMessageId: MessageId, removeKeyboard);
        }
    }
}