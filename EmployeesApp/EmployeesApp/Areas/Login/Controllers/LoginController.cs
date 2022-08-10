using Microsoft.AspNetCore.Mvc;

namespace EmployeesApp.Areas.Login.Controllers
{
    [Area("Login")]
    [Route("Login")]
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
