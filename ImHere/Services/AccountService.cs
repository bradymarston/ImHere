using ImHere.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ImHere.Services
{
    public class AccountService
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IHostEnvironmentAuthenticationStateProvider _hostAuthenticationStateProvider;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private const int loginExpirationSeconds = 5;

        public AccountService(
            IJSRuntime jSRuntime,
            IDataProtectionProvider dataProtectionProvider,
            AuthenticationStateProvider authenticationStateProvider,
            IHostEnvironmentAuthenticationStateProvider hostAuthenticationStateProvider,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _jSRuntime = jSRuntime;
            _dataProtectionProvider = dataProtectionProvider;
            _authenticationStateProvider = authenticationStateProvider;
            _hostAuthenticationStateProvider = hostAuthenticationStateProvider;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Task<SignInResult> LoginAsync(LoginViewModel loginModel)
        {
            var callback = new LoginCallback(loginModel, this);

            loginModel.ExpirationUTC = DateTime.UtcNow + TimeSpan.FromSeconds(loginExpirationSeconds);
            var protector = _dataProtectionProvider.CreateProtector("login");
            var encodedLoginModel = protector.Protect(JsonConvert.SerializeObject(loginModel));
            
            _jSRuntime.InvokeVoidAsync("accountHelpers.login", encodedLoginModel, DotNetObjectReference.Create(callback));

            return callback.ResultSource.Task;
        }

        private async Task FinishLoginAsync(LoginViewModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Email);
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            var identity = new ClaimsIdentity(
                principal.Claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            principal = new ClaimsPrincipal(identity);
            _signInManager.Context.User = principal;
            _hostAuthenticationStateProvider.SetAuthenticationState(Task.FromResult(new AuthenticationState(principal)));
        }

        public class LoginCallback
        {
            private readonly AccountService _accountService;

            public LoginCallback(LoginViewModel loginModel, AccountService accountService)
            {
                LoginModel = loginModel;
                _accountService = accountService;
            }

            public TaskCompletionSource<SignInResult> ResultSource { get; set; } = new TaskCompletionSource<SignInResult>();
            public LoginViewModel LoginModel { get; }

            [JSInvokable]
            public void LoginComplete(string result)
            {

                var signInResult = result switch
                {
                    AccountServiceLoginResults.Succeeded => SignInResult.Success,
                    AccountServiceLoginResults.RequiresTwoFactor => SignInResult.TwoFactorRequired,
                    AccountServiceLoginResults.IsLockedOut => SignInResult.LockedOut,
                    AccountServiceLoginResults.IsNotAllowed => SignInResult.NotAllowed,
                    _ => SignInResult.Failed,
                };

                ResultSource.SetResult(signInResult);
                if (signInResult.Succeeded)
                    _accountService.FinishLoginAsync(LoginModel);
            }
        }
    }

    public static class AccountServiceLoginResults
    {
        public const string Succeeded = "Succeeded";
        public const string Failed = "Failed";
        public const string RequiresTwoFactor = "RequiresTwoFactor";
        public const string IsLockedOut = "IsLockedOut";
        public const string IsNotAllowed = "IsNotAllowed";
    }
}
