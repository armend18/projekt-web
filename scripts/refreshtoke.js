
const API_BASE = "https://localhost:5001"; 

// REFRESH TOKEN LOGIC 
async function refreshAccessToken() {
    const currentToken = localStorage.getItem("jwt_token");
    const refreshToken = localStorage.getItem("refresh_token");

    if (!currentToken || !refreshToken) return false;

    try {
        const response = await fetch(`${API_BASE}/api/authentication/refreshtoken`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                token: currentToken,
                refreshToken: refreshToken
            })
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem("jwt_token", data.token);
            localStorage.setItem("refresh_token", data.refreshToken);
            console.log("Session refreshed!");
            return true;
        }
    } catch (error) {
        console.error("Refresh Error:", error);
    }

    logout();
    return false;
}

// --- 2. LOGOUT ---
function logout() {
    localStorage.removeItem("jwt_token");
    localStorage.removeItem("refresh_token");
    localStorage.removeItem("username");
    window.location.href = "login.html";
}

// SMART FETCH (Global Helper) 
async function fetchWithAuth(url, options = {}) {
    let token = localStorage.getItem("jwt_token");

    if (!options.headers) options.headers = {};
    options.headers["Content-Type"] = "application/json";
    
    if (token) {
        options.headers["Authorization"] = `Bearer ${token}`;
    }

    let response = await fetch(url, options);

    if (response.status === 401) {
        console.warn("Token expired. Refreshing...");
        const success = await refreshAccessToken();

        if (success) {
            token = localStorage.getItem("jwt_token");
            options.headers["Authorization"] = `Bearer ${token}`;
            response = await fetch(url, options); // Retry
        } else {
            alert("Session expired. Please login again.");
            logout();
        }
    }

    return response;
}