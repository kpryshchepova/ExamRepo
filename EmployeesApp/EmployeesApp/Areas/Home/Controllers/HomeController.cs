using Microsoft.AspNetCore.Mvc;

namespace EmployeesApp.Areas.Home.Controllers
{
    [Area("Home")]
    
    public class HomeController : Controller
    {
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

    }
}
