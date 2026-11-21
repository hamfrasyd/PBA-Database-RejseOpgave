
let currentUserJwtAccessToken = null;

const authenticationLoginEndpointPath = "/api/auth/login";

// Check whether the request targets our own API.
function isRequestTargetingOurApi(httpRequest) {
    const requestUrl = new URL(httpRequest.url);

    const isRequestFromSameOrigin = requestUrl.origin === window.location.origin;

    const isRequestTargetingApiEndpoint = requestUrl.pathname.startsWith("/api/");

    return isRequestFromSameOrigin && isRequestTargetingApiEndpoint;
}

// Check whether the request is attempting to log in.
function isAuthenticationLoginRequest(httpRequest) {
    const requestUrl = new URL(httpRequest.url);

    const isHttpPostRequest = httpRequest.method.toUpperCase() === "POST";

    const isAuthenticationLoginEndpoint = requestUrl.pathname === authenticationLoginEndpointPath;

    return isRequestTargetingOurApi(httpRequest) && isHttpPostRequest && isAuthenticationLoginEndpoint;
}

export default {

    // // ---------------------------------------------------------------------------------------------------------------
    // // Kevin - Sikkerhedsindstillinger jeg kommer til at bruge i PFA :)
    // agent: { disabled: true },   // Deaktiverer Scalars AI-assistent.
    // showDeveloperTools: "never", // Skjuler udviklerværktøjer, toolbar og visse promoverende elementer i Scalar.
    // telemetry: false,            // Deaktiverer eventuel telemetri.
    // persistAuth: false,          // Undgår at gemme loginoplysninger i localStorage.
    // withDefaultFonts: false,     // Undgår at hente skrifttyper fra eksterne servere. (fx. fonts.scalar.com)
    // // ---------------------------------------------------------------------------------------------------------------

    // Capture and save the JWT after a successful login.
    async onResponseReceived({ request: httpRequest, response: httpResponse }) {
        const isSuccessfulLoginResponse = isAuthenticationLoginRequest(httpRequest) && httpResponse.status === 200;

        if (!isSuccessfulLoginResponse) {
            return;
        }

        try {
            const loginResponseBody = await httpResponse.json();
            const receivedJwtAccessToken = loginResponseBody?.accessToken;

            const isValidJwtAccessToken = typeof receivedJwtAccessToken === "string" && receivedJwtAccessToken.length > 0;

            if (isValidJwtAccessToken) {
                currentUserJwtAccessToken = receivedJwtAccessToken;
            }
        } catch {
            // Ignore login responses that do not contain valid JSON.
        }
    },

    // Automatically attach the saved JWT to API requests.
    onRequestBuilt({ request: httpRequest }) {
        if (!isRequestTargetingOurApi(httpRequest)) {
            return;
        }

        // Clear the previous token when attempting another login.
        if (isAuthenticationLoginRequest(httpRequest)) {
            currentUserJwtAccessToken = null;
            httpRequest.headers.delete("Authorization");
            return;
        }

        // Include the saved JWT in subsequent API requests.
        if (currentUserJwtAccessToken) {
            httpRequest.headers.set("Authorization", `Bearer ${currentUserJwtAccessToken}`);
        }
    }
};
