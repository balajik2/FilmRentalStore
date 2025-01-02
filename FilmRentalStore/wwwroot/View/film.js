// Fetch the JWT token from localStorage
function getAuthToken() {
    const token = localStorage.getItem('jwtToken');
    if (!token) {
        alert('You are not authenticated. Please log in.');
        return null;
    }
    return token;
}


function showLoadingState(button) {
    button.disabled = true;
    button.innerText = 'Loading...';
}


function hideLoadingState(button, text) {
    button.disabled = false;
    button.innerText = text;
}


function handleAuthError(xhr) {
    if (xhr.status === 401) {
        alert('Authentication expired. Please log in again.');
        localStorage.removeItem('jwtToken');
        window.location.href = '/View/login.html';
    }
}

//Add Films


document.getElementById('addFilmBtn').addEventListener('click', function () {
    const filmRequestBody = document.getElementById('filmRequestBody').value.trim();


    try {
        const filmData = JSON.parse(filmRequestBody);

        if (!filmData.title || !filmData.description) {
            alert("Please provide the film title and description.");
            return;
        }

        const token = getAuthToken();  // Function to get the authentication token
        if (!token) {
            alert("Authorization token is missing.");
            return;
        }

        const url = "https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/post"; // Your API endpoint

        // Make the API call
        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify(filmData),  // Send the JSON data
            contentType: 'application/json', // Set content type to JSON
            headers: {
                Authorization: `Bearer ${token}`  // Include the authorization token
            },
            success: function (response) {
                console.log("Film added successfully:", response);

                // Display the response in the page
                const responseElement = document.getElementById('addFilmResponse');
                responseElement.style.display = 'block';  // Show the response block
                responseElement.textContent = JSON.stringify(response, null, 2);  // Display formatted JSON response
            },
            error: function (xhr, status, error) {
                console.error("Error adding film:", xhr, status, error);
                alert(`Error: ${xhr.responseText || 'Unable to add film'}`);
            }
        });
    } catch (error) {
        alert("Invalid JSON format. Please check the request body.");
        console.error("Invalid JSON:", error);
    }
});


//search film by Title
document.getElementById('searchFilmsByTitleBtn').addEventListener('click', function () {
    const token = getAuthToken(); // Function to retrieve the token
    if (!token) return; // Stop if token is not available

    const title = document.getElementById('filmTitle').value.trim(); // Get the input value
    if (!title) {
        alert("Please provide a title to search for.");
        return;
    }

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/title/${encodeURIComponent(title)}`, // Replace with the correct API endpoint
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            console.log("Search results for films by title:", response);
            if (response && response.length > 0) {
                let tbody = document.querySelector("#filmsByTitleTable tbody");
                tbody.innerHTML = ''; // Clear existing table content

                response.forEach(film => {
                    let row = `
                        <tr>
                             <td>${film.filmId}</td>
                            <td>${film.title}</td>
                            <td>${film.description}</td>
                            <td>${film.releaseYear}</td>
                            <td>${film.rentalDuration}</td>
                            <td>${film.rentalRate}</td>
                            <td>${film.length}</td>
                            <td>${film.rating}</td>
                            <td>${film.languageId}</td>
                              
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('filmsByTitleTable').style.display = 'table'; // Show the table
            } else {
                alert('No films found with the provided title.');
                document.getElementById('filmsTable').style.display = 'none'; // Hide the table
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                handleAuthError(xhr); // Handle authentication errors
            }
            console.error("Error searching films by title", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to search films'}`);
        }
    });
});

//Search Films By Release Year
document.getElementById('searchFilmsByReleaseYearBtn').addEventListener('click', function () {
    const token = getAuthToken(); // Function to retrieve the token
    if (!token) return; // Stop if token is not available

    const releaseYear = document.getElementById('filmReleaseYear').value.trim(); // Get the input value
    if (!releaseYear) {
        alert("Please provide a release year to search for.");
        return;
    }

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/year/${encodeURIComponent(releaseYear)}`, // Replace with the correct API endpoint
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            console.log("Search results for films by release year:", response);
            if (response && response.length > 0) {
                let tbody = document.querySelector("#filmsByReleaseYearTable tbody");
                tbody.innerHTML = ''; // Clear existing table content

                response.forEach(film => {
                    let row = `
                        <tr>
                            <td>${film.filmId}</td>
                            <td>${film.title}</td>
                            <td>${film.description || 'N/A'}</td>
                            <td>${film.releaseYear}</td>
                            <td>${film.rentalDuration}</td>
                            <td>${film.rentalRate}</td>
                            <td>${film.length}</td>
                            <td>${film.rating}</td>
                            <td>${film.languageId}</td>
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('filmsByReleaseYearTable').style.display = 'table'; // Show the table
            } else {
                alert('No films found with the provided release year.');
                document.getElementById('filmsByReleaseYearTable').style.display = 'none'; // Hide the table
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                handleAuthError(xhr); // Handle authentication errors
            }
            console.error("Error searching films by release year", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to search films'}`);
        }
    });
});

//search Films By Rental Duration Greater
document.getElementById('searchFilmsByRentalDurationBtn').addEventListener('click', function () {
    const token = getAuthToken(); // Function to retrieve the token
    if (!token) return; // Stop if token is not available

    const rentalDuration = document.getElementById('filmRentalDuration').value.trim(); // Get the input value
    if (!rentalDuration || isNaN(rentalDuration) || rentalDuration <= 0) {
        alert("Please provide a valid rental duration (greater than 0).");
        return;
    }

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/duration/gt/${encodeURIComponent(rentalDuration)}`, // Replace with the correct API endpoint
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            console.log("Search results for films by rental duration:", response);
            if (response && response.length > 0) {
                let tbody = document.querySelector("#filmsByRentalDurationTable tbody");
                tbody.innerHTML = ''; // Clear existing table content

                response.forEach(film => {
                    let row = `
                        <tr>
                            <td>${film.filmId}</td>
                            <td>${film.title}</td>
                            <td>${film.description || 'N/A'}</td>
                            <td>${film.releaseYear}</td>
                            <td>${film.rentalDuration}</td>
                            <td>${film.rentalRate}</td>
                            <td>${film.length}</td>
                            <td>${film.rating}</td>
                            <td>${film.languageId}</td>
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('filmsByRentalDurationTable').style.display = 'table'; // Show the table
            } else {
                alert('No films found with the specified rental duration.');
                document.getElementById('filmsByRentalDurationTable').style.display = 'none'; // Hide the table
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                handleAuthError(xhr); // Handle authentication errors
            }
            console.error("Error searching films by rental duration", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to search films'}`);
        }
    });
});


//Search films by RentalRate Greater
document.getElementById('searchFilmsByRentalRateBtn').addEventListener('click', function () {
    const token = getAuthToken(); // Function to retrieve the token
    if (!token) return; // Stop if token is not available

    const rentalRate = document.getElementById('filmRentalRate').value.trim(); // Get the input value
    if (!rentalRate || isNaN(rentalRate) || rentalRate <= 0) {
        alert("Please provide a valid rental rate (greater than 0).");
        return;
    }

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/rate/gt/${encodeURIComponent(rentalRate)}`, // Replace with the correct API endpoint
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            console.log("Search results for films by rental rate:", response);
            if (response && response.length > 0) {
                let tbody = document.querySelector("#filmsByRentalRateTable tbody");
                tbody.innerHTML = ''; // Clear existing table content

                response.forEach(film => {
                    let row = `
                        <tr>
                            <td>${film.filmId}</td>
                            <td>${film.title}</td>
                            <td>${film.description || 'N/A'}</td>
                            <td>${film.releaseYear}</td>
                            <td>${film.rentalDuration}</td>
                            <td>${film.rentalRate}</td>
                            <td>${film.length}</td>
                            <td>${film.rating}</td>
                            <td>${film.languageId}</td>
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('filmsByRentalRateTable').style.display = 'table'; // Show the table
            } else {
                alert('No films found with the specified rental rate.');
                document.getElementById('filmsByRentalRateTable').style.display = 'none'; // Hide the table
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                handleAuthError(xhr); // Handle authentication errors
            }
            console.error("Error searching films by rental rate", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to search films'}`);
        }
    });
});

//Search Films By Length Greater
document.getElementById('searchFilmsByLengthBtn').addEventListener('click', function () {
    const token = getAuthToken(); // Function to retrieve the token
    if (!token) return; // Stop if token is not available

    const length = document.getElementById('filmLength').value.trim(); // Get the input value
    if (!length || isNaN(length) || length <= 0) {
        alert("Please provide a valid film length (greater than 0).");
        return;
    }

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/length/gt/${encodeURIComponent(length)}`, // Replace with the correct API endpoint
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            console.log("Search results for films by length:", response);
            if (response && response.length > 0) {
                let tbody = document.querySelector("#filmsByLengthTable tbody");
                tbody.innerHTML = ''; // Clear existing table content

                response.forEach(film => {
                    let row = `
                        <tr>
                            <td>${film.filmId}</td>
                            <td>${film.title}</td>
                            <td>${film.description || 'N/A'}</td>
                            <td>${film.releaseYear}</td>
                            <td>${film.rentalDuration}</td>
                            <td>${film.rentalRate}</td>
                            <td>${film.length}</td>
                            <td>${film.rating}</td>
                            <td>${film.languageId}</td>
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('filmsByLengthTable').style.display = 'table'; // Show the table
            } else {
                alert('No films found with the specified length.');
                document.getElementById('filmsByLengthTable').style.display = 'none'; // Hide the table
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                handleAuthError(xhr); // Handle authentication errors
            }
            console.error("Error searching films by length", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to search films'}`);
        }
    });
});


//Search Films By Rental Duration Lower

document.getElementById('searchingFilmsByRentalDurationBtn').addEventListener('click', function () {
    const token = getAuthToken(); // Function to retrieve the token
    if (!token) return; // Stop if token is not available

    const rentalDuration = document.getElementById('filmRentalDurations').value.trim(); // Get the input value
    if (!rentalDuration || isNaN(rentalDuration) || rentalDuration <= 0) {
        alert("Please provide a valid rental duration (greater than 0).");
        return;
    }

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/duration/lt/${encodeURIComponent(rentalDuration)}`, // Replace with the correct API endpoint
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            console.log("Search results for films by rental duration:", response);
            if (response && response.length > 0) {
                let tbody = document.querySelector("#filmsByRentalDurationsTable tbody");
                tbody.innerHTML = ''; // Clear existing table content

                response.forEach(film => {
                    let row = `
                        <tr>
                            <td>${film.filmId}</td>
                            <td>${film.title}</td>
                            <td>${film.description || 'N/A'}</td>
                            <td>${film.releaseYear}</td>
                            <td>${film.rentalDuration}</td>
                            <td>${film.rentalRate}</td>
                            <td>${film.length}</td>
                            <td>${film.rating}</td>
                            <td>${film.languageId}</td>
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('filmsByRentalDurationsTable').style.display = 'table'; // Show the table
            } else {
                alert('No films found with the specified rental duration.');
                document.getElementById('filmsByRentalDurationsTable').style.display = 'none'; // Hide the table
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                handleAuthError(xhr); // Handle authentication errors
            }
            console.error("Error searching films by rental duration", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to search films'}`);
        }
    });
});


//Search Films By Rental Rate Lower

document.getElementById('searchFilmsByRentalRateLowerBtn').addEventListener('click', function () {
    const token = getAuthToken(); // Function to retrieve the token
    if (!token) return; // Stop if token is not available

    const rentalRate = document.getElementById('filmRentalRateLower').value.trim(); // Get the input value
    if (!rentalRate || isNaN(rentalRate) || rentalRate <= 0) {
        alert("Please provide a valid rental rate (greater than 0).");
        return;
    }

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/rate/lt/${encodeURIComponent(rentalRate)}`, // Replace with the correct API endpoint
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            console.log("Search results for films by rental rate lower:", response);
            if (response && response.length > 0) {
                let tbody = document.querySelector("#filmsByRentalRateLowerTable tbody");
                tbody.innerHTML = ''; // Clear existing table content

                response.forEach(film => {
                    let row = `
                        <tr>
                            <td>${film.filmId}</td>
                            <td>${film.title}</td>
                            <td>${film.description || 'N/A'}</td>
                            <td>${film.releaseYear}</td>
                            <td>${film.rentalDuration}</td>
                            <td>${film.rentalRate}</td>
                            <td>${film.length}</td>
                            <td>${film.rating}</td>
                            <td>${film.languageId}</td>
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('filmsByRentalRateLowerTable').style.display = 'table'; // Show the table
            } else {
                alert('No films found with the specified rental rate.');
                document.getElementById('filmsByRentalRateLowerTable').style.display = 'none'; // Hide the table
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                handleAuthError(xhr); // Handle authentication errors
            }
            console.error("Error searching films by rental rate lower", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to search films'}`);
        }
    });
});

//Search Films By Length Lower

document.getElementById('searchFilmsByLengthLowerBtn').addEventListener('click', function () {
    const token = getAuthToken(); // Function to retrieve the token
    if (!token) return; // Stop if token is not available

    const length = document.getElementById('filmLengthLower').value.trim(); // Get the input value
    if (!length || isNaN(length) || length <= 0) {
        alert("Please provide a valid length (greater than 0).");
        return;
    }

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/length/lt/${encodeURIComponent(length)}`, // Replace with the correct API endpoint
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            console.log("Search results for films by length lower:", response);
            if (response && response.length > 0) {
                let tbody = document.querySelector("#filmsByLengthLowerTable tbody");
                tbody.innerHTML = ''; // Clear existing table content

                response.forEach(film => {
                    let row = `
                        <tr>
                            <td>${film.filmId}</td>
                            <td>${film.title}</td>
                            <td>${film.description || 'N/A'}</td>
                            <td>${film.releaseYear}</td>
                            <td>${film.rentalDuration}</td>
                            <td>${film.rentalRate}</td>
                            <td>${film.length}</td>
                            <td>${film.rating}</td>
                            <td>${film.languageId}</td>
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('filmsByLengthLowerTable').style.display = 'table'; // Show the table
            } else {
                alert('No films found with the specified length.');
                document.getElementById('filmsByLengthLowerTable').style.display = 'none'; // Hide the table
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                handleAuthError(xhr); // Handle authentication errors
            }
            console.error("Error searching films by length lower", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to search films'}`);
        }
    });
});

//Search Films By Release Year Range

$('#searchFilmsByReleaseYearRangeBtn').click(function () {
    const startYear = $('#filmReleaseYearFrom').val().trim();
    const endYear = $('#filmReleaseYearTo').val().trim();

    // Validate inputs to ensure they are numerical strings
    if (!startYear || !endYear) {
        alert('Please fill in both Start Year and End Year.');
        return;
    }

    if (!/^\d{4}$/.test(startYear) || !/^\d{4}$/.test(endYear)) {
        alert('Please enter valid 4-digit years.');
        return;
    }

    if (parseInt(startYear) > parseInt(endYear)) {
        alert('Start Year should not be greater than End Year.');
        return;
    }

    const token = localStorage.getItem('jwtToken');
    if (!token) {
        alert('You need to be logged in to search films.');
        return;
    }

    // AJAX request to search films by release year range
    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/betweenyear/${startYear}/${endYear}`,
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            const tableBody = $('#filmsByReleaseYearTableRange tbody');
            tableBody.empty(); // Clear previous results

            if (response && response.length > 0) {
                response.forEach(film => {
                    const row = `
                    <tr>
                        <td>${film.filmId}</td>
                        <td>${film.title}</td>
                        <td>${film.description}</td>
                        <td>${film.releaseYear}</td>
                        <td>${film.rentalDuration}</td>
                        <td>${film.rentalRate}</td>
                        <td>${film.length}</td>
                        <td>${film.rating}</td>
                        <td>${film.languageId}</td>
                    </tr>`;
                    tableBody.append(row);
                });

                $('#filmsByReleaseYearTableRange').fadeIn();
            } else {
                alert('No films found for the given range.');
                $('#filmsByReleaseYearTableRange').fadeOut();
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            alert(`Failed to fetch films. Please try again later. Status: ${xhr.status}, ${xhr.statusText}`);
            console.log('Response:', xhr.responseText);
        }
    });

});





//Search Films By Rating Lower
document.getElementById('searchFilmsByRatingBtn').addEventListener('click', function () {
    const token = getAuthToken(); // Function to retrieve the token
    if (!token) return; // Stop if token is not available

    const selectedRating = document.getElementById('filmRating').value.trim(); // Get the selected rating

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/rating/lt/${encodeURIComponent(selectedRating)}`,
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            console.log("Search results for films by rating:", response);
            if (response && response.length > 0) {
                let tbody = document.querySelector("#filmsByRatingTable tbody");
                tbody.innerHTML = ''; // Clear existing table content

                response.forEach(film => {
                    let row = `
                        <tr>
                            <td>${film.filmId}</td>
                            <td>${film.title}</td>
                            <td>${film.description || 'N/A'}</td>
                            <td>${film.releaseYear}</td>
                            <td>${film.rentalDuration}</td>
                            <td>${film.rentalRate}</td>
                            <td>${film.length}</td>
                            <td>${film.rating}</td>
                            <td>${film.languageId}</td>
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('filmsByRatingTable').style.display = 'table'; // Show the table
            } else {
                alert('No films found with the specified rating.');
                document.getElementById('filmsByRatingTable').style.display = 'none'; // Hide the table
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                handleAuthError(xhr); // Handle authentication errors
            }
            console.error("Error searching films by rating", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to search films'}`);
        }
    });
});


//Search Films By Rating Higher
document.getElementById('searchFilmsByHigherRatingBtn').addEventListener('click', function () {
    const token = getAuthToken(); // Function to retrieve the token
    if (!token) return; // Stop if token is not available

    const selectedRating = document.getElementById('filmRatingSelect').value.trim(); // Get the selected rating

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/rating/gt/${encodeURIComponent(selectedRating)}`,
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            console.log("Search results for films by higher rating:", response);
            if (response && response.length > 0) {
                let tbody = document.querySelector("#filmsByHigherRatingTable tbody");
                tbody.innerHTML = ''; // Clear existing table content

                response.forEach(film => {
                    let row = `
                        <tr>
                            <td>${film.filmId}</td>
                            <td>${film.title}</td>
                            <td>${film.description || 'N/A'}</td>
                            <td>${film.releaseYear}</td>
                            <td>${film.rentalDuration}</td>
                            <td>${film.rentalRate}</td>
                            <td>${film.length}</td>
                            <td>${film.rating}</td>
                            <td>${film.languageId}</td>
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('filmsByHigherRatingTable').style.display = 'table'; // Show the table
            } else {
                alert('No films found with the specified higher rating.');
                document.getElementById('filmsByHigherRatingTable').style.display = 'none'; // Hide the table
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                handleAuthError(xhr); // Handle authentication errors
            }
            console.error("Error searching films by higher rating", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to search films'}`);
        }
    });
});

//Search Films By Language
document.getElementById('searchFilmsByLanguageBtn').addEventListener('click', function () {
    const token = getAuthToken(); // Function to retrieve the token
    if (!token) return; // Stop if token is not available

    const language = document.getElementById('filmLanguage').value.trim(); // Get the language input

    if (!language) {
        alert("Please enter a valid language.");
        return;
    }

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/language/${encodeURIComponent(language)}`, // Replace with the correct API endpoint
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            console.log("Search results for films by language:", response);
            if (response && response.length > 0) {
                let tbody = document.querySelector("#filmsByLanguageTable tbody");
                tbody.innerHTML = ''; // Clear existing table content

                response.forEach(film => {
                    let row = `
                        <tr>
                            <td>${film.filmId}</td>
                            <td>${film.title}</td>
                            <td>${film.description || 'N/A'}</td>
                            <td>${film.releaseYear}</td>
                            <td>${film.rentalDuration}</td>
                            <td>${film.rentalRate}</td>
                            <td>${film.length}</td>
                            <td>${film.rating}</td>
                            <td>${film.languageId}</td>
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('filmsByLanguageTable').style.display = 'table'; // Show the table
            } else {
                alert('No films found for the specified language.');
                document.getElementById('filmsByLanguageTable').style.display = 'none'; // Hide the table
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                handleAuthError(xhr); // Handle authentication errors
            }
            console.error("Error searching films by language", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to search films'}`);
        }
    });
});

//Get Films By Release Year
document.getElementById('getFilmCountByReleaseYearBtn').addEventListener('click', function () {
    const token = getAuthToken(); // Function to retrieve the token
    if (!token) return; // Stop if token is not available

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/countbyyear`, // Replace with the correct API endpoint
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            console.log("Film count by release year:", response);
            if (response && Object.keys(response).length > 0) {
                let tbody = document.querySelector("#filmCountByReleaseYearTable tbody");
                tbody.innerHTML = ''; // Clear existing table content

                Object.entries(response).forEach(([releaseYear, filmCount]) => {
                    let row = `
                        <tr>
                            <td>${releaseYear}</td>
                            <td>${filmCount}</td>
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('filmCountByReleaseYearTable').style.display = 'table'; // Show the table
            } else {
                alert('No film data found.');
                document.getElementById('filmCountByReleaseYearTable').style.display = 'none'; // Hide the table
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                handleAuthError(xhr); // Handle authentication errors
            }
            console.error("Error fetching film count by release year", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch film count'}`);
        }
    });
});



//Get Actor By Film Id
document.getElementById('getActorsByFilmIdBtn').addEventListener('click', function () {
    const filmId = document.getElementById('filmId').value.trim();
    if (!filmId) {
        alert("Please enter a valid Film ID.");
        return;
    }

    const token = getAuthToken(); // Function to retrieve the token
    if (!token) return; // Stop if token is not available

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/${filmId}/actors`,
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            console.log("Actors by film ID:", response);
            if (response && response.length > 0) {
                let tbody = document.querySelector("#actorsByFilmIdTable tbody");
                tbody.innerHTML = ''; // Clear existing table content

                response.forEach(actor => {
                    let row = `
                        <tr>
                            <td>${actor.actorId}</td>
                            <td>${actor.firstName}</td>
                            <td>${actor.lastName}</td>
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('actorsByFilmIdTable').style.display = 'table'; // Show the table
            } else {
                alert('No actors found for the given Film ID.');
                document.getElementById('actorsByFilmIdTable').style.display = 'none'; // Hide the table
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                handleAuthError(xhr); // Handle authentication errors
            }
            console.error("Error fetching actors by film ID", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch actors'}`);
        }
    });
});


//Get Films By Category
document.getElementById('getFilmsByCategoryBtn').addEventListener('click', function () {
    const token = getAuthToken(); // Function to retrieve the token
    if (!token) return; // Stop if token is not available

    const category = document.getElementById('filmCategory').value.trim(); // Get the input value
    if (!category) {
        alert("Please provide a category to search for.");
        return;
    }

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/category/${encodeURIComponent(category)}`, // Replace with the correct API endpoint
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}` // Include the authorization token
        },
        success: function (response) {
            console.log("Films by category:", response);
            if (response && response.length > 0) {
                let tbody = document.querySelector("#filmsByCategoryTable tbody");
                tbody.innerHTML = ''; // Clear existing table content

                response.forEach(film => {
                    let row = `
                        <tr>
                             <td>${film.filmId}</td>
                            <td>${film.title}</td>
                            <td>${film.description}</td>
                            <td>${film.releaseYear}</td>
                            <td>${film.rentalDuration}</td>
                            <td>${film.rentalRate}</td>
                            <td>${film.length}</td>
                            <td>${film.rating}</td>
                            <td>${film.languageId}</td>
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('filmsByCategoryTable').style.display = 'table'; // Show the table
            } else {
                alert('No films found in the provided category.');
                document.getElementById('filmsByCategoryTable').style.display = 'none'; // Hide the table
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                handleAuthError(xhr); // Handle authentication errors
            }
            console.error("Error fetching films by category", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch films'}`);
        }
    });
});


//Assign Actor to a film

document.getElementById('assignActorToFilmBtn').addEventListener('click', function () {
    // Get values from input fields
    const filmId = document.getElementById('FilmIdAssignActor').value.trim();
    const actorId = document.getElementById('ActorIdAssignActor').value.trim();

    // Validate inputs
    if (!filmId || !actorId) {
        alert("Please provide both Film ID and Actor ID.");
        return;
    }

    // Retrieve authorization token
    const token = getAuthToken(); // Assuming getAuthToken is a function that retrieves the token
    if (!token) {
        alert("Authorization token is missing.");
        return;
    }

    // Construct the API URL
    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/${encodeURIComponent(filmId)}/actor?actorId=${encodeURIComponent(actorId)}`;

    // Make the AJAX request
    $.ajax({
        url: url,
        type: 'PUT',  // Use POST method to assign actor
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            alert('Actor assigned to the film successfully.');
            console.log("Actor assigned to the film successfully.");


            if (Array.isArray(response) && response.length > 0) {

                let tbody = document.querySelector("#assignedActorsTable tbody");
                tbody.innerHTML = '';


                response.forEach(actor => {
                    let row = `
                        <tr>
                            <td>${actor.actorId}</td>
                            <td>${actor.firstName}</td>
                            <td>${actor.lastName}</td>
                        </tr>
                    `;
                    tbody.innerHTML += row;
                });

                document.getElementById('assignedActorsTable').style.display = 'table';
            } else {
                alert('No actors found.');
            }
        },
        error: function (xhr, status, error) {
            // Error handling
            console.error("Error assigning actor to film", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to assign actor to film'}`);
        }
    });
});

//Update Title
$('#updateFilm').click(function () {
    const filmId = $('#updateFilmId').val().trim();
    const title = $('#updateFilmTitle').val().trim();

    // Validate inputs
    if (!filmId || !title) {
        alert('Please fill in all fields.');
        return;
    }

    // AJAX request to update the film title
    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/update/title/${filmId}?title=${encodeURIComponent(title)}`,
        type: 'PUT',
        contentType: 'application/json',
        headers: { 'Authorization': `Bearer ${getAuthToken()}` }, // Replace with your token retrieval logic
        success: function () {
            alert('Film Title updated successfully!');
            // Show success message
            $('#updateFilmResult')
                .removeClass('alert-danger')
                .addClass('alert-success')
                .text('Film title updated successfully!')
                .fadeIn();

            // Clear input fields
            $('#updateFilmId').val('');
            $('#updateFilmTitle').val('');

            // Hide success message after a few seconds
            setTimeout(() => $('#updateFilmResult').fadeOut(), 5000);
        },
        error: function (xhr, status, error) {
            // Handle error response
            const errorMessage = xhr.responseText || error;
            $('#updateFilmResult')
                .removeClass('alert-success')
                .addClass('alert-danger')
                .text(`Failed to update title: ${errorMessage}`)
                .fadeIn();

            console.error('Error updating title:', errorMessage);
        },
    });
});


//Update Release Year
$('#updateFilmReleaseYearBtn').click(function () {
    const filmId = $('#UpdateReleaseYearfilmId').val().trim(); // Get the film ID
    const releaseYear = $('#UpdatefilmReleaseYear').val().trim(); // Get the new release year

    // Validate inputs
    if (!filmId || !releaseYear) {
        alert('Please fill in both Film ID and New Release Year.');
        return;
    }

    // AJAX request to update the film release year
    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/update/releaseyear/${filmId}?releaseYear=${encodeURIComponent(releaseYear)}`,
        type: 'PUT',
        contentType: 'application/json',
        headers: { 'Authorization': `Bearer ${getAuthToken()}` }, // Replace with your token retrieval logic

        success: function () {

            alert('Film Release Year updated successfully!');
            // Show success message
            $('#updateFilmResult1')
                .removeClass('alert-danger')
                .addClass('alert-success')
                .text('Film Release Year updated successfully!')
                .fadeIn();

            // Clear input fields
            $('#updateFilmId').val('');
            $('#updateFilmTitle').val('');

            // Hide success message after a few seconds
            setTimeout(() => $('#updateFilmResult1').fadeOut(), 5000);
        },
        error: function (xhr, status, error) {
            // Handle error response
            const errorMessage = xhr.responseText || error;
            $('#updateFilmResult1')
                .removeClass('alert-success')
                .addClass('alert-danger')
                .text(`Failed to update title: ${errorMessage}`)
                .fadeIn();

            console.error('Error updating title:', errorMessage);
        },
    });
});

//Update Rental Duration

$('#updateFilmRentalDurationBtnNew').click(function () {
    const filmId = $('#filmIdforUpdateRentalDurationNew').val().trim();
    const rentalDuration = $('#filmRentalDurationForUpdateNew').val().trim();

    // Validate inputs
    if (!filmId || !rentalDuration) {
        alert('Please fill in both Film ID and Rental Duration.');
        return;
    }

    

    // AJAX request to update the film rental duration
    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/update/rentalduration/${filmId}?rentalduration=${encodeURIComponent(rentalDuration)}`,
        type: 'PUT',
        contentType: 'application/json',
        headers: { 'Authorization': `Bearer ${getAuthToken()}` },
        success: function (response) {

            alert('Film Rental Duration updated successfully!');
            $('#updateFilmRentalDurationResultNew') // Consistent ID
                .removeClass('alert-danger')
                .addClass('alert-success')
                .text('Film rental duration updated successfully!')
                .fadeIn();

            // Clear input fields
            $('#filmIdforUpdateRentalDurationNew').val('');
            $('#filmRentalDurationForUpdateNew').val('');

            // Hide success message after a few seconds
            setTimeout(() => $('#updateFilmRentalDurationResultNew').fadeOut(), 5000);
        },
        error: function (xhr, status, error) {
            const errorMessage = xhr.responseText || error || "Unknown error";
            console.error('XHR Object:', xhr);
            console.error('Status:', status);
            console.error('Error:', error);
            $('#updateFilmRentalDurationResultNew')
                .removeClass('alert-success')
                .addClass('alert-danger')
                .text(`Failed to update rental duration: ${errorMessage}`)
                .fadeIn();
        }
    });
});


//Update Rental Rate Of Film

$('#updateFilmRentalRateBtn').click(function () {
    const filmId = $('#filmIdforUpdateRentalRate').val().trim();
    const rentalRate = $('#filmRentalRateforUpdate').val().trim();

    // Validate inputs
    if (!filmId || !rentalRate) {
        alert('Please fill in both Film ID and Rental Rate.');
        return;
    }

    // Construct the URL
    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/update/rentalrate/${filmId}?rentalrate=${encodeURIComponent(rentalRate)}`;
    console.log(`Request URL: ${url}`);

    // AJAX request to update the film rental rate
    $.ajax({
        url: url,
        type: 'PUT',
        contentType: 'application/json',
        headers: { 'Authorization': `Bearer ${getAuthToken()}` }, // Replace with your token retrieval logic
        success: function () {

            alert('Film Rental Rate updated successfully!');
            $('#updateFilmResultRentalRate')
                .removeClass('alert-danger')
                .addClass('alert-success')
                .text('Film rental rate updated successfully!')
                .fadeIn();

            // Clear input fields
            $('#filmIdforUpdateRentalRate').val('');
            $('#filmRentalRateforUpdate').val('');

            // Hide success message after a few seconds
            setTimeout(() => $('#updateFilmResultRentalRate').fadeOut(), 5000);
        },
        error: function (xhr, status, error) {
            console.error('XHR Object:', xhr);
            console.error('Status:', status);
            console.error('Error:', error);

            const errorMessage = xhr.status === 404
                ? 'Endpoint not found. Please check the API route and Film ID.'
                : xhr.responseText || error || 'Unknown error occurred.';

            $('#updateFilmResultRentalRate')
                .removeClass('alert-success')
                .addClass('alert-danger')
                .text(`Failed to update rental rate: ${errorMessage}`)
                .fadeIn();
        }
    });
});

//Update Rating

$('#updateFilmRatingBtn').click(function () {
    const filmId = $('#filmIdforUpdateRating').val().trim();
    const rating = $('#filmRatingforUpdate').val();

    // Validate inputs
    if (!filmId || !rating) {
        alert('Please fill in both Film ID and select a Rating.');
        return;
    }

    // Construct the URL
    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/update/rating/${filmId}?rating=${encodeURIComponent(rating)}`;
    console.log(`Request URL: ${url}`);

    // AJAX request to update the film rating
    $.ajax({
        url: url,
        type: 'PUT',
        contentType: 'application/json',
        headers: { 'Authorization': `Bearer ${getAuthToken()}` }, // Replace with your token retrieval logic
        success: function () {

            alert('Film Rating updated successfully!');
            $('#updateFilmResultRating')
                .removeClass('alert-danger')
                .addClass('alert-success')
                .text('Film rating updated successfully!')
                .fadeIn();

            // Clear input fields
            $('#filmIdforUpdateRating').val('');
            $('#filmRatingforUpdate').val('');

            // Hide success message after a few seconds
            setTimeout(() => $('#updateFilmResultRating').fadeOut(), 5000);
        },
        error: function (xhr, status, error) {
            console.error('XHR Object:', xhr);
            console.error('Status:', status);
            console.error('Error:', error);

            const errorMessage = xhr.status === 404
                ? 'Endpoint not found. Please check the API route and Film ID.'
                : xhr.responseText || error || 'Unknown error occurred.';

            $('#updateFilmResultRating')
                .removeClass('alert-success')
                .addClass('alert-danger')
                .text(`Failed to update rating: ${errorMessage}`)
                .fadeIn();
        }
    });
});


//Update Language

$('#updateFilmLanguageBtn').click(function () {
    const filmId = $('#filmIdforUpdateLanguage').val().trim();
    const selectedOption = $('#filmLanguageforUpdate').find(":selected");
    const languageId = selectedOption.val();
    const languageName = selectedOption.data('name'); // Get the language name based on selected option

    // Validate inputs
    if (!filmId || !languageId) {
        alert('Please fill in both Film ID and select a Language.');
        return;
    }

    // Construct the language data to send to the backend
    const languageData = {
        LanguageId: languageId,
        Name: languageName // Send the language name as well
    };

    // Construct the URL for the API request
    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/update/language/${filmId}`;

    // AJAX request to update the film language
    $.ajax({
        url: url,
        type: 'PUT',
        contentType: 'application/json',
        headers: { 'Authorization': `Bearer ${getAuthToken()}` }, // Replace with your token retrieval logic
        data: JSON.stringify(languageData), // Send both LanguageId and Name in the request body
        success: function () {

            alert('Film Language updated successfully!');
            $('#updateFilmResultLanguage')
                .removeClass('alert-danger')
                .addClass('alert-success')
                .text('Film language updated successfully!')
                .fadeIn();

            // Clear input fields
            $('#filmIdforUpdateLanguage').val('');
            $('#filmLanguageforUpdate').val('');

            // Hide success message after a few seconds
            setTimeout(() => $('#updateFilmResultLanguage').fadeOut(), 5000);
        },
        error: function (xhr, status, error) {
            const errorMessage = xhr.responseText || error || 'Unknown error occurred.';
            $('#updateFilmResultLanguage')
                .removeClass('alert-success')
                .addClass('alert-danger')
                .text(`Failed to update language: ${errorMessage}`)
                .fadeIn();

            console.error('Error updating language:', errorMessage);
        }
    });
});


//Update Category

$('#updateFilmCategoryBtn').click(function () {
    const filmId = $('#filmIdForUpdateCategory').val().trim();
    const categoryId = $('#filmCategory1').val().trim();

    // Validate inputs
    if (!filmId || !categoryId) {
        alert('Please fill in both Film ID and Category.');
        return;
    }

    // AJAX request to update the film category
    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/films/update/category/${filmId}?CategoryId=${encodeURIComponent(categoryId)}`,
        type: 'PUT',
        contentType: 'application/json',
        headers: { 'Authorization': `Bearer ${getAuthToken()}` }, // Replace with your token retrieval logic
        success: function (response) {

            alert('Film category updated successfully!');
            // Show success message
            $('#updateFilmResultCategory')
                .removeClass('alert-danger')
                .addClass('alert-success')
                .text('Film category updated successfully!')
                .fadeIn();

            // Clear input fields
            $('#filmIdForUpdateCategory').val('');
            $('#filmCategory1').val('');

            // Hide success message after a few seconds
            setTimeout(() => $('#updateFilmResultCategory').fadeOut(), 5000);
        },
        error: function (xhr, status, error) {
            // Handle error response
            const errorMessage = xhr.responseText || error;
            $('#updateFilmResultCategory')
                .removeClass('alert-success')
                .addClass('alert-danger')
                .text(`Failed to update category: ${errorMessage}`)
                .fadeIn();

            console.error('Error updating category:', errorMessage);
        }
    });
});












