using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Model;

namespace Warehouse.Controllers
{
    [Route("api/Logs")]
    [ApiController]
    [Authorize]
    public class LogsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LogsController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Получение записей Eventlogs
        /// </summary>
        /// <returns>Возвращает список всез перемещений Item'ов</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new
            {
                data = await _db.ItemLogs.ToListAsync()
            });
        }

        [HttpGet("sorted")]
        public async Task<IActionResult> GetSortLogs()
        {
            var Dates = await _db.ItemLogs
                .Select(x => x.LogDateTransfer.Date)
                .Distinct()
                .ToListAsync();

            LogsOfLogsModel LogsList  =  new LogsOfLogsModel();
            LogsModel LogsArr = new LogsModel();

            foreach (var date in Dates)
            {
                var Data = await _db.ItemLogs
                    .Where(x => x.LogDateTransfer.Date == date)
                    .ToArrayAsync();

                LogsList.Logs = LogsArr;
            }

            return Json(new
            {
                data = LogsList
            });
        }

        public class LogsOfLogsModel
        {
            public LogsModel Logs { get; set; }
        }

        public class LogsModel
        {
            public ItemLog[] Logs { get; set; }
        }
    }
}