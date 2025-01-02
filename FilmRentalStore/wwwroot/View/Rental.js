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
// Renting a film
document.getElementById('rentButton').addEventListener('click', async () => {
    const customerId = document.getElementById('customerId').value;
    const inventoryId = document.getElementById('inventoryId').value;
    const staffId = document.getElementById('staffId').value;

    if (!customerId || !inventoryId || !staffId) {
        alert('Please fill in all fields.');
        return;
    }
    const rentalDate = new Date().toISOString();
    const returnDate = new Date(); // Set return date to 7 days from rental date
    returnDate.setDate(new Date().getDate() + 7);
    const rentalDTO = {
        customerId: parseInt(customerId),
        inventoryId: parseInt(inventoryId),
        staffId: parseInt(staffId),
        rentalDate: rentalDate,
        returnDate: returnDate.toISOString()
    };

    try {
        const response = await fetch('https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/rental/add', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(rentalDTO)
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.statusText}`);
        }

        alert('Record created successfully!');
    } catch (error) {
        console.error(error);
        alert('An error occurred while creating the record.');
    }
});
// Get Films Rented By Customer
document.getElementById('fetchFilmsButton').addEventListener('click', async () => {
    const customerId = document.getElementById('customerId1').value;

    if (!customerId) {
        alert('Please enter a Customer ID.');
        return;
    }

    try {
        const response = await fetch(`https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/rental/customer/${customerId}`, {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.statusText}`);
        }

        const films = await response.json();

        if (films.length > 0) {
            updateCustomerFilmsTable(films); // Use specific function for customer films table
            alert('Films retrieved successfully!');
        } else {
            alert('No films rented by this customer.');
        }
    } catch (error) {
        console.error(error);
        alert('An error occurred while fetching rented films.');
    }
});

function updateCustomerFilmsTable(films) {
    const tableBody = document.getElementById('filmsTable').querySelector('tbody');
    tableBody.innerHTML = ''; // Clear existing rows

    films.forEach(film => {
        const newRow = document.createElement('tr');
        newRow.innerHTML = `
            <td>${film.filmId}</td>
            <td>${film.title}</td>
            <td>${film.description}</td>
            <td>${film.releaseYear}</td>
            <td>${film.languageId}</td>
            <td>${film.originalLanguageId}</td>
            <td>${film.rentalDuration}</td>
            <td>${film.rentalRate}</td>
            <td>${film.length}</td>
            <td>${film.replacementCost}</td>
            <td>${new Date(film.lastUpdate).toLocaleString()}</td>
        `;
        tableBody.appendChild(newRow);
    });
}

// Top 10 Rented Films
document.getElementById('fetchTopFilmsButton1').addEventListener('click', async () => {
    try {
        const response = await fetch('https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/rental/toptenfilms', {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.statusText}`);
        }

        const topFilms = await response.json();

        if (topFilms.length > 0) {
            updateTopFilmsTable(topFilms); // Use specific function for top films table
            alert('Top 10 films retrieved successfully!');
        } else {
            alert('No data available for top rented films.');
        }
    } catch (error) {
        console.error(error);
        alert('An error occurred while fetching the top rented films.');
    }
});

function updateTopFilmsTable(topFilms) {
    const tableBody = document.getElementById('topFilmsTable').querySelector('tbody');
    tableBody.innerHTML = ''; // Clear existing rows

    topFilms.forEach(film => {
        const newRow = document.createElement('tr');
        newRow.innerHTML = `
            <td>${film.filmId}</td>
            <td>${film.title}</td>
            <td>${film.rentalCount}</td>
        `;
        tableBody.appendChild(newRow);
    });
}

// Get Top 10 film By Store Id
document.getElementById('fetchTop10FilmsBtn').addEventListener('click', function () {
    const storeId = document.getElementById('storeId').value; // Get the store ID from the input field

    if (!storeId) {
        alert('Please enter a valid Store ID.');
        return;
    }

    const apiUrl = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/rental/toptenfilms/store/${storeId}`;
    const table = document.getElementById('top10FilmsTable');
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
                alert('No data found for the specified store.');
                table.style.display = 'none';
                return;
            }

            // Populate table with data
            data.forEach(film => {
                const row = `
                    <tr>
                        <td>${film.filmId}</td>
                        <td>${film.title}</td>
                        <td>${film.rentalCount}</td>
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
            this.innerText = 'Fetch Top 10 Films';
        });
});

// Get Customer with Due By Store Id
document.getElementById('fetchDueCustomersBtn').addEventListener('click', function () {
    const storeId1 = document.getElementById('storeId1').value; // Get the store ID from the input field

    if (!storeId1) {
        alert('Please enter a valid Store ID.');
        return;
    }

    const apiUrl = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/rental/due/store/${storeId1}`;
    const table = document.getElementById('dueCustomersTable');
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
                alert('No customers with due rentals found for the specified store.');
                table.style.display = 'none';
                return;
            }

            // Populate table with data
            data.forEach(customer => {
                const row = `
                    <tr>
                       
                        <td>${customer.storeId}</td>
                        <td>${customer.firstName}</td>
                        <td>${customer.lastName}</td>
                        <td>${customer.email || 'N/A'}</td>
                        <td>${customer.addressId}</td>
                        <td>${customer.active}</td>
                        <td>${new Date(customer.createDate).toLocaleDateString()}</td>
                        <td>${new Date(customer.lastUpdate).toLocaleDateString()}</td>
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
            this.innerText = 'Fetch Customers with Due Rentals';
        });
});

// Update Rental Return Date
document.getElementById('updateReturnDateBtn').addEventListener('click', async function () {
    const rentalId = document.getElementById('rentalId').value.trim();
    const returnDate = document.getElementById('returnDate').value.trim();

    const resultDiv = document.getElementById('updateResult');
    const updatedRentalTable = document.getElementById('updatedRentalTable');
    const tbody = updatedRentalTable.querySelector('tbody');

    // Input validation
    if (!rentalId || !returnDate) {
        resultDiv.textContent = 'Rental ID and Return Date are required!';
        resultDiv.className = 'result-message error';
        resultDiv.style.display = 'block';
        updatedRentalTable.style.display = 'none';
        return;
    }

    // Convert returnDate to ISO format
    const formattedReturnDate = new Date(returnDate).toISOString();

    try {
        // API call
        const response = await fetch(`https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/rental/update/returndate/${rentalId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(formattedReturnDate),
        });

        if (!response.ok) {
            throw new Error(`Failed to update. Server responded with status ${response.status}`);
        }

        const updatedRental = await response.json();

        // Clear previous result
        tbody.innerHTML = '';

        // Populate the table with updated rental data
        const row = `
            <tr>
                <td>${updatedRental.rentalId}</td>
                <td>${updatedRental.rentalDate}</td>
                <td>${updatedRental.inventoryId}</td>
                <td>${updatedRental.customerId}</td>
                <td>${updatedRental.returnDate}</td>
                <td>${updatedRental.staffId}</td>
                <td>${updatedRental.lastUpdate}</td>
            </tr>
        `;
        tbody.innerHTML = row;

        // Show success message and table
        resultDiv.textContent = 'Return date updated successfully!';
        resultDiv.className = 'result-message success';
        resultDiv.style.display = 'block';
        updatedRentalTable.style.display = 'table';
    } catch (error) {
        resultDiv.textContent = `Error: ${error.message}`;
        resultDiv.className = 'result-message error';
        resultDiv.style.display = 'block';
        updatedRentalTable.style.display = 'none';
    }
});





    


