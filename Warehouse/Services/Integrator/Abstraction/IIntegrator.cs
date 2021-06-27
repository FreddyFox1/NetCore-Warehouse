using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Model;

namespace Warehouse.Services.Integrator.Abstraction
{
    interface IIntegrator
    {
        /// <summary>
        /// Запрос данных от API в формате json
        /// </summary>
        /// <param name="api">Ссылка на источник</param>
        /// <param name="method">Api метод</param>
        /// <returns></returns>
        string ApiRequest(string api, string method);
    }
}
