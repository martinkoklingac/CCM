using Microsoft.AspNetCore.Mvc;

namespace CcmClient.Controllers
{
    public class IndexController :
        Controller
    {
        public IndexController() { }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}
