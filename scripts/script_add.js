const BASE_URL = (typeof API_BASE !== 'undefined') ? API_BASE : "https://localhost:5001";

const inputFile = document.getElementById("input-file");
const previewBox = document.querySelector(".preview-box");
let coverDataUrl = "";

// Image Preview Logic
inputFile.addEventListener("change", () => {
    const file = inputFile.files[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = () => {
        coverDataUrl = reader.result;
        
        // Update the preview box style
        previewBox.style.backgroundImage = `url('${coverDataUrl}')`;
        previewBox.innerHTML = ""; 
    };
    reader.readAsDataURL(file);
});

document.getElementById("publishBtn").addEventListener("click", async (e) => {
    e.preventDefault();

    
    const title = document.getElementById("title").value.trim();
    const description = document.getElementById("description").value.trim();
    const year = document.getElementById("year").value;
    const runtime = document.getElementById("runtime").value;
    const age = document.getElementById("age").value;
    const rating = document.getElementById("rating").value;
    const director = document.getElementById("director").value.trim();
    const country = document.getElementById("country").value;
    const videoLink = document.getElementById("videoLink").value.trim();

    // Handle Lists
    const castInput = document.getElementById("cast").value;
    const castList = castInput ? castInput.split(",").map(c => c.trim()).filter(c => c !== "") : [];
    const selectedGenres = Array.from(document.getElementById("genres").selectedOptions).map(o => o.value);

    // Validation
    if (!title || !year || !coverDataUrl || selectedGenres.length === 0) {
        alert("Please fill in at least Title, Year, Genres, and upload a Cover image.");
        return;
    }

    const embedLink = toEmbed(videoLink);
    if (videoLink && !embedLink) {
        alert("Invalid YouTube URL.");
        return;
    }

    // Build Object to match C# CreateMovieRequest
    const movieData = {
        title: title,
        description: description,
        yearOfRelease: parseInt(year),
        runTime: runtime ? parseInt(runtime) : 0,
        age: age ? parseInt(age) : 0,
        rating: rating ? parseFloat(rating) : 0,
        cover: coverDataUrl,
        videoLink: embedLink,
        country: country,
        genres: selectedGenres,     
        castList: castList, // Named to match 'request.CastList' in C#
        director: director  // Named to match 'request.Director' in C#
    };

    try {
        const fetchMethod = (typeof fetchWithAuth !== 'undefined') ? fetchWithAuth : fetch;
        
        const response = await fetchMethod(`${BASE_URL}/api/movies`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json" // Crucial for [FromBody] and fixing 415 error
            },
            body: JSON.stringify(movieData)
        });

        if (response.ok) {
            alert("Movie added successfully!");
            window.location.href = "home_page.html";
        } else {
            const errorText = await response.text();
            console.error("API Error:", errorText);
            alert("Failed to add movie: " + errorText);
        }
    } catch (error) {
        console.error("Network Error:", error);
        alert("Check your connection to the server.");
    }
});

// Robust YouTube Embed Converter
function toEmbed(url) {
    if (!url) return "";
    const regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*/;
    const match = url.match(regExp);
    return (match && match[2].length === 11) ? `https://www.youtube.com/embed/${match[2]}` : null;
}