using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Args;
using Microsoft.Extensions.Options;
using Warehouse.Services.TelegramService.Abstractions;
using Warehouse.Services.TelegramService.Commands;
using Warehouse.Model;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Services.TelegramService
{
    internal class TelegramService : IHostedService, ITelegram, ITelegramDb
    {
        private static ILogger<TelegramService> logger;
        private Timer timer;
        private static TelegramBotClient telegramClient;
        private readonly IOptions<TelegramKey> telegramKey;
        private static List<CommandBase> CommandsList;
        private readonly IServiceScopeFactory scopeFactory;

        public TelegramService(ILogger<TelegramService> _logger,
                               IOptions<TelegramKey> _telegramKey,
                               IServiceScopeFactory scopeFactory)
        {
            logger = _logger;
            telegramKey = _telegramKey;
            telegramClient = new TelegramBotClient(telegramKey.Value.AuthKey);
            this.scopeFactory = scopeFactory;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            CreateCommandList();

            logger.LogInformation("Telegram service started");
            telegramClient.OnUpdate += OnUpdateReceived;

            timer = new Timer(a =>
            {
                GetUpdates();
            },
            null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Telegram service was stoped");
            return Task.CompletedTask;
        }

        public void GetUpdates()
        {
            telegramClient.StartReceiving(Array.Empty<UpdateType>());
        }

        private async void OnUpdateReceived(object sender, UpdateEventArgs e)
        {
            var message = e.Update.Message;
            if (message != null)
            {
                if (message.Type == MessageType.Text)
                {
                    foreach (var command in CommandsList)
                    {
                        if (command.Contains(message.Text))
                        {
                            command.Execute(message, telegramClient);
                            break;
                        }
                    }
                }

                if (message.Type == MessageType.Contact)
                {
                    foreach (var command in CommandsList)
                    {
                        if (command.Contains("Contact"))
                        {
                            command.Execute(message, telegramClient);

                            TelegramEntity user = new TelegramEntity()
                            {
                                ChatID = message.Chat.Id,
                                FirstName = message.Contact.FirstName,
                                LastName = message.Contact.LastName,
                                PhoneNumber = message.Contact.PhoneNumber
                            };

                            await SaveUser(user);
                            break;
                        }
                    }
                }
                else return;
            }
        }

        private void CreateCommandList()
        {
            CommandsList = new List<CommandBase>();
            CommandsList.Add(new StartCommand());
            CommandsList.Add(new SearchCommand());
            CommandsList.Add(new ContactCommand());
        }

        public async Task SaveUser(TelegramEntity entity)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                
                bool isExist = await _db.TelegramEntities
                    .Where(x => x.PhoneNumber == entity.PhoneNumber)
                    .AnyAsync();
                
                if (!isExist)
                {
                    _db.Add(entity);
                    await _db.SaveChangesAsync();
                    logger.LogInformation($"Добавлен новый пользователь с номером {entity.PhoneNumber}");
                }
                else logger.LogError($"Пользователь с номером {entity.PhoneNumber} уже существует");
            }
        }

        public void DeleteUser()
        {
            throw new NotImplementedException();
        }
        public void SendNotification()
        {
            throw new NotImplementedException();
        }
    }
}
