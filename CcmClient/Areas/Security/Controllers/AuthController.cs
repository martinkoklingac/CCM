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
        private readonly UserManager<CcmUser> _userManager;
        #endregion

        #region CONTROLLERS
        public AuthController(
            SignInManager<CcmUser> signInManager,
            UserManager<CcmUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
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

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View(new RegistrationModel());
        }


        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new CcmUser
            {
                UserName = model.UserName,
                Password = model.Password
            };


            var result = await this._userManager
                .CreateAsync(user, user.Password);


            if(result.Succeeded)
            {
                await this._signInManager.SignInAsync(user, true);

                return RedirectToAction("Test", "Index", new { area = "" });
            }
            else
            {
                return View(model);
            }
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
                return RedirectToAction("Test", "Index", new { area = "" });
            }
            else
            {
                return View(model);
            }
        }
        #endregion
    }
}