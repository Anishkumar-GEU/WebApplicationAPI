const API_URL = "https://localhost:7081/api/BooksOps";

const bookForm = document.getElementById("bookForm");
const bookTableBody = document.getElementById("bookTableBody");
const message = document.getElementById("message");
const submitBtn = document.getElementById("submitBtn");

// GET: Fetch and display all books
async function loadBooks() {
    bookTableBody.innerHTML =
        '<tr><td colspan="5">Loading books...</td></tr>';

    try {
        const response = await fetch(API_URL);

        if (!response.ok) {
            throw new Error(`HTTP Error: ${response.status}`);
        }

        const books = await response.json();

        bookTableBody.replaceChildren();

        if (books.length === 0) {
            bookTableBody.innerHTML =
                '<tr><td colspan="5">No books found.</td></tr>';
            return;
        }

        books.forEach(book => {
            const row = document.createElement("tr");

            const values = [
                book.id,
                book.name,
                book.author,
                book.price
            ];

            values.forEach((value, index) => {
                const cell = document.createElement("td");

                if (index === 3 && value != null) {
                    cell.textContent = Number(value).toFixed(2);
                } else {
                    cell.textContent = value ?? "";
                }

                row.appendChild(cell);
            });

            // Create the Action cell
            const actionCell = document.createElement("td");

            // Create the Delete button
            const deleteBtn = document.createElement("button");

            deleteBtn.type = "button";
            deleteBtn.textContent = "Delete";
            deleteBtn.className = "delete-btn";

            // Store the book ID on the button
            deleteBtn.dataset.id = book.id;

            // Delete the selected book when clicked
            deleteBtn.addEventListener("click", () => {
                deleteBook(book.id, deleteBtn);
            });

            actionCell.appendChild(deleteBtn);
            row.appendChild(actionCell);

            bookTableBody.appendChild(row);
        });

    } catch (error) {
        console.error("GET API Error:", error);

        bookTableBody.innerHTML =
            '<tr><td colspan="5">Unable to load books. Check API URL, HTTPS certificate, and CORS settings.</td></tr>';
    }
}

// POST: Insert a new book
bookForm.addEventListener("submit", async function (event) {
    event.preventDefault();

    message.textContent = "";
    submitBtn.disabled = true;
    submitBtn.textContent = "Adding Book...";

    const book = {
        name: document.getElementById("name").value.trim(),
        author: document.getElementById("authorName").value.trim(),
        price: Number(document.getElementById("price").value)
    };

    if (
        !book.name ||
        !book.author ||
        !Number.isFinite(book.price) ||
        book.price <= 0
    ) {
        message.textContent = "Please enter valid book details.";
        message.style.color = "red";
        submitBtn.disabled = false;
        submitBtn.textContent = "Add Book";
        return;
    }

    try {
        const response = await fetch(API_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(book)
        });

        if (response.status === 409) {
            message.textContent =
                "This book already exists for the same author.";
            message.style.color = "red";
            return;
        }

        if (!response.ok) {
            const errorText = await response.text();

            throw new Error(
                errorText || `HTTP Error: ${response.status}`
            );
        }

        message.textContent = "Book added successfully!";
        message.style.color = "green";

        bookForm.reset();

        // Reload the table after successful insertion
        await loadBooks();

    } catch (error) {
        console.error("POST API Error:", error);

        message.textContent =
            "Unable to add book. Check the API and browser Console.";
        message.style.color = "red";

    } finally {
        submitBtn.disabled = false;
        submitBtn.textContent = "Add Book";
    }
});

// DELETE: Delete a book using its ID
async function deleteBook(id, deleteBtn) {
    const confirmed = confirm(
        `Are you sure you want to delete book ID ${id}?`
    );

    if (!confirmed) {
        return;
    }

    message.textContent = "";

    deleteBtn.disabled = true;
    deleteBtn.textContent = "Deleting...";

    try {
        // Pass the selected book ID in the API URL
        const response = await fetch(`${API_URL}/${encodeURIComponent(id)}`, {
            method: "DELETE"
        });

        if (!response.ok) {
            const errorText = await response.text();

            throw new Error(
                errorText || `HTTP Error: ${response.status}`
            );
        }

        message.textContent = `Book ID ${id} deleted successfully!`;
        message.style.color = "green";

        // Refresh the table after deletion
        await loadBooks();

    } catch (error) {
        console.error("DELETE API Error:", error);

        message.textContent =
            `Unable to delete book ID ${id}. Check the API route and browser Console.`;
        message.style.color = "red";

        deleteBtn.disabled = false;
        deleteBtn.textContent = "Delete";
    }
}

// Automatically load books when the page opens
loadBooks();