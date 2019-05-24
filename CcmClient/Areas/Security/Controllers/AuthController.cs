using CcmClient.Areas.Security.Models.Auth;
using CcmClient.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using __SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

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


            if (result.Succeeded)
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
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return Json(Fail(this.ModelState));

            var result = await this._signInManager
                .PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
                return Json(Success());
            }
            else
            {
                foreach (var error in GetErrorMessages(result))
                    this.ModelState.AddModelError(string.Empty, error);

                return Json(Fail(this.ModelState));
            }
        }

        

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await this._signInManager
                .SignOutAsync();

            return RedirectToAction("Index", "Index", new { area = "" });
        }
        #endregion


        #region PRIVATE METHODS
        private IEnumerable<string> GetErrorMessages(__SignInResult result)
        {
            if (result.IsLockedOut)
                yield return "Account is locked out";

            if (result.IsNotAllowed)
                yield return "Account is not allowed";

            if (result.RequiresTwoFactor)
                yield return "Two factor";

            yield return "Login Failed";
        }

        private object Success()
        {
            return new JsonSuccessModel();
        }

        private object Fail(ModelStateDictionary state)
        {
            var errorMap = new Dictionary<string, IEnumerable<string>>();
            foreach (var error in state.Where(z => z.Value.ValidationState == ModelValidationState.Invalid))
            {
                var key = string.IsNullOrEmpty(error.Key)
                    ? "__root"
                    : error.Key;

                errorMap[key] = error.Value
                    .Errors.Select(y => y.ErrorMessage);
            }

            return new JsonFailModel(errorMap);
        }
        #endregion
    }


    

    public class JsonSuccessModel :
        JsonResultModel
    {
        public override bool IsSuccess => true;
        [JsonProperty("data")]
        public object Data { get; set; }
    }

    public class JsonFailModel :
        JsonResultModel
    {
        #region CONSTRUCTORS
        public JsonFailModel(
            IReadOnlyDictionary<string, IEnumerable<string>> errorMap)
        {
            this.ErrorMap = errorMap;
        }
        #endregion

        #region PUBLIC PROPERTIES
        public override bool IsSuccess => false;

        [JsonProperty("errorMap")]
        public IReadOnlyDictionary<string, IEnumerable<string>> ErrorMap { get; }
        #endregion
    }
}