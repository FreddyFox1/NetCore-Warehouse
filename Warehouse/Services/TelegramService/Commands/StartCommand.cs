using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Warehouse.Services.TelegramService.Commands
{
    public class StartCommand : Command
    {
        public override string Name => "start";

        public async override void Execute(Message message, TelegramBotClient client)
        {
            var ChatId = message.Chat.Id;
            var MessageId = message.MessageId;
            await client.SendTextMessageAsync(ChatId, "Start...", replyToMessageId: MessageId);
        }
    }
}
