using Microsoft.AspNetCore.Mvc;

namespace CCM.Controllers
{
    public class HomeController : 
        Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
