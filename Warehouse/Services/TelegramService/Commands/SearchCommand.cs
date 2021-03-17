using Telegram.Bot;
using Telegram.Bot.Types;
using Warehouse.Services.TelegramService.TelegramAbstraction;

namespace Warehouse.Services.TelegramService.Commands
{
    public class SearchCommand : Command
    {
        public override string Name => "/search";

        public override void Execute(Message message, TelegramBotClient client)
        {
            throw new System.NotImplementedException();
        }
    }
}
