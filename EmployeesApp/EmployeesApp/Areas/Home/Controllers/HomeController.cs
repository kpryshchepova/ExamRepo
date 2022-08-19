using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesApp.Areas.Home.Controllers;

[Area("Home")]
[AllowAnonymous]
public class HomeController : Controller
{
    [Route("")]
    [Route("Index")]

    public IActionResult Index()
    {
        return View();
    }

}
