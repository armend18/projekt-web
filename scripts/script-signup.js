document.getElementById('show-signup').addEventListener('click', (e) => {
    e.preventDefault();
    document.getElementById('login-container').classList.add('hidden');
    document.getElementById('signup-container').classList.remove('hidden');
});

document.getElementById('show-login').addEventListener('click', (e) => {
    e.preventDefault();
    document.getElementById('signup-container').classList.add('hidden');
    document.getElementById('login-container').classList.remove('hidden');
});


// Email validation function
function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

// Password validation function
function validatePassword(password) {
    return password.length >= 6;
}

// Show error with shake animation
function showError(inputId, errorId) {
    const input = document.getElementById(inputId);
    const error = document.getElementById(errorId);
    
    input.classList.add('error');
    input.classList.add('shake');
    error.style.display = 'block';
    
    setTimeout(() => {
        input.classList.remove('shake');
    }, 500);
}

// Hide error
function hideError(inputId, errorId) {
    const input = document.getElementById(inputId);
    const error = document.getElementById(errorId);
    
    input.classList.remove('error');
    error.style.display = 'none';
}


// ==========================
// LOCAL STORAGE FUNCTIONS
// ==========================

// Get users list from localStorage
function getUsers() {
    return JSON.parse(localStorage.getItem("users")) || [];
}

// Save users list to localStorage
function saveUsers(users) {
    localStorage.setItem("users", JSON.stringify(users));
}

// Save logged in user
function setLoggedInUser(user) {
    localStorage.setItem("loggedInUser", JSON.stringify(user));
}

// Get logged in user
function getLoggedInUser() {
    return JSON.parse(localStorage.getItem("loggedInUser"));
}


// ==========================
// PASSWORD STRENGTH
// ==========================
function calculatePasswordStrength(password) {
    let strength = 0;
    
    if (password.length >= 6) strength += 25;
    if (password.length >= 8) strength += 15;
    if (/[a-z]/.test(password)) strength += 15;
    if (/[A-Z]/.test(password)) strength += 15;
    if (/[0-9]/.test(password)) strength += 15;
    if (/[^a-zA-Z0-9]/.test(password)) strength += 15;
    
    return Math.min(strength, 100);
}

function updatePasswordStrength(password) {
    const strength = calculatePasswordStrength(password);
    const strengthBar = document.getElementById('signup-password-strength-bar');
    const strengthText = document.getElementById('signup-password-strength-text');
    
    strengthBar.style.width = `${strength}%`;
    
    if (strength < 40) {
        strengthBar.style.backgroundColor = 'var(--weak)';
        strengthText.textContent = 'Weak password';
        strengthText.style.color = 'var(--weak)';
    } else if (strength < 70) {
        strengthBar.style.backgroundColor = 'var(--medium)';
        strengthText.textContent = 'Medium password';
        strengthText.style.color = 'var(--medium)';
    } else {
        strengthBar.style.backgroundColor = 'var(--strong)';
        strengthText.textContent = 'Strong password';
        strengthText.style.color = 'var(--strong)';
    }
}


// ==========================
// PASSWORD TOGGLE
// ==========================
function setupPasswordToggle(passwordId, toggleId) {
    const passwordInput = document.getElementById(passwordId);
    const toggleButton = document.getElementById(toggleId);
    
    toggleButton.addEventListener('click', () => {
        const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
        passwordInput.setAttribute('type', type);
        
        toggleButton.innerHTML = type === 'password' ? 
            '<i class="fas fa-eye"></i>' : 
            '<i class="fas fa-eye-slash"></i>';
    });
}

setupPasswordToggle('login-password', 'login-password-toggle');
setupPasswordToggle('signup-password', 'signup-password-toggle');
setupPasswordToggle('signup-confirm', 'signup-confirm-toggle');


// ==========================
// LOGIN FORM
// ==========================
document.getElementById('login-email').addEventListener('blur', function() {
    if (!validateEmail(this.value) && this.value !== '') {
        showError('login-email', 'login-email-error');
    } else {
        hideError('login-email', 'login-email-error');
    }
});

document.getElementById('login-password').addEventListener('blur', function() {
    if (!validatePassword(this.value) && this.value !== '') {
        showError('login-password', 'login-password-error');
    } else {
        hideError('login-password', 'login-password-error');
    }
});

document.getElementById('login-form').addEventListener('submit', (e) => {
    e.preventDefault();
    
    const email = document.getElementById('login-email').value;
    const password = document.getElementById('login-password').value;

    let isValid = true;

    if (!validateEmail(email)) {
        showError('login-email', 'login-email-error');
        isValid = false;
    }

    if (!validatePassword(password)) {
        showError('login-password', 'login-password-error');
        isValid = false;
    }

    if (!isValid) return;

    // Get users from storage
    const users = getUsers();

    const user = users.find(u => u.email === email && u.password === password);

    if (!user) {
        alert("Wrong email or password");
        return;
    }

    setLoggedInUser(user);

    window.location.href="home_page.html"
});


// ==========================
// SIGNUP FORM
// ==========================
document.getElementById('signup-name').addEventListener('blur', function() {
    if (this.value.trim() === '') {
        showError('signup-name', 'signup-name-error');
    } else {
        hideError('signup-name', 'signup-name-error');
    }
});

document.getElementById('signup-email').addEventListener('blur', function() {
    if (!validateEmail(this.value) && this.value !== '') {
        showError('signup-email', 'signup-email-error');
    } else {
        hideError('signup-email', 'signup-email-error');
    }
});

document.getElementById('signup-password').addEventListener('input', function() {
    updatePasswordStrength(this.value);
    
    if (!validatePassword(this.value) && this.value !== '') {
        showError('signup-password', 'signup-password-error');
    } else {
        hideError('signup-password', 'signup-password-error');
    }
});

document.getElementById('signup-confirm').addEventListener('blur', function() {
    const password = document.getElementById('signup-password').value;
    if (this.value !== password && this.value !== '') {
        showError('signup-confirm', 'signup-confirm-error');
    } else {
        hideError('signup-confirm', 'signup-confirm-error');
    }
});


document.getElementById('signup-form').addEventListener('submit', (e) => {
    e.preventDefault();
    
    const name = document.getElementById('signup-name').value;
    const email = document.getElementById('signup-email').value;
    const password = document.getElementById('signup-password').value;
    const confirmPassword = document.getElementById('signup-confirm').value;

    let isValid = true;

    if (name.trim() === '') {
        showError('signup-name', 'signup-name-error');
        isValid = false;
    }

    if (!validateEmail(email)) {
        showError('signup-email', 'signup-email-error');
        isValid = false;
    }

    if (!validatePassword(password)) {
        showError('signup-password', 'signup-password-error');
        isValid = false;
    }

    if (password !== confirmPassword) {
        showError('signup-confirm', 'signup-confirm-error');
        isValid = false;
    }

    if (!isValid) return;

    // Load existing users
    let users = getUsers();

    // Check email already exists
    if (users.some(u => u.email === email)) {
        alert("Email already registered!");
        return;
    }

    // Create new user
    const newUser = {
        name,
        email,
        password
    };

    users.push(newUser);

    saveUsers(users);

    alert("Account created! You can now log in.");

    document.getElementById('signup-container').classList.add('hidden');
    document.getElementById('login-container').classList.remove('hidden');
});
// ===============================
// GOOGLE LOGIN + SIGN UP SUPPORT
// ===============================

// Google Initialization
window.onload = function() {
    google.accounts.id.initialize({
        client_id: "305774601778-d05cbdn68knakviat0vhi9bjir6pk0i7.apps.googleusercontent.com", // <-- Replace this!
        callback: handleGoogleResponse
    });

    google.accounts.id.renderButton(
        document.getElementById("google-btn"),
        { theme: "outline", size: "extra-large", width: "500", }
    );
    google.accounts.id.renderButton(
        document.getElementById("google-signup-btn"),
        { theme: "outline", size: "extra-large", width: "500", }
    );
};


// When Google sends the login data:
function handleGoogleResponse(response) {
    // Decode JWT token
    const data = parseJwt(response.credential);

    const name = data.name;
    const email = data.email;

    // Save google user into localStorage users list
    let users = getUsers();

    // If user does NOT exist → create new localStorage account
    let user = users.find(u => u.email === email);

    if (!user) {
        user = {
            name,
            email,
            password: null,        // ← Google accounts have no password
            googleAccount: true
        };

        users.push(user);
        saveUsers(users);
    }

    // Log user in
    setLoggedInUser(user);

   window.location.href="home_page.html"
}


// Decode Google JWT
function parseJwt(token) {
    try {
        return JSON.parse(atob(token.split('.')[1]));
    } catch (e) {
        return {};
    }
}
