// Cover Upload Preview
const inputFile = document.getElementById("input-file");
const previewBox = document.querySelector(".preview-box");
let coverDataUrl = "";

inputFile.addEventListener("change", () => {
    const file = inputFile.files[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = () => {
        coverDataUrl = reader.result;
        previewBox.innerHTML = `<img src="${coverDataUrl}" alt="Cover" style="width:100%;">`;
    };
    reader.readAsDataURL(file);
});

// Publish Button Logic
document.getElementById("publishBtn").addEventListener("click", (e) => {
    e.preventDefault();

    // Get form values so we can create form object
    
    const title = document.getElementById("title").value;
    const description = document.getElementById("description").value;
    const yearOfRelease = parseInt(document.getElementById("year").value);
    const runTime = parseInt(document.getElementById("runtime").value);
    const age = parseInt(document.getElementById("age").value);
    const rating = parseFloat(document.getElementById("rating").value) || 0;
    const director = document.getElementById("director").value;
    const cast = document.getElementById("cast").value.split(",").map(a => a.trim());
    const genres = Array.from(document.getElementById("genres").selectedOptions).map(o => o.value);
    const country = document.getElementById("country").value;
    const videoLink = document.getElementById("videoLink").value;
    const type = document.querySelector('input[name="type"]:checked').value;

    // Validate all fields
    if (!title || !description || !yearOfRelease || !runTime || !age || !director || cast.length === 0 || genres.length === 0 || !country || !coverDataUrl || !videoLink) {
        alert("Please fill all fields!");
        return;
    }

    // Create movie object
    const movie = {
        id: crypto.randomUUID(),
        title,
        description,
        yearOfRelease,
        runTime,
        age,
        rating,
        director,
        cast,
        genres,
        country,
        cover: coverDataUrl,
        videoLink,
        type
    };

    const movies = JSON.parse(localStorage.getItem("movies") || "[]");
    movies.push(movie);
    localStorage.setItem("movies", JSON.stringify(movies));

    alert("Movie added successfully!");
     window.location.href = "home_page.html";
});
