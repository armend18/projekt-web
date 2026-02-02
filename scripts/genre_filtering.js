if (window.location.pathname.includes("filtered_page.html")) {

    const template2 = document.getElementById("movie-card-small");
    const container2 = document.getElementById("new-releases2");

    const urlParams = new URLSearchParams(window.location.search);
    const genre = urlParams.get("genre");


    if (!genre) {
        alert("No genre provided in URL.");
    } else {
        console.log("Fetching genre:", genre);
        
        (async () => {
            try {
                const response = await fetch(`http://localhost:5000/api/movies/genre/${genre}`);
                const data = await response.json();

       
                const movies = data.items ? data.items : data;

                movies.forEach((movie) => {
                    console.log(movie);
                    if (container2) {
                        container2.appendChild(createClone(template2, movie));
                    }
                });
            } catch (error) {
                console.error("Error fetching genre movies:", error);
            }
        })();
    }
}


function styleRing(ring, rating) {
    if (!ring) return;
    ring.textContent = rating.toFixed(1);
    const r = parseFloat(rating);
    if (r >= 8.5) ring.style.border = "2px solid green";
    else if (r >= 6.5) ring.style.border = "2px solid yellow";
    else if (r >= 4.5) ring.style.border = "2px solid orange";
    else ring.style.border = "2px solid red";
}

function createClone(template, movie) { 
    const clone = template.content.cloneNode(true);
    
   
    const img = clone.querySelector("img");
    if(img) img.src = movie.cover;

    const title = clone.querySelector(".title");
    if(title) title.textContent = movie.title;

    const genres = clone.querySelector(".genres");
    if(genres) genres.textContent = (movie.genres || []).join(", ");

    styleRing(clone.querySelector(".status-ring"), movie.rating);

    const cardDiv = clone.querySelector("div");
    if (cardDiv) {
        cardDiv.addEventListener("click", () => {
            window.location.href = `movie_details_page.html?id=${movie.id}`;
        });
    }
    return clone;
}