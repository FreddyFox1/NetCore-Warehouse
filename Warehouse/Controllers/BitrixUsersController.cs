using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Model;
using Warehouse.Services.Bitrix24Service;
using Warehouse.Services.Bitrix24Service.Abstractions;

namespace Warehouse.Controllers
{
    [Route("api/RefreshBitrixUsers")]
    [ApiController]
    [Authorize(Roles = "AdminArea")]
    public class BitrixUsersController : Controller
    {
        private readonly ApplicationDbContext db;
        public BitrixUsersController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public IActionResult Refresh()
        {
            return View();
        }

    }
}
