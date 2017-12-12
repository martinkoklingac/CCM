using CCM.Data.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CCMAdmin.Controllers
{
    public class HomeController : 
        Controller
    {
        [UowTransactionFactory]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
