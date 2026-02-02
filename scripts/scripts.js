
const template = document.getElementById("movie-card");
const template2 = document.getElementById("movie-card-small");

const container = document.getElementById("new-releases");
const container2 = document.getElementById("new-releases2");
const container3 = document.getElementById("new-releases3");



(async () => {

  const response = await fetch("https://localhost:5001/api/movies/cards");
  const data = await response.json();

  data.items.forEach((movie, index) => {
    
    if (index < 6) {
      container.appendChild(createClone(template, movie));
      container3.appendChild(createClone(template2, movie));
    }

    container2.appendChild(createClone(template2, movie));
  });
})();




function styleRing(ring, rating) {
  ring.textContent = rating.toFixed(1);
  const r = parseFloat(rating);
  
  if (r >= 8.5) ring.style.border = "2px solid green";
  else if (r >= 6.5) ring.style.border = "2px solid yellow";
  else if (r >= 4.5) ring.style.border = "2px solid orange";
  else ring.style.border = "2px solid red";
}

function createClone(template, movieData) {
  const clone = template.content.cloneNode(true);

  
  clone.querySelector("img").src = movieData.cover;
  clone.querySelector(".title").textContent = movieData.title;
  clone.querySelector(".genres").textContent = movieData.genres.join(", ");
  
  styleRing(clone.querySelector(".status-ring"), movieData.rating);


  clone.firstElementChild.addEventListener("click", () => {
    window.location.href = `movie_details_page.html?id=${movieData.id}`;
  });

  return clone;
}