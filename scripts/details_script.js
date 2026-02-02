

document.addEventListener("DOMContentLoaded", async () => {
    const urlParams = new URLSearchParams(window.location.search);
    const movieId = urlParams.get("id");

    if (!movieId) {
        alert("No movie ID provided in URL.");
        return;
    }

    await loadMovieDetails(movieId);

 
    await loadComments(movieId);


    const submitBtn = document.getElementById("submit-comment");
    submitBtn.addEventListener("click", () => handleMainPost(movieId));
    
});


async function loadMovieDetails(movieId) {
    try {
        const response = await fetch(`${API_BASE}/api/movies/${movieId}`);
        if (!response.ok) throw new Error(`API Error: ${response.status}`);
        const data = await response.json();

        // Populate UI
        const title = document.getElementById("MovieTitleBanner");
        if(title) title.textContent = data.title;
        
        const poster = document.querySelector(".img-poster");
        if (poster) poster.src = data.cover || "assets/placeholder.jpg";

        const desc = document.querySelector(".description_area");
        if (desc) desc.textContent = data.description || "No description.";

        const trailer = document.getElementById("trailer");
        if (trailer) trailer.src = data.videoLink || "";

        const tags = document.querySelectorAll(".description_tag");
        if (tags.length >= 6) {
            tags[0].textContent = (data.directors || []).join(", ");
            tags[1].textContent = (data.cast || []).join(", ");
            tags[2].textContent = (data.genres || []).join(", ");
            tags[3].textContent = data.yearOfRelease || "N/A";
            tags[4].textContent = data.runTime ? `${data.runTime} min` : "N/A";
            tags[5].textContent = data.country || "N/A";
        }
        styleRing(document.querySelector(".status-ring"), data.rating);

    } catch (error) {
        console.error("Error loading movie:", error);
    }
}

async function loadComments(movieId) {
    const list = document.getElementById("comments-list");
    if(!list) return;

    try {
        const response = await fetch(`${API_BASE}/api/movies/${movieId}/comments`);
        if (!response.ok) throw new Error("Failed to fetch comments");

        const comments = await response.json();
        list.innerHTML = ""; 

        if (comments.length === 0) {
            list.innerHTML = "<p class='text-secondary'>No comments yet. Be the first!</p>";
            return;
        }

        
        comments.forEach(c => {
            list.appendChild(createCommentElement(c, movieId));
        });

    } catch (error) {
        console.error("Comment Error:", error);
        list.innerHTML = "<p class='text-danger'>Could not load comments.</p>";
    }
}



function createCommentElement(comment, movieId) {
    const container = document.createElement("div");
    container.classList.add("mb-4");

    const currentUser = localStorage.getItem("username");
    const isOwner = currentUser && (comment.username === currentUser);
    const date = new Date(comment.createdAt).toLocaleDateString();

    const content = comment.isDeleted 
        ? `<span class="text-muted fst-italic">This comment was deleted.</span>`
        : comment.text;


    container.innerHTML = `
      <div class="d-flex align-items-start text-white">
          <img src="assets/user_icon.png" class="rounded-circle me-3" width="40" height="40" alt="User">
          <div class="w-100">
              <div class="d-flex justify-content-between align-items-center">
                  <h6 class="mb-0 fw-bold text-warning">${comment.username || "User"}</h6>
                  <small class="text-muted">${date}</small>
              </div>
              <p class="mt-1 mb-1 text-break" style="font-size: 0.95rem;">${content}</p>
              
              ${!comment.isDeleted ? `
              <div class="d-flex gap-3 mt-2">
                  <button class="btn btn-sm btn-link text-decoration-none p-0 text-secondary btn-reply">Reply</button>
                  <button class="btn btn-sm btn-link text-decoration-none p-0 text-secondary btn-like">👍 Like</button>
                  <button class="btn btn-sm btn-link text-decoration-none p-0 text-secondary btn-dislike">👎 Dislike</button>
                  ${isOwner ? `<button class="btn btn-sm btn-link text-decoration-none p-0 text-danger btn-delete">Delete</button>` : ''}
              </div>
              
              <div class="reply-box d-none mt-2">
                  <textarea class="form-control bg-dark text-white mb-2" rows="2" placeholder="Write a reply..."></textarea>
                  <button class="btn btn-sm btn-warning btn-submit-reply">Post Reply</button>
                  <button class="btn btn-sm btn-outline-secondary btn-cancel-reply">Cancel</button>
              </div>
              ` : ''}
          </div>
      </div>
    `;

    
    if (!comment.isDeleted) {
     
        const replyBtn = container.querySelector(".btn-reply");
        const replyBox = container.querySelector(".reply-box");
        const cancelReplyBtn = container.querySelector(".btn-cancel-reply");
        
        replyBtn.addEventListener("click", () => replyBox.classList.toggle("d-none"));
        cancelReplyBtn.addEventListener("click", () => replyBox.classList.add("d-none"));

   
        const submitReplyBtn = container.querySelector(".btn-submit-reply");
        const replyInput = container.querySelector("textarea");

        submitReplyBtn.addEventListener("click", async () => {
            const text = replyInput.value.trim();
            if (!text) return alert("Reply cannot be empty");
            
            const success = await sendCommentToApi(movieId, text, comment.id); 
            if (success) {
                replyBox.classList.add("d-none");
                replyInput.value = "";
                await loadComments(movieId); 
            }
        });

        
        if (isOwner) {
            const deleteBtn = container.querySelector(".btn-delete");
            deleteBtn.addEventListener("click", () => deleteComment(comment.id, movieId));
        }

        
        container.querySelector(".btn-like").addEventListener("click", function() {
            this.classList.toggle("text-primary");
            this.classList.toggle("text-secondary");
        });
        
        container.querySelector(".btn-dislike").addEventListener("click", function() {
            this.classList.toggle("text-danger");
            this.classList.toggle("text-secondary");
        });
    }

    // 3. Recursive Replies (Nested)
    if (comment.replies && comment.replies.length > 0) {
        const replyContainer = document.createElement("div");
        replyContainer.classList.add("ms-5", "mt-3", "border-start", "border-secondary", "ps-3");
        
        comment.replies.forEach(reply => {
            replyContainer.appendChild(createCommentElement(reply, movieId));
        });
        
        container.appendChild(replyContainer);
    }

    return container;
}




async function handleMainPost(movieId) {
    const input = document.getElementById("comment-input");
    const text = input.value.trim();

    if (!text) return alert("Comment cannot be empty!");

    const success = await sendCommentToApi(movieId, text, null); 
    if (success) {
        input.value = "";
        await loadComments(movieId);
    }
}


async function sendCommentToApi(movieId, text, parentId) {
    if (!localStorage.getItem("jwt_token")) {
        alert("Please log in to post.");
        window.location.href = "index.html";
        return false;
    }

    try {
        const response = await fetchWithAuth(`${API_BASE}/api/comments`, {
            method: "POST",
            body: JSON.stringify({
                text: text,
                movieId: movieId,
                parentCommentId: parentId 
            })
        });

        if (response.ok) {
            return true;
        } else {
            alert("Failed to post. Please try again.");
            return false;
        }
    } catch (error) {
        console.error("Post Error:", error);
        return false;
    }
}


async function deleteComment(commentId, movieId) {
    if(!confirm("Are you sure you want to delete this comment?")) return;

    try {
        const response = await fetchWithAuth(`${API_BASE}/api/comments/${commentId}`, {
            method: "DELETE"
        });

        if (response.ok) {
            await loadComments(movieId); 
        } else {
            alert("Failed to delete comment.");
        }
    } catch (error) {
        console.error("Delete Error:", error);
    }
}



function styleRing(ring, rating) {
    if (!ring || rating == null) return;
    ring.textContent = rating.toFixed(1);
    const r = parseFloat(rating);
    ring.style.border = r >= 8.5 ? "2px solid green" : 
                        r >= 6.5 ? "2px solid yellow" : 
                        r >= 4.5 ? "2px solid orange" : "2px solid red";
}