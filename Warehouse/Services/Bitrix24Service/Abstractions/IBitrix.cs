using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Model;

namespace Warehouse.Services.Bitrix24Service.Abstractions
{
    /// <summary>
    /// Интерфейс для работы с порталом CRM Bitrix24
    /// </summary>
    public interface IBitrix
    {
        /// <summary>
        /// Генерация запроса для создания задачи
        /// </summary>
        /// <param name="item">Передаем новый добавленный объект</param>
        /// <returns>Возвращает строку запроса</returns>
        string CreateTask(Item item);
        
        /// <summary>
        /// Cоздание задачи на портале Bitrix24
        /// </summary>
        /// <param name="_fields">Передаем новый добавленный объект</param>
        /// <param name="Article"></param>
        /// <returns>В случае успешной генерации возвращает true</returns>
        bool PushTask(string _fields, string Article);

        /// <summary>
        /// Отправка уведомлений пользователям в Bitrix24
        /// </summary>
        /// <param name="BitrixUsers">Список пользователей подписаных на уведомления</param>
        /// <param name="message">Сообщение для пользователей</param>
        void SendNotyfication(string message);
    }
}
