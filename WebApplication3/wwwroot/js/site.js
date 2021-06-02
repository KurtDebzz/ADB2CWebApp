var msalInstance;
var request;
var idToken;
var accessToken;
$(document).ready(function () {
    var config = {
        auth: {            
            clientId: 'cd46ab97-3daf-41eb-863e-4d85ffd3f652',
            authority: 'https://kurttesting.b2clogin.com/kurttesting.onmicrosoft.com/B2C_1_signupsignin1'
        }
    };

    msalInstance = new msal.PublicClientApplication(config);
    request = {
        scopes: ["openid", "offline_access", "https://kurttesting.onmicrosoft.com/f475343a-be0e-4d21-920e-507dbff744b3/Files.Read"]
    };

    idToken = '';
    accessToken = '';

    function login() {
        try {
            idToken = msalInstance.loginRedirect(request).then(() => {
                var loggedInAccountName = idToken.idTokenClaims.preferred_username;
                request.account = msalInstance.getAccountByUsername(loggedInAccountName);
            });
            
        } catch (error) {
            console.log(error);
        }
    }

    function getAccessToken() {
        let tokenResponse = '';
        try {
            tokenResponse = msalInstance.acquireTokenSilent(request);
            accessToken = tokenResponse.accessToken;
        }
        catch (error) {
            console.log(error);
            if (requiresInteraction(error)) {
                try {
                    tokenResponse = msalInstance.acquireTokenPopup(request);
                    accessToken = tokenResponse.accessToken;
                }
                catch (error) {
                    console.log(error);
                }
            }
        }
    }

    function getWeatherForecast() {
        getAccessToken();
        const headers = {
            'Accept': 'application/json',
            'Authorization': 'Bearer ' + accessToken
        };
        const weatherForecastUrl = 'https://localhost:44368/weatherforecast';
        const response = fetch(weatherForecastUrl, {
            method: 'GET',
            headers: headers
        });

        const responseData = response.json();

        return responseData;

    }

    function requiresInteraction(errorCode) {
        if (!errorCode || !errorCode.length) {
            return false;
        }
        return errorCode === "consent_required" ||
            errorCode === "interaction_required" ||
            errorCode === "login_required";
    }

    $('#login').click(function () {
        login();
    });

    $('#get').click(function () {
        getWeatherForecast();
    })
});