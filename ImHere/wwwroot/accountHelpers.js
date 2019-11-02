window.accountHelpers = {
    login: (loginModel, callbackObject) => {
        var xhr = new XMLHttpRequest();
        xhr.open("POST", '/account/login?encodedLoginModel=' + loginModel, true);

        //Send the proper header information along with the request
        xhr.setRequestHeader("Content-Type", "application/json");

        xhr.onreadystatechange = function () { // Call a function when the state changes.
            if (this.readyState === XMLHttpRequest.DONE) {
                callbackObject.invokeMethodAsync('LoginComplete', this.responseText);
            }
        };

        xhr.send();
    }
};