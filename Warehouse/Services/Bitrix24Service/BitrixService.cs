using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// Создаем задачу на основе добавленного Item'a
        /// </summary>
        /// <param name="item">Передаем новый добавленный объект</param>
        public void CreateTask(Item item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Отправляем сформированную задачу на сервер Bitrix24
        /// </summary>
        public void PushTask()
        {
            throw new NotImplementedException();
        }
    }
}
