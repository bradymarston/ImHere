using ImHere.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services
{
    public class AccountService
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly IDataProtectionProvider _dataProtectionProvider;

        private const int loginExpirationSeconds = 5;

        public AccountService(IJSRuntime jSRuntime, IDataProtectionProvider dataProtectionProvider)
        {
            _jSRuntime = jSRuntime;
            _dataProtectionProvider = dataProtectionProvider;
        }

        public Task<bool> LoginAsync(LoginModel loginModel)
        {
            var callback = new LoginCallback();

            loginModel.ExpirationUTC = DateTime.UtcNow + TimeSpan.FromSeconds(loginExpirationSeconds);
            var protector = _dataProtectionProvider.CreateProtector("login");
            var encodedLoginModel = protector.Protect(JsonConvert.SerializeObject(loginModel));
            
            _jSRuntime.InvokeVoidAsync("accountHelpers.login", encodedLoginModel, DotNetObjectReference.Create(callback));

            return callback.ResultSource.Task;
        }
    }

    public class LoginCallback
    {
        public TaskCompletionSource<bool> ResultSource { get; set; } = new TaskCompletionSource<bool>();

        [JSInvokable]
        public void LoginComplete(bool result)
        {
            ResultSource.SetResult(result);
        }
    }
}
