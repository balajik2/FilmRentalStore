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
        window.location.href = '/View/login.html'; // Redirect to login page or show login modal
    }
}



document.getElementById('addInventoryBtn').addEventListener('click', function () {
    const token = getAuthToken();
    if (!token) return;

    const requestBody = document.getElementById('inventoryRequestBody').value;
    try {
        const parsedData = JSON.parse(requestBody);

        // Validate required fields in the request
        if (!parsedData.filmId || !parsedData.storeId || !parsedData.lastUpdate) {
            alert("Please provide all required fields (filmId, storeId, lastUpdate).");
            return;
        }

        $.ajax({
            url: 'https://localhost:7239/api/Inventory/add', // URL for adding inventory
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(parsedData),
            headers: {
                Authorization: `Bearer ${token}`
            },
            success: function (response) {
                console.log("POST Request Successful", response);
                const formattedResponse = "Record created successfully"; // Response text based on your server
                document.getElementById('inventoryResponseDetails').textContent = formattedResponse;
                document.getElementById('inventoryResponseDetails').style.color = 'green'; // Show success message in green
            },
            error: function (xhr, status, error) {
                if (xhr.status === 401) {
                    handleAuthError(xhr);
                }
                console.error("POST Request Failed", xhr, status, error);
                alert(`Error: ${xhr.responseText || 'Unable to add inventory'}`);
            }
        });
    } catch (error) {
        alert("Invalid JSON input. Please ensure the request body is properly formatted.");
    }
});



document.getElementById('getInventoryCountBtn').addEventListener('click', function () {
    const token = getAuthToken();
    if (!token) return;

    // Make an AJAX request to fetch the inventory count
    $.ajax({
        url: 'https://localhost:7239/api/Inventory/Count',
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}`,
            accept: '*/*'
        },
        success: function (response) {
            const tbody = document.querySelector("#inventoryCountTable tbody");
            tbody.innerHTML = ""; // Clear previous table data

            if (!response || response.length === 0) {
                alert("No inventory data found.");
                document.getElementById('inventoryCountTable').style.display = 'none';
                return;
            }

            // Loop through the response and populate the table
            response.forEach(item => {
                const row = `
                    <tr>
                        <td>${item.key}</td>
                        <td>${item.value}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            // Display the table
            document.getElementById('inventoryCountTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch inventory count", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch inventory count'}`);
        }
    });
});








document.getElementById('getInventoryByStoreIdBtn').addEventListener('click', function () {
    const storeId = parseInt(document.getElementById('storeid').value.trim(), 10);  // Get the value from the input field and ensure it's a number

    if (isNaN(storeId) || storeId <= 0) {  // Check if the storeId is valid
        alert("Please enter a valid Store ID.");
        return;
    }

    const token = getAuthToken(); // Make sure this function is defined and works to get the auth token
    if (!token) return;

    // Make an AJAX request to fetch the inventory by Store ID
    $.ajax({
        url: `https://localhost:7239/api/Inventory/Films?storeid=${storeId}`,  // Correctly pass the storeId as a query parameter
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}`,
            accept: '*/*'
        },
        success: function (response) {
            const tbody = document.querySelector("#inventoryFilmsTable tbody");
            tbody.innerHTML = ""; // Clear previous table data

            if (!response || response.length === 0) {
                alert("No inventory data found for the given Store ID.");
                document.getElementById('inventoryFilmsTable').style.display = 'none';
                return;
            }

            // Loop through the response and populate the table
            response.forEach(item => {
                const row = `
                    <tr>
                        <td>${item.filmTitle}</td>
                        <td>${item.copies}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            // Display the table
            document.getElementById('inventoryFilmsTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch inventory data", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch inventory data'}`);
        }
    });
});



// Function to fetch JWT token from localStorage
function getAuthToken() {
    const token = localStorage.getItem('jwtToken');
    if (!token) {
        alert('You are not authenticated. Please log in.');
        return null;
    }
    return token;
}

// Handle click event for "Get Inventory" button
document.getElementById('fetch-inventory-btn').addEventListener('click', function () {
    const filmId = document.getElementById('film-id-input').value.trim();  // Get the value from the input field

    // Validate the filmId input
    if (!filmId || isNaN(filmId) || filmId <= 0) {
        alert("Please enter a valid Film ID.");
        return;
    }

    const token = getAuthToken(); // Get authentication token
    if (!token) return;

    // Make an AJAX request to fetch the inventory by Film ID
    $.ajax({
        url: `https://localhost:7239/api/Inventory/Film${filmId}`,  // API endpoint with Film ID as a path parameter
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}`,
            accept: '*/*'
        },
        success: function (response) {
            const tbody = document.querySelector("#film-inventory-table tbody");
            tbody.innerHTML = ""; // Clear previous table data

            if (!response || response.length === 0) {
                alert("No inventory data found for the given Film ID.");
                document.getElementById('film-inventory-table').style.display = 'none';
                return;
            }

            // Loop through the response and populate the table
            response.forEach(item => {
                const row = `
                    <tr>
                        <td>${item.storeAddress}</td>
                        <td>${item.copies}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            // Display the table
            document.getElementById('film-inventory-table').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch inventory data", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch inventory data'}`);
        }
    });
});



document.getElementById('fetch-inventory-btn-new').addEventListener('click', function () {
    const filmId = document.getElementById('film-id-input-new').value.trim();
    const storeId = document.getElementById('store-id-input-new').value.trim();

    // Validate the filmId and storeId inputs
    if (!filmId || isNaN(filmId) || filmId <= 0 || !storeId || isNaN(storeId) || storeId <= 0) {
        alert("Please enter valid Film ID and Store ID.");
        return;
    }

    const token = getAuthToken(); // Function to get the auth token (replace with actual method)
    if (!token) return;

    // Make an AJAX request to fetch the inventory by Film ID and Store ID
    $.ajax({
        url: `https://localhost:7239/api/Inventory/film/${filmId}/store/${storeId}`,  // API endpoint
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}`,
            accept: '*/*'
        },
        success: function (response) {
            const tbody = document.querySelector("#film-inventory-table-new tbody");
            tbody.innerHTML = ""; // Clear previous table data

            if (!response || !response.storeAddress || response.copies === undefined) {
                alert("No inventory data found for the given Film ID and Store ID.");
                document.getElementById('film-inventory-table-new').style.display = 'none';
                return;
            }

            // Populate the table with the response data
            const row = `
                <tr>
                    <td>${response.storeAddress}</td>
                    <td>${response.copies}</td>
                </tr>
            `;
            tbody.innerHTML += row;

            // Display the table
            document.getElementById('film-inventory-table-new').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch inventory data", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch inventory data'}`);
        }
    });
});

