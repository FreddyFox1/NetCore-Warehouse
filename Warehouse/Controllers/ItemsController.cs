using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Controllers
{
    public class ItemsController : Controller
    {
        [Route("Items/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
