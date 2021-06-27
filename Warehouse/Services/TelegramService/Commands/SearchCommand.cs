using Telegram.Bot;
using Telegram.Bot.Types;
using Warehouse.Services.TelegramService.Abstractions;

namespace Warehouse.Services.TelegramService.Commands
{
    public class SearchCommand : CommandBase
    {
        public override string Name => "/search";

        public override void Execute(Message message, TelegramBotClient client)
        {
            throw new System.NotImplementedException();
        }
    }
}
