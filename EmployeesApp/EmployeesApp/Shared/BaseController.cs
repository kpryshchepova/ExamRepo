using EmployeesApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesApp.Shared;

public class BaseController : Controller
{
    public async Task<IActionResult> Create<T>(T data, IRepository<T> repository, string redirection)
    {

        try
        {
            await repository.InsertAsync(data);
            await repository.SaveAsync();
            return RedirectToAction(redirection);
        }
        catch
        {
            return RedirectToAction("Error", "Error");
        }

    }


    public async Task<IActionResult> Edit<T>(T data, IRepository<T> repository, string redirection)
    {
        try
        {
            repository.Update(data);
            await repository.SaveAsync();
            return RedirectToAction(redirection);
        }
        catch
        {
            return RedirectToAction("Error", "Error");
        }
    }

}
