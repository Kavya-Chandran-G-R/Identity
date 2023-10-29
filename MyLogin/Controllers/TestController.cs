using Microsoft.AspNetCore.Mvc;

namespace MyLogin.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Test()
        {
            return View();
        }
    }
}
