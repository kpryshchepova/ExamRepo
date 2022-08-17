using EmployeesApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace EmployeesApp.Areas.Login.Controllers
{
    [Area("Login")]
    
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

        [HttpPost("Login")]
        public ActionResult Login(User user)
        {
            string data = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Login", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("EmployeeList", "Employee");
            };

            if (Convert.ToInt32(response.StatusCode) == 400) ViewBag.ErrorMessage = "Wrong password";
            if (Convert.ToInt32(response.StatusCode) == 401) ViewBag.ErrorMessage = "No such user. Please, register first.";

            return View();
        }
    }
}
