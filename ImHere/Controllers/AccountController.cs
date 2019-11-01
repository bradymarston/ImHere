using ImHere.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<IdentityUser> signInManager, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            string errorMessage;
            
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return Ok();
            }
            if (result.RequiresTwoFactor)
            {
                errorMessage = "Requires two factor authentication (not supported).";
            }
            else
            {
                if (result.IsLockedOut)
                {
                    errorMessage = "User account locked out.";
                }
                else
                {
                    errorMessage = "Invalid login attempt.";
                }
            }

            _logger.LogWarning(errorMessage);
            ModelState.AddModelError(string.Empty, errorMessage);
            return BadRequest();
        }
    }
}
