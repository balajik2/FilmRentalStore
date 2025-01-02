// Fetch the JWT token from localStorage
function getAuthToken() {
    const token = localStorage.getItem('jwtToken');
    if (!token) {
        alert('You are not authenticated. Please log in.');
        return null;
    }
    return token;
}

// Show loading state on button
function showLoadingState(button) {
    button.disabled = true; // Disable the button to prevent multiple clicks
    button.innerText = 'Loading...'; // Change button text
}

// Hide loading state on button
function hideLoadingState(button, text) {
    button.disabled = false;
    button.innerText = text; // Reset button text
}

// Handle 401 Unauthorized errors (expired token or no token)
function handleAuthError(xhr) {
    if (xhr.status === 401) {
        alert('Authentication expired. Please log in again.');
        localStorage.removeItem('jwtToken');
        window.location.href = '/login'; // Redirect to login page or show login modal
    }
}
// Add customer
document.getElementById('addActorBtn').addEventListener('click', async function () {
    const firstName = document.getElementById('actorFirstName').value.trim();
    const lastName = document.getElementById('actorLastName').value.trim();
    const lastUpdate = document.getElementById('actorLastUpdate').value;

    if (!firstName || !lastName || !lastUpdate) {
        alert('All fields are required!');
        return;
    }

    const token = getAuthToken();
    if (!token) return;

    const actorDto = {
        firstName,
        lastName,
        lastUpdate
    };

    showLoadingState(this);

    try {
<<<<<<< HEAD
        const response = await fetch('https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net
 /api/actors/post', {
=======
        const response = await fetch('https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/actors/post', {
>>>>>>> origin/FilmRentalStore-2
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(actorDto)
        });

        hideLoadingState(this, 'Add Actor');

        const resultDiv = document.getElementById('addActorResult');
        if (response.ok) {
            resultDiv.textContent = 'Actor added successfully!';
            resultDiv.className = 'result-message success';
            resultDiv.style.display = 'block';
        } else {
            resultDiv.textContent = 'Failed to add actor. Please try again.';
            resultDiv.className = 'result-message error';
            resultDiv.style.display = 'block';
        }
    } catch (error) {
        hideLoadingState(this, 'Add Actor');
        const resultDiv = document.getElementById('addActorResult');
        resultDiv.textContent = `Error: ${error.message}`;
        resultDiv.className = 'result-message error';
        resultDiv.style.display = 'block';
    }
});


// Get Actor by last name
document.getElementById('getActorByLastNameBtn').addEventListener('click', function () {
    const lastName = document.getElementById('actorLastName1').value.trim();
    if (!lastName) {
        alert("Please enter a last name.");
        return;
    }

    const token = getAuthToken(); // Assuming getAuthToken retrieves the auth token
    if (!token) return;

    showLoadingState(this); // Assuming showLoadingState shows a loading spinner

    // Fetch the actors using AJAX
    $.ajax({
<<<<<<< HEAD
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net
 /api/actors/lastname/${encodeURIComponent(lastName)}`,
=======
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/actors/lastname/${encodeURIComponent(lastName)}`,
>>>>>>> origin/FilmRentalStore-2
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            hideLoadingState(document.getElementById('getActorByLastNameBtn'), 'Get Actor By Last Name'); // Hide loading spinner
            const tbody = document.querySelector("#searchActorTable2 tbody");
            tbody.innerHTML = ""; // Clear any previous results

            if (!response || response.length === 0) {
                alert("No actors found with the given last name.");
                document.getElementById('searchActorTable2').style.display = 'none';
                return;
            }

            // Populate the table with the response data
            response.forEach(actor => {
                const row = `
                        <tr>
                            <td>${actor.actorId}</td>
                            <td>${actor.firstName}</td>
                            <td>${actor.lastName}</td>
                        </tr>
                    `;
                tbody.innerHTML += row;
            });

            document.getElementById('searchActorTable2').style.display = 'table'; // Show the table
        },
        error: function () {
            hideLoadingState(document.getElementById('getActorByLastNameBtn'), 'Get Actor By Last Name');
            alert("Failed to fetch actors by last name.");
        }
    });
});

// Get Actor by first name

document.getElementById('getActorByFirstNameBtn').addEventListener('click', function () {
    const firstName = document.getElementById('actorFirstName1').value.trim();
    if (!firstName) {
        alert("Please enter a first name.");
        return;
    }

    const token = getAuthToken(); // Assuming getAuthToken retrieves the auth token
    if (!token) return;

    showLoadingState(this); // Show loading spinner (assuming implementation)

    $.ajax({
<<<<<<< HEAD
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net
 /api/actors/firstname/${encodeURIComponent(firstName)}`,
=======
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/actors/firstname/${encodeURIComponent(firstName)}`,
>>>>>>> origin/FilmRentalStore-2
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            hideLoadingState(document.getElementById('getActorByFirstNameBtn'), 'Get Actor By First Name'); // Hide loading spinner
            const tbody = document.querySelector("#searchActorTable1 tbody");
            tbody.innerHTML = ""; // Clear previous results

            if (!response || response.length === 0) {
                alert("No actors found with the given first name.");
                document.getElementById('searchActorTable1').style.display = 'none';
                return;
            }

            // Populate the table with actor data
            response.forEach(actor => {
                const row = `
                    <tr>
                        <td>${actor.actorId}</td>
                        <td>${actor.firstName}</td>
                        <td>${actor.lastName}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            document.getElementById('searchActorTable1').style.display = 'table'; // Show the table
        },
        error: function () {
            hideLoadingState(document.getElementById('getActorByFirstNameBtn'), 'Get Actor By First Name');
            alert("Failed to fetch actors by first name.");
        }
    });
});

// Update Actor Last Name
document.getElementById('updateLastNameBtn').addEventListener('click', async function () {
    const actorId = document.getElementById('updateActorId').value.trim();
    const newLastName = document.getElementById('updateLastName').value.trim();

    if (!actorId || !newLastName) {
        alert('Both Actor ID and Last Name are required!');
        return;
    }

    const token = getAuthToken(); // Ensure the user is authenticated
    if (!token) return;

    showLoadingState(this);

    try {
<<<<<<< HEAD
        const response = await fetch(`https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net
 /api/actors/update/lastname/${actorId}?name=${encodeURIComponent(newLastName)}`, {
=======
        const response = await fetch(`https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/actors/update/lastname/${actorId}?name=${encodeURIComponent(newLastName)}`, {
>>>>>>> origin/FilmRentalStore-2
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        hideLoadingState(this, 'Update Last Name');

        const resultDiv = document.getElementById('updateLastNameResult');
        const updatedActorTable = document.getElementById('updatedActorTable');
        const tbody = updatedActorTable.querySelector('tbody');

        if (response.ok) {
            const updatedActors = await response.json();

            // Clear any previous rows
            tbody.innerHTML = '';

            // Populate the table with updated actor data
            updatedActors.forEach(actor => {
                const row = `
                    <tr>
                        <td>${actor.actorId}</td>
                        <td>${actor.firstName}</td>
                        <td>${actor.lastName}</td>
                        <td>${actor.lastUpdate}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            // Show the result message and the table
            resultDiv.textContent = 'Actor last name updated successfully!';
            resultDiv.className = 'result-message success';
            resultDiv.style.display = 'block';
            updatedActorTable.style.display = 'table';
        } else {
            const errorMsg = await response.text();
            resultDiv.textContent = `Failed to update actor's last name. Error: ${errorMsg}`;
            resultDiv.className = 'result-message error';
            resultDiv.style.display = 'block';
            updatedActorTable.style.display = 'none';
        }
    } catch (error) {
        hideLoadingState(this, 'Update Last Name');
        const resultDiv = document.getElementById('updateLastNameResult');
        resultDiv.textContent = `Error: ${error.message}`;
        resultDiv.className = 'result-message error';
        resultDiv.style.display = 'block';
    }
});
// Update Actor First Name
document.getElementById('updateFirstNameBtn').addEventListener('click', async function () {
    const actorId = document.getElementById('updateActorId1').value.trim();
    const newFirstName = document.getElementById('updateFirstName').value.trim();

    if (!actorId || !newFirstName) {
        alert('Both Actor ID and First Name are required!');
        return;
    }

    const token = getAuthToken(); // Ensure the user is authenticated
    if (!token) return;

    showLoadingState(this);

    try {
<<<<<<< HEAD
        const response = await fetch(`https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net
 /api/actors/update/firstname/${actorId}?name=${encodeURIComponent(newFirstName)}`, {
=======
        const response = await fetch(`https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/actors/update/firstname/${actorId}?name=${encodeURIComponent(newFirstName)}`, {
>>>>>>> origin/FilmRentalStore-2
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        hideLoadingState(this, 'Update First Name');

        const resultDiv = document.getElementById('updateFirstNameResult');
        const updatedActorTable1 = document.getElementById('updatedActorTable1');
        const tbody = updatedActorTable1.querySelector('tbody');

        if (response.ok) {
            const updatedActors = await response.json();

            // Clear any previous rows
            tbody.innerHTML = '';

            // Populate the table with updated actor data
            updatedActors.forEach(actor => {
                const row = `
                    <tr>
                        <td>${actor.actorId}</td>
                        <td>${actor.firstName}</td>
                        <td>${actor.lastName}</td>
                        <td>${actor.lastUpdate}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            // Show the result message and the table
            resultDiv.textContent = 'Actor first name updated successfully!';
            resultDiv.className = 'result-message success';
            resultDiv.style.display = 'block';
            updatedActorTable1.style.display = 'table';
        } else {
            const errorMsg = await response.text();
            resultDiv.textContent = `Failed to update actor's first name. Error: ${errorMsg}`;
            resultDiv.className = 'result-message error';
            resultDiv.style.display = 'block';
            updatedActorTable1.style.display = 'none';
        }
    } catch (error) {
        hideLoadingState(this, 'Update First Name');
        const resultDiv = document.getElementById('updateFirstNameResult');
        resultDiv.textContent = `Error: ${error.message}`;
        resultDiv.className = 'result-message error';
        resultDiv.style.display = 'block';
    }
});




// Fetch films by actor ID
document.getElementById("getFilmsBtn").addEventListener("click", async function () {
    const actorId = document.getElementById("actorIdInput").value.trim();

    if (!actorId) {
        alert("Please enter a valid Actor ID.");
        return;
    }

    const token = getAuthToken();
    if (!token) return;

    showLoadingState(this);

    try {
<<<<<<< HEAD
        const response = await fetch(`https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net
 /api/actors/${actorId}/films`, {
=======
        const response = await fetch(`https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/actors/${actorId}/films`, {
>>>>>>> origin/FilmRentalStore-2
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            }
        });

        hideLoadingState(this, "Get Films");

        if (response.ok) {
            const films = await response.json();
            populateFilmsTable(films);
        } else if (response.status === 404) {
            alert("Actor not found or no films available.");
            document.getElementById("filmsTable").style.display = "none";
        } else {
            alert("Failed to fetch films. Please try again.");
        }
    } catch (error) {
        hideLoadingState(this, "Get Films");
        console.error("Error fetching films:", error);
    }
});

// Populate the films table
function populateFilmsTable(films) {
    const table = document.getElementById("filmsTable");
    const tbody = table.querySelector("tbody");
    tbody.innerHTML = "";

    if (films.length === 0) {
        alert("No films found for the given Actor ID.");
        table.style.display = "none";
        return;
    }

    films.forEach(film => {
        const row = `
            <tr>
                <td>${film.filmId}</td>
                <td>${film.title}</td>
                <td>${film.description}</td>
                <td>${film.releaseYear}</td>
            </tr>
        `;
        tbody.innerHTML += row;
    });

    table.style.display = "table";
}

// Assign film to actor

document.getElementById('assignFilmToActorBtn').addEventListener('click', function () {
    // Get values from input fields
    const actorId2 = document.getElementById('ActorIdAssignFilm').value.trim();
    const filmId2 = document.getElementById('FilmIdAssignFilm').value.trim();

    // Validate inputs
    if (!actorId2 || !filmId2) {
        alert("Please provide both Actor ID and Film ID.");
        return;
    }

    // Retrieve authorization token
    const token = getAuthToken(); // Assuming getAuthToken is a function that retrieves the token
    if (!token) {
        alert("Authorization token is missing.");
        return;
    }

    // Construct the API URL
<<<<<<< HEAD
    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net
 /api/actors/${encodeURIComponent(actorId2)}/film?filmId=${encodeURIComponent(filmId2)}`;
=======
    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/actors/${encodeURIComponent(actorId2)}/film?filmId=${encodeURIComponent(filmId2)}`;
>>>>>>> origin/FilmRentalStore-2

    // Make the AJAX request
    $.ajax({
        url: url,
        type: 'PUT', // Use POST method to assign film
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            alert('Film assigned to the actor successfully.');
            console.log("Film assigned to the actor successfully.");

            if (Array.isArray(response) && response.length > 0) {
                let tbody = document.querySelector("#assignedFilmsTable tbody");
                tbody.innerHTML = '';

                response.forEach(film => {
                    let row = `
              <tr>
                <td>${film.filmId}</td>
                <td>${film.title}</td>
                <td>${film.description || ''}</td>
                <td>${film.releaseYear || ''}</td>
                <td>${film.languageId}</td>
                <td>${film.originalLanguageId || ''}</td>
                <td>${film.rentalDuration}</td>
                <td>${film.rentalRate}</td>
                <td>${film.length || ''}</td>
                <td>${film.replacementCost}</td>
                <td>${film.rating || ''}</td>
                <td>${new Date(film.lastUpdate).toLocaleString()}</td>
              </tr>
            `;
                    tbody.innerHTML += row;
                });

                document.getElementById('assignedFilmsTable').style.display = 'table';
            } else {
                alert('No films found.');
            }
        },
        error: function (xhr, status, error) {
            // Error handling
            console.error("Error assigning film to actor", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to assign film to actor'}`);
        }
    });
});

// Top TEN Actor By Film
document.getElementById('fetchTop10ActorsBtn').addEventListener('click', function () {
<<<<<<< HEAD
    const apiUrl = 'https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net
 /api/actors/toptenbyfilmcount';
=======
    const apiUrl = 'https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/actors/toptenbyfilmcount';
>>>>>>> origin/FilmRentalStore-2
    const table = document.getElementById('top10ActorsTable');
    const tbody = table.querySelector('tbody');

    // Show loading state
    this.disabled = true;
    this.innerText = 'Loading...';

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch data.');
            }
            return response.json();
        })
        .then(data => {
            // Clear previous table rows
            tbody.innerHTML = '';

            if (data.length === 0) {
                alert('No data found.');
                table.style.display = 'none';
                return;
            }

            // Populate table with data
            data.forEach(actor => {
                const row = `
                    <tr>
                        <td>${actor.actorId}</td>
                        <td>${actor.firstName}</td>
                        <td>${actor.lastName}</td>
                        <td>${actor.noOfFilm}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            table.style.display = 'table'; // Show table
        })
        .catch(error => {
            alert(error.message);
        })
        .finally(() => {
            // Reset button state
            this.disabled = false;
            this.innerText = 'Fetch Top 10 Actors';
        });
});

