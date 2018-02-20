using Microsoft.AspNetCore.Mvc;
using System;

namespace CCM.Areas.Public.Controllers
{
    [Area("Public")]
    [Route("public/auth")]
    public class AuthController :
        Controller
    {
        #region CONSTRUCTORS
        public AuthController()
        {

        }
        #endregion

        #region ACTIONS
        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public void Register(object request)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public void Logout()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}