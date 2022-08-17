using Microsoft.AspNetCore.Mvc;

namespace EmployeesApp.Areas.Login.Controllers
{
    [Area("Login")]
    [Route("Login")]
    public class LoginController : Controller
    {
        Uri baseAdress = new Uri("https://localhost:7216/api");
        private HttpClient client;

        public LoginController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAdress;
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
