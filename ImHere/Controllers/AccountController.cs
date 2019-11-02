using ImHere.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ImHere.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public AccountController(SignInManager<IdentityUser> signInManager, ILogger<AccountController> logger, IDataProtectionProvider dataProtectionProvider)
        {
            _signInManager = signInManager;
            _logger = logger;
            _dataProtectionProvider = dataProtectionProvider;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string encodedLoginModel)
        {
            string errorMessage;

            var protector = _dataProtectionProvider.CreateProtector("login");
            var decodedLoginModel = "";
            LoginModel loginModel;

            try
            {
                decodedLoginModel = protector.Unprotect(encodedLoginModel);
            }
            catch (CryptographicException)
            {
                _logger.LogWarning("Invalid login data submitted");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(decodedLoginModel))
            {
                _logger.LogWarning("Invalid login data submitted");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest();
            }

            try
            {
                loginModel = JsonConvert.DeserializeObject<LoginModel>(decodedLoginModel);
            }
            catch
            {
                _logger.LogWarning("Invalid login data submitted");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest();
            }

            if (loginModel.ExpirationUTC < DateTime.UtcNow)
            {
                _logger.LogWarning("Expired login data submitted");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest();
            }

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