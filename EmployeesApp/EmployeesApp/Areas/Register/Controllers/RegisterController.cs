using EmployeesApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace EmployeesApp.Areas.Register.Controllers
{
    [Area("Register")]
    public class RegisterController : Controller
    {
        Uri baseAdress = new Uri("https://localhost:7216/api");
        private HttpClient client;

        public RegisterController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAdress;
        }
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public ActionResult Register(User user)
        {
            string data = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Register", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("", "Login");
            };

            if (Convert.ToInt32(response.StatusCode) == 409) ViewBag.ErrorMessage = "User with this user name is already exists!";

            return View();
        }
    }
}