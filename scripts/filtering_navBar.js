document.querySelectorAll(".dropdown-item.genres").forEach((item) => {
  item.addEventListener("click", () => {
    window.location.href = `filtered_page.html?genre=${item.id}`;
  });
});
document.getElementById("log-in-home").onclick = function () {
    window.location.href = "index.html";
};

document.getElementById("add_movie_button").onclick = function () {
    window.location.href = "add_movie.html";
};

document.addEventListener("DOMContentLoaded", () => {
    

  const authButton = document.getElementById("log-in-home");
  

  const token = localStorage.getItem("jwt_token");
  const username = localStorage.getItem("username");

  if (token) {
     
      authButton.textContent = "Logout";
      authButton.classList.remove("btn-warning"); 
      authButton.classList.add("btn-outline-warning");

      
      authButton.title = `Logged in as ${username}`;

      // Add Logout Logic
      authButton.onclick = function () {
          // Clear all user data
          localStorage.removeItem("jwt_token");
          localStorage.removeItem("refresh_token");
          localStorage.removeItem("username");
          localStorage.removeItem("user_email");

          // Redirect to Login Page
          window.location.href = "index.html"; 
      };
      
      // Show "Add Movie" button only if logged in (Optional)
      const addMovieBtn = document.getElementById("add_movie_button");
      if(addMovieBtn) addMovieBtn.style.display = "block";

  } else {
    
      
      authButton.textContent = "Login";
      
      // Login Logic
      authButton.onclick = function () {
          window.location.href = "index.html"; 
      };

      // Hide "Add Movie" button 
      const addMovieBtn = document.getElementById("add_movie_button");
      if(addMovieBtn) addMovieBtn.style.display = "none";
  }
});