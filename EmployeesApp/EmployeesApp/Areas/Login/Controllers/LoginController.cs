using EmployeesApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace EmployeesApp.Areas.Login.Controllers;

[Area("Login")]
[AllowAnonymous]
public class LoginController : Controller
{
    Uri baseAdress = new Uri("https://localhost:7216/api");
    private HttpClient client;

    public LoginController()
    {
        client = new HttpClient();
        client.BaseAddress = baseAdress;
    }

    [Route("Login")]
    public IActionResult Login()
    {
        return View();
    }

    [Route("LoginUser")]
    public JsonResult LoginUser([FromBody] UserAuthModel user)
    {
        string data = JsonConvert.SerializeObject(user);
        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
        HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Login", content).Result;
        if (response.IsSuccessStatusCode)
        {
            var token = response.Content.ReadAsStringAsync().Result;
            HttpContext.Session.SetString("token", token);
            return Json(token);
        };

        return Json(null);
    }

    [Route("Logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("token");
        return View("Login");
    }
}
