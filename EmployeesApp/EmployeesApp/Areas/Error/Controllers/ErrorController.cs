using Microsoft.AspNetCore.Mvc;

namespace EmployeesApp.Areas.Error.Controllers;

[Area("Error")]
[Route("Error")]
public class ErrorController : Controller
{
    [Route("NotFound")]
    public IActionResult NotFoundError()
    {
        return View();
    }

    [Route("Unauthorised")]
    public IActionResult Unauthorised()
    {
        return View();
    }

    [Route("Error")]
    public IActionResult Error()
    {
        return View();
    }
}

