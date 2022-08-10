using Microsoft.AspNetCore.Mvc;

namespace EmployeesApp.Areas.Error.Controllers;

[Area("Error")]
public class ErrorController : Controller
{
    [Route("NotFound")]
    public IActionResult NotFoundError()
    {
        return View();
    }

    [Route("Error")]
    public IActionResult OtherError()
    {
        return View();
    }
}

