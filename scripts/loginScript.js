
const API_BASE = "https://localhost:5001";

$(document).ready(function() {

    // --- 1. AUTO-REDIRECT CHECK ---
    // If the user already has tokens, send them to the Home Page immediately.
    const existingToken = localStorage.getItem("jwt_token");
    const existingRefresh = localStorage.getItem("refresh_token");

    if (existingToken && existingRefresh) {
        console.log("Active session found. Redirecting to Home...");
        window.location.href = "home_page.html";
        return; // Stop the rest of the script
    }

    // --- 2. LOGIN BUTTON LOGIC ---
    $("#loginButton").click(function() {
        const loginData = {
            email: $("#loginEmail").val(),
            password: $("#loginPassword").val()
        };

        if (!loginData.email || !loginData.password) {
            alert("Please enter both email and password.");
            return;
        }

        $.ajax({
            url: `${API_BASE}/api/authentication/login`,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(loginData),
           // Inside your $.ajax success function:
success: function(response) {
    console.log("Login Successful", response);

    // 1. Save the Raw Tokens
    localStorage.setItem("jwt_token", response.token);
    localStorage.setItem("refresh_token", response.refreshToken);

    // 2. Decode the Token to get User Info
    const decodedToken = parseJwt(response.token);
    console.log("Decoded Token Data:", decodedToken); // <-- CHECK CONSOLE FOR THIS!

    // 3. Extract Data (Handle .NET's long key names)
    // Try to get the name from the long .NET key, or fallback to 'unique_name', or 'sub'
    const serverUsername = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] || 
                           decodedToken["unique_name"] || 
                           decodedToken.sub;
    
    const serverEmail = decodedToken.email;

    // 4. Save User Details for UI usage
    // Use the data from the TOKEN, not the Input field (it's more trustworthy)
    localStorage.setItem("username", serverUsername || "User");
    localStorage.setItem("user_email", serverEmail);

    // Redirect
    window.location.href = "home_page.html";
},
            error: function(xhr) {
                console.error("Login Failed", xhr);
                alert("Login failed: " + (xhr.responseText || "Invalid credentials"));
            }
        });
    });

    // --- 3. SIGNUP BUTTON LOGIC ---
    $("#signupButton").click(function(e) {
        e.preventDefault();

        const registerData = {
            email: $("#signupEmail").val(),
            name: $("#signupName").val(),
            lastName: $("#signupLastname").val(),
            username: $("#signupUsername").val(),
            password: $("#signupPassword").val(),
        };

        // Simple validation
        if (!registerData.email || !registerData.password || !registerData.username || !registerData.name || !registerData.lastName) {
            alert("Please fill in all required fields.");
            return;
        }

        $.ajax({
            url: `${API_BASE}/api/authentication/register`,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(registerData),
            success: function(response) {
                console.log("Registration Successful", response);
                alert("Account created successfully! Please login.");
                toggleForms(); // Switch back to login view
            },
            error: function(xhr) {
                console.error("Registration Failed", xhr);

                let errorMessage = "Registration failed.";
                if (xhr.responseJSON && xhr.responseJSON.errors) {
                    if (Array.isArray(xhr.responseJSON.errors)) {
                        errorMessage += "\n" + xhr.responseJSON.errors.join("\n");
                    } else {
                        errorMessage += "\n" + JSON.stringify(xhr.responseJSON.errors);
                    }
                } else if (xhr.responseText) {
                    errorMessage = xhr.responseText;
                }

                alert(errorMessage);
            }
        });
    });
});


function toggleForms() {
    const loginBox = document.getElementById('login-box');
    const signupBox = document.getElementById('signup-box');

    if (loginBox.classList.contains('d-none')) {
        loginBox.classList.remove('d-none');
        signupBox.classList.add('d-none');
    } else {
        loginBox.classList.add('d-none');
        signupBox.classList.remove('d-none');
    }
}


// Helper: Decodes the JWT string into a JSON object
function parseJwt(token) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        return JSON.parse(jsonPayload);
    } catch (e) {
        console.error("Error parsing JWT", e);
        return null;
    }
}