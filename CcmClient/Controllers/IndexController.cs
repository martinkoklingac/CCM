using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CcmClient.Controllers
{
    public class IndexController :
        Controller
    {
        private readonly UserManager<CcmUser> _userManager;

        public IndexController(
            UserManager<CcmUser> userManager)
        {
            this._userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var x = this.User.Identity.IsAuthenticated;

            return View();
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Test()
        {
            var user = await this._userManager
                .GetUserAsync(this.HttpContext.User);

            return Content($"=> TEST - UserName : {user.UserName} / FirstName : {user.FirstName}");
        }
    }
}
