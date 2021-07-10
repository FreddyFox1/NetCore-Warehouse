using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Warehouse.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
       
        #warning TODO: Перенести настройки в JSON AppSettings
        public class AuthOptions
        {

            public const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации

            public const string ISSUER = "MyAuthServer"; // издатель токена
            public const string AUDIENCE = "MyAuthClient"; // потребитель токена

            public const int LIFETIME = 1; // время жизни токена - 1 минута
            public static SymmetricSecurityKey GetSymmetricSecurityKey()
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
            }
        }
    }
}
