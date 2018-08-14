using CcmClient.Areas.Security.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CcmClient.Areas.Security.Controllers
{
    [Area("security")]
    [Route("security/auth")]
    public class AuthController :
        Controller
    {
        #region PRIVATE FIELDS
        private readonly SignInManager<CcmUser> _signInManager;
        #endregion

        #region CONTROLLERS
        public AuthController(SignInManager<CcmUser> signInManager)
        {
            this._signInManager = signInManager;
        }
        #endregion

        #region ACTIONS
        /// <summary>
        /// Locin action
        /// </summary>
        /// <example>/security/auth/login</example>
        /// <returns></returns>
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await this._signInManager
                .PasswordSignInAsync(model.UserName, model.Password, false, false);

            if(result.Succeeded)
            {

            }
            else
            {

            }

            return View(null);
        }
        #endregion
    }
}