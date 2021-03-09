﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Warehouse.Services.TelegramService.TelegramAbstraction
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract void Execute(Message message, TelegramBotClient client);
        public bool Contains(string command)
        {
            return command.Contains(this.Name);
        }
    }
}