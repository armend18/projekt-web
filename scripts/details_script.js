//get data from the url for the genre and add functions on script to redirect url to the genres_page.html plus genren qeduam
$(document).ready(function () {
  const urlParams = new URLSearchParams(window.location.search);
  const movieId = urlParams.get("id");

  if (!movieId) {
    alert("No movie ID provided in URL.");
    return;
  }

  // FETCH MOVIE DATA
  const data = JSON.parse(localStorage.getItem("movies")) || [];
  let movie = data.find((el) => el.id == movieId);

  if (!movie) {
    alert("Movie not found!");
    return;
  }

  console.log(movie);

  // BANNER TITLE
  $("#MovieTitleBanner").text(movie.title);

  // POSTER
  $(".img-poster").attr("src", movie.cover);

  const tags = $(".description_tag");
  tags.eq(0).text(movie.director);
  tags.eq(1).text(movie.cast.join(", "));
  tags.eq(2).text(movie.genres.join(", "));
  tags.eq(3).text(movie.yearOfRelease);
  tags.eq(4).text(`${movie.runTime} minutes`);
  tags.eq(5).text(movie.country);

  $(".description_area").text(movie.description);

  // RATING RING
  styleRing($(".status-ring"), movie.rating);

  // TRAILER
  $("#trailer").attr("src", movie.videoLink);
});

// STYLE RATING RING
function styleRing($ring, rating) {
  $ring.text(parseFloat(rating).toFixed(1));

  if (rating >= 8.5) $ring.css("border", "2px solid green");
  else if (rating >= 6.5) $ring.css("border", "2px solid yellow");
  else if (rating >= 4.5) $ring.css("border", "2px solid orange");
  else $ring.css("border", "2px solid red");
}
document.getElementById("log-in-home").onclick = function () {
        window.location.href = "signup.html";}