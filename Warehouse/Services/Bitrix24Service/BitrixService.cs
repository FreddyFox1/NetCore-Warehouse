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
    public class BitrixService : IBitrix
    {
        private readonly IOptions<BitrixKeys> BitrixKeys;
        
        public BitrixService(IOptions<BitrixKeys> _BitrixKeys)
        {
            BitrixKeys = _BitrixKeys;
        }

        private static RestClient RC = new RestClient();
        /// <summary>
        /// Создаем задачу на основе добавленного Item'a
        /// </summary>
        /// <param name="item">Передаем новый добавленный объект</param>
        public string CreateTask(Item item)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("fields[TITLE]=" + item.ItemName);
            builder.Append("&fields[RESPONSIBLE_ID]=546");
            builder.Append("&fields[DESCRIPTION]=546\n");
            builder.Append("2 строка\n");
            builder.Append("3 строка\n");
            builder.Append("Артикул: " + item.ItemArticle);
            return builder.ToString();
        }

        /// <summary>
        /// Отправляем запрос для создание задачи на сервер Bitrix24
        /// </summary>
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
    }
}
