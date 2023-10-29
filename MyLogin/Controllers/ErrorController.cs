using Microsoft.AspNetCore.Mvc;

namespace MyLogin.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error404()
        {
            return View();
        }
    }
}
