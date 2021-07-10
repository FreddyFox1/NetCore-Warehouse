using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Services.Bitrix24Service;

namespace Warehouse.Controllers
{
    [Route("api/Bitrix")]
    [ApiController]
    [Authorize(Policy ="AdminArea")]
    public class BitrixController : Controller
    {
        private readonly BitrixService _bitrixService;

        public BitrixController(BitrixService bitrixService)
        {
            _bitrixService = bitrixService;
        }

        [HttpGet]
        public async Task<IActionResult> RefreshTable()
        {
            await _bitrixService.GetUsersAsync();
            return RedirectToPage("/Account/Manage/Bitrix", new { area = "Identity" });
        }
    }
}
