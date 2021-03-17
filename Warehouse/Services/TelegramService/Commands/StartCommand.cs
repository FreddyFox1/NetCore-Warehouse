using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Warehouse.Services.TelegramService.TelegramAbstraction;

namespace Warehouse.Services.TelegramService.Commands
{
    public class StartCommand : Command
    {
        public override string Name => "start";

        /// <summary>
        ///     Выполнение комады Start
        /// </summary>
        /// <param name="message">Сообщение из чата Telegram</param>
        /// <param name="client">Клиент для отправи ответа в Telegram</param>
        public async override void Execute(Message message, TelegramBotClient client)
        {
            var ChatId = message.Chat.Id;
            var MessageId = message.MessageId;

            var ButtonsMenu = new ReplyKeyboardMarkup();

            ButtonsMenu.Keyboard = new KeyboardButton[][]{
                new KeyboardButton[]{
                    new KeyboardButton("Log in")
                },
            };

            ButtonsMenu.ResizeKeyboard = true;
            await client.SendTextMessageAsync(ChatId, "Введите логин и пароль через пробел!", Telegram.Bot.Types.Enums.ParseMode.Default, false, true, replyToMessageId: MessageId, ButtonsMenu);
        }
    }
}
