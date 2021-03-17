using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }
}