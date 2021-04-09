using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Model;
using Warehouse.Services.Bitrix24Service.BitrixAbstractions;

namespace Warehouse.Services.Bitrix24Service
{
    /// <summary>
    /// Сервис для работы с API Bitrix24
    /// </summary>
    public class BitrixService : IBitrix, IBitrixUser
    {
        private readonly IOptions<BitrixKeys> BitrixKeys;
        private readonly ILogger<BitrixService> logger;
        private readonly static RestClient RC = new RestClient();

        public BitrixService(IOptions<BitrixKeys> _BitrixKeys, ILogger<BitrixService> _logger)
        {
            BitrixKeys = _BitrixKeys;
            logger = _logger;
        }
          
        public string CreateTask(Item item)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append($"fields[TITLE]= Новая позиция/форма для вывода на сайты: {(!String.IsNullOrEmpty(item.ItemName) ? item.ItemName : "")}");
                builder.Append("&fields[RESPONSIBLE_ID]=546");
                builder.Append("&fields[DESCRIPTION]=");
                builder.Append($"Артикул: {(!String.IsNullOrEmpty(item.ItemArticle) ? item.ItemArticle : "")}\n");
                builder.Append($"Дата запуска в работу: {(!String.IsNullOrEmpty(item.DateTransferItem.ToString()) ? item.DateTransferItem : "")}\n");
                builder.Append($"Открытая / закрытая позиция: \n");
                builder.Append($"Название: {(!String.IsNullOrEmpty(item.ItemName) ? item.ItemName : "")}\n");
                builder.Append($"Размер: {(!String.IsNullOrEmpty(item.ItemSizes) ? item.ItemSizes : "")}\n");
                builder.Append("Где формуется: \n");
                builder.Append("Ед.измерения для формы: \n");
                builder.Append("Цена на форму(2, 3, 4 мм):\n");
                builder.Append("Отлить бетонный экземпляр:\n");
                builder.Append("Ед.измерения для бетона:\n");
                builder.Append("Цена на бетонный изделие:\n");
                builder.Append("Назначение:\n");
                builder.Append($"Раздел на сайте(категория):{(!String.IsNullOrEmpty(item.Category.CategoryName) ? item.Category.CategoryName : "")}\n");
                return builder.ToString();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return "";
            }
        }

        public bool PushTask(string _fields, string Article)
        {
            var Request = new RestRequest(BitrixKeys.Value.ReqUrl
                + BitrixKeys.Value.AuthKey
                + "/tasks.task.add.json?" + _fields);

            var Response = RC.Post(Request);
            if (Response.Content.Contains(Article))
                return true;
            else return false;
        }

        public async void SendNotyfication(List<string> BitrixUsers, string message)
        {
            foreach (var user in BitrixUsers)
            {
                var Request = new RestRequest(BitrixKeys.Value.ReqUrl
                + BitrixKeys.Value.AuthKey
                + $"/im.notify.json?message={message}&to{user}");
                await RC.ExecutePostAsync(Request);
            }
        }

        public List<string> GetUsers()
        {
            throw new NotImplementedException();
        }

        public bool isUserCreated()
        {
            return true;
        }

        public void SaveUser()
        {
            throw new NotImplementedException();
        }
    }
}
