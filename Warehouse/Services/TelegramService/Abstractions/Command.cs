using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Warehouse.Services.TelegramService.Abstractions
{
    /// <summary>
    /// Абстрактный класс для команд бота
    /// </summary>
    public abstract class CommandBase
    {
        public abstract string Name { get; }
        public abstract void Execute(Message message, TelegramBotClient client);
        public bool Contains(string command)
        {
            return command.Contains(this.Name);
        }
    }
}
