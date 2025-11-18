// Movie Data

const movies = [
  {
    id: crypto.randomUUID(),
    title: "Inception",
    description:
      "A skilled thief is offered a chance to have his past crimes forgiven by implanting another person's idea into a target's subconscious.",
    yearOfRelease: 2010,
    runTime: 148,
    age: 13,
    country: "USA",
    director: "Christopher Nolan",
    cast: ["Leonardo DiCaprio", "Joseph Gordon-Levitt", "Elliot Page"],
    genres: ["Action", "Sci-Fi", "Thriller"],
    rating: 8.8,
    cover:
      "https://filmartgallery.com/cdn/shop/files/Inception-Vintage-Movie-Poster-Original.jpg?v=1738912645",
    videoLink: "https://www.youtube.com/embed/YoHD9XEInc0",
  },
  {
    id: crypto.randomUUID(),
    title: "The Shawshank Redemption",
    description:
      "Two imprisoned men bond over years, finding solace and eventual redemption through acts of common decency.",
    yearOfRelease: 1994,
    runTime: 142,
    age: 16,
    country: "USA",
    director: "Frank Darabont",
    cast: ["Tim Robbins", "Morgan Freeman", "Bob Gunton"],
    genres: ["Drama"],
    rating: 3.3,
    cover:
      "https://image.tmdb.org/t/p/original/5OWFF1DhvYVQiX1yUrBE9CQqO5t.jpg",
    videoLink: "https://www.youtube.com/embed/PLl99DlL6b4",
  },
  {
    id: crypto.randomUUID(),
    title: "Interstellar",
    description:
      "A group of explorers travels through a wormhole in space in an attempt to ensure humanity's survival.",
    yearOfRelease: 2014,
    runTime: 169,
    age: 12,
    country: "USA",
    director: "Christopher Nolan",
    cast: ["Matthew McConaughey", "Anne Hathaway", "Jessica Chastain"],
    genres: ["Adventure", "Drama", "Sci-Fi"],
    rating: 8.6,
    cover:
      "https://m.media-amazon.com/images/I/61ASebTsLpL._AC_UF1000,1000_QL80_.jpg",
    videoLink: "https://www.youtube.com/embed/zSWdZVtXT7E",
  },
  {
    id: crypto.randomUUID(),
    title: "The Dark Knight",
    description:
      "Batman faces the Joker, a criminal mastermind who plunges Gotham City into chaos.",
    yearOfRelease: 2008,
    runTime: 152,
    age: 13,
    country: "USA",
    director: "Christopher Nolan",
    cast: ["Christian Bale", "Heath Ledger", "Aaron Eckhart"],
    genres: ["Action", "Crime", "Drama"],
    rating: 9.0,
    cover: "https://storage.googleapis.com/pod_public/750/257216.jpg",
    videoLink: "https://www.youtube.com/embed/EXeTwQWrcwY",
  },
  {
    id: crypto.randomUUID(),
    title: "Parasite",
    description:
      "Greed and class discrimination threaten the newly formed symbiotic relationship between the wealthy Park family and the destitute Kim clan.",
    yearOfRelease: 2019,
    runTime: 132,
    age: 16,
    country: "South Korea",
    director: "Bong Joon Ho",
    cast: ["Song Kang-ho", "Lee Sun-kyun", "Cho Yeo-jeong"],
    genres: ["Comedy", "Drama", "Thriller"],
    rating: 8.6,
    cover:
      "https://image.tmdb.org/t/p/original/7IiTTgloJzvGI1TAYymCfbfl3vT.jpg",
    videoLink: "https://www.youtube.com/embed/5xH0HfJHsaY",
  },
  {
    id: crypto.randomUUID(),
    title: "Gladiator",
    description:
      "A betrayed Roman general seeks revenge against the corrupt emperor who murdered his family and sent him into slavery.",
    yearOfRelease: 2000,
    runTime: 155,
    age: 16,
    country: "USA",
    director: "Ridley Scott",
    cast: ["Russell Crowe", "Joaquin Phoenix", "Connie Nielsen"],
    genres: ["Action", "Adventure", "Drama"],
    rating: 8.5,
    cover: "https://i.ebayimg.com/images/g/OcEAAOSwHgdmFp-N/s-l1200.jpg",
    videoLink: "https://www.youtube.com/embed/P5ieIbInFpg",
  },
];

const $template = $("#movie-card");
const $template2 = $("#movie-card-small");

const $container = $("#new-releases");
const $container2 = $("#new-releases2");
const $container3 = $("#new-releases3");

// Save and Load Movies

$(document).ready(function () {
  localStorage.setItem("movies", JSON.stringify(movies));
  populate();
});

// Populate UI

function populate() {
  const data = JSON.parse(localStorage.getItem("movies"));

  data.forEach((movie, index) => {
    if (index < 6) {
      $container.append(createClone($template, movie));
      $container3.append(createClone($template2, movie));
    }

    $container2.append(createClone($template2, movie));
  });
}

// Utility: Style Rating Ring

function styleRing(ring, rating) {
  ring.textContent = rating.toFixed(1);
  const r = parseFloat(rating);

  if (r >= 8.5) ring.style.border = "2px solid green";
  else if (r >= 6.5) ring.style.border = "2px solid yellow";
  else if (r >= 4.5) ring.style.border = "2px solid orange";
  else ring.style.border = "2px solid red";
}

// Template Cloning

function createClone(template, data) {
  const clone = template[0].content.cloneNode(true); // FIXED

  clone.querySelector("img").src = data.cover;
  clone.querySelector(".title").textContent = data.title;
  clone.querySelector(".genres").textContent = data.genres.join(", ");
  styleRing(clone.querySelector(".status-ring"), data.rating);

  clone.querySelector("div").addEventListener("click", () => {
    window.location.href = `movie_details_page.html?id=${data.id}`;
  });

  return clone;
}
