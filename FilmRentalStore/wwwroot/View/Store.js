// Utility function to get the authentication token
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

// Fetch all stores with authentication
document.getElementById('getStoresBtn').addEventListener('click', function () {
    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    $.ajax({
        url: `https://localhost:7239/api/Store`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#storeTable tbody");
            tbody.innerHTML = ""; // Clear existing rows

            if (response.length === 0) {
                alert("No stores found.");
                document.getElementById('storeTable').style.display = 'none';
                return;
            }


            response.forEach(store => {
                const row = `
                    <tr>
                        <td>${store.storeId}</td>
                        <td>${store.addressId}</td>
                        <td>${store.managerStaffId}</td>
                        <td>${store.lastUpdate}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            document.getElementById('storeTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch stores", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch stores'}`);
        }
    });
});


// Add a new store with authentication
document.getElementById('addStoreBtn').addEventListener('click', function () {
    const token = getAuthToken();
    if (!token) return;

    const storeData = JSON.parse(document.getElementById("storeRequestBody").value);

    $.ajax({
        url: `https://localhost:7239/api/Store/post`,
        type: "POST",
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json"
        },
        data: JSON.stringify(storeData),
        success: function (response) {
            document.getElementById("storeResponseDetails").textContent = JSON.stringify(response, null, 2);
            alert("Store added successfully!");
        },
        error: function (xhr, status, error) {
            console.error("Failed to add store", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to add store'}`);
        }
    });
});


// Get stores by city with authentication
document.getElementById('getStoreByCityBtn').addEventListener('click', function () {
    const city = document.getElementById('city').value.trim();
    if (!city) {
        alert("Please enter a city name.");
        return;
    }

    const token = getAuthToken(); // Get the authentication token
    if (!token) {
        alert("No authentication token found.");
        return; // If no token, do not proceed
    }

    // Show loading state (optional)
    showLoadingState();

    $.ajax({
        url: `https://localhost:7239/api/Store/GetByCity?city=${encodeURIComponent(city)}`, // Correctly formatted URL
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}` // Pass the token in the Authorization header
        },
        success: function (response) {
            console.log(response); // Log response to check the data format

            const tbody = document.querySelector("#citySearchStoreTable tbody");
            tbody.innerHTML = ""; // Clear any previous rows

            // Check if the response contains data
            if (response.length === 0) {
                alert("No stores found in the specified city.");
                document.getElementById('citySearchStoreTable').style.display = 'none'; // Hide table if no data
                return;
            }

            // Populate the table with data from the response
            response.forEach(store => {
                const row = `
                    <tr>
                        <td>${store.storeId}</td>
                        <td>${store.addressId}</td>
                        <td>${store.managerStaffId}</td>
                        <td>${store.lastUpdate}</td>
                    </tr>
                `;
                tbody.innerHTML += row; // Add the row to the table body
            });

            // Show the table after populating it with data
            document.getElementById('citySearchStoreTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch store by city", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch store by city'}`);
        },
        complete: function () {
            // Hide loading state (optional)
            hideLoadingState();
        }
    });
});

// Optional: Define loading state functions
function showLoadingState() {
    document.getElementById('getStoreByCountryBtn').innerText = "Loading..."; // Change button text to indicate loading
}

function hideLoadingState() {
    document.getElementById('getStoreByCountryBtn').innerText = "Search by Country"; // Reset button text
}

// Get store details by country name
document.getElementById('getStoreByCountryBtn').addEventListener('click', function () {
    const country = document.getElementById('country').value.trim();
    if (!country) {
        alert("Please enter a country name.");
        return; // If no country, do not proceed
    }

    const token = getAuthToken(); // Assuming you have a function to retrieve the token
    if (!token) {
        alert("No authentication token found.");
        return; // If no token, do not proceed
    }

    // Show loading state
    showLoadingState();

    $.ajax({
        url: `https://localhost:7239/api/Store/GetByCountry?country=${encodeURIComponent(country)}`, // Correct URL format with query parameter
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}` // Pass the token in the headers
        },
        success: function (response) {
            const tbody = document.querySelector("#searchCountryStoreTable tbody");
            tbody.innerHTML = ""; // Clear the table rows before adding new ones

            // Check if no stores are found
            if (response.length === 0) {
                alert("No stores found in this country.");
                document.getElementById('searchCountryStoreTable').style.display = 'none'; // Hide the table if no data
                hideLoadingState(); // Hide loading state
                return;
            }

            // Populate the table with the response data
            response.forEach(store => {
                const row = `
                    <tr>
                        <td>${store.storeId}</td>
                        <td>${store.managerStaffId}</td>
                        <td>${store.addressId}</td> 
                        <td>${store.lastUpdate}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            // Display the table after populating it with data
            document.getElementById('searchCountryStoreTable').style.display = 'table';

            // Hide loading state after processing the data
            hideLoadingState();
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch stores by country", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch store by country'}`);

            // Hide loading state in case of an error
            hideLoadingState();
        }
    });
});

// Get all staff details by storeId
document.getElementById('getStaffByStoreBtn').addEventListener('click', function () {
    const storeId = document.getElementById('storeId').value.trim();
    if (!storeId) {
        alert("Please enter a valid store ID.");
        return; // If no storeId, do not proceed
    }

    const token = getAuthToken(); // Assuming you have a function to retrieve the token
    if (!token) {
        alert("No authentication token found.");
        return; // If no token, do not proceed
    }

    // Show loading state
    showLoadingState();

    $.ajax({
        url: `https://localhost:7239/api/Store/AllStaffOfStore?storeid=${encodeURIComponent(storeId)}`, // Correct URL format with storeId parameter
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}` // Pass the token in the headers
        },
        success: function (response) {
            const tbody = document.querySelector("#staffTable tbody");
            tbody.innerHTML = ""; // Clear the table rows before adding new ones

            // Check if no staff are found
            if (response.length === 0) {
                alert("No staff found for this store.");
                document.getElementById('staffTable').style.display = 'none'; // Hide the table if no data
                hideLoadingState(); // Hide loading state
                return;
            }

            // Populate the table with the response data
            response.forEach(staff => {
                const row = `
                    <tr>
                        <td>${staff.staffId}</td>
                        <td>${staff.firstName}</td>
                        <td>${staff.lastName}</td>
                        <td>${staff.addressId}</td>
                        <td>${staff.email}</td>
                        <td>${staff.active}</td>
                        <td>${staff.lastUpdate}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            // Display the table after populating it with data
            document.getElementById('staffTable').style.display = 'table';

            // Hide loading state after processing the data
            hideLoadingState();
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch staff by store", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch staff by store'}`);

            // Hide loading state in case of an error
            hideLoadingState();
        }
    });
});


//get all customer details by store id
// Event listener for button click
document.getElementById('getCustomersBtn').addEventListener('click', function () {
    const storeId = document.getElementById('storeId1').value.trim();  // Getting the store ID value

    if (!storeId) {
        alert("Please enter a store ID.");
        return;
    }

    const token = getAuthToken();  // Get token from your method
    if (!token) {
        alert("No authentication token found.");
        return;
    }

    // Logging the token for debugging purposes
    console.log("Token:", token);

    // Ensure storeId is encoded correctly
    const encodedStoreId = encodeURIComponent(storeId);

    // Sending AJAX request
    $.ajax({
        url: `https://localhost:7239/api/Store/Allcustomers?storeid=${encodedStoreId}`,  // Corrected URL with encoded storeId
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`  // Send token in headers
        },
        success: function (response) {
            console.log("Response:", response);  // Log response to check data structure

            const tbody = document.querySelector("#customersTable tbody");
            tbody.innerHTML = "";  // Clear any existing rows in the table

            if (Array.isArray(response) && response.length === 0) {
                alert("No customers found for the specified store.");
                document.getElementById('customersTable').style.display = 'none';  // Hide table if no customers found
                return;
            }

            // Populate the table with customer data
            response.forEach(customer => {
                const row = `
                    <tr>
                        <td>${customer.customerId}</td>
                        <td>${customer.storeId}</td>
                        <td>${customer.firstName}</td>
                        <td>${customer.lastName}</td>
                        <td>${customer.email}</td>
                        <td>${customer.addressId}</td>
                        <td>${customer.active}</td>
                        <td>${customer.createDate}</td>
                        <td>${customer.lastUpdate}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            // Display the table after populating it with data
            document.getElementById('customersTable').style.display = 'table';  // Show the table
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch customers", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch customer details'}`);  // Alert on error
        }
    });
});





//get all manager details by store id

document.getElementById('getmanagerByStoreBtn').addEventListener('click', function () {
    const storeId2 = document.getElementById('storeId2').value.trim();
    if (!storeId2) {
        alert("Please enter a valid store ID.");
        return; // If no storeId, do not proceed
    }

    const token = getAuthToken(); // Assuming you have a function to retrieve the token
    if (!token) {
        alert("No authentication token found.");
        return; // If no token, do not proceed
    }

    // Show loading state
    showLoadingState();

    $.ajax({
        url: `https://localhost:7239/api/Store/managerdetails?storeid=${encodeURIComponent(storeId2)}`, // Correct URL format with storeId parameter
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}` // Pass the token in the headers
        },
        success: function (response) {
            const tbody = document.querySelector("#managerTable tbody");
            tbody.innerHTML = ""; // Clear the table rows before adding new ones

            // Check if no staff are found
            if (response.length === 0) {
                alert("No manager found for this store.");
                document.getElementById('managerTable').style.display = 'none'; // Hide the table if no data
                hideLoadingState(); // Hide loading state
                return;
            }

            // Populate the table with the response data
            response.forEach(staff => {
                const row = `
                    <tr>
                        <td>${staff.staffId}</td>
                        <td>${staff.firstName}</td>
                        <td>${staff.lastName}</td>
                        <td>${staff.addressId}</td>
                        <td>${staff.email}</td>
                        <td>${staff.active}</td>
                        <td>${staff.lastUpdate}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            // Display the table after populating it with data
            document.getElementById('managerTable').style.display = 'table';

            // Hide loading state after processing the data
            hideLoadingState();
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch manager by store", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch manager by store'}`);

            // Hide loading state in case of an error
            hideLoadingState();
        }
    });
});

//get all manager details
document.getElementById('getManagerBtn').addEventListener('click', function () {
    const token = getAuthToken();
    if (!token) {
        alert("No authentication token found.");
        return;
    }

    console.log("Token:", token);

    $.ajax({
        url: `https://localhost:7239/api/store/managers`,  // API endpoint for fetching manager details
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`  // Send token in headers
        },
        success: function (response) {
            console.log("Response:", response);  // Log the entire response to check its structure

            const tbody = document.querySelector("#managerdetails tbody");
            tbody.innerHTML = "";  // Clear any existing rows

            if (!response) {
                alert("No manager data received.");
                document.getElementById('managerdetails').style.display = 'none';
                return;
            }

            // Example: Assuming response is an object with the manager details
            response.forEach(staff => {
                const row = `
                        <tr>
                            <td>${staff.firstName}</td>
                            <td>${staff.lastName}</td>
                            <td>${staff.email}</td>
                            <td>${staff.phone}</td>
                            <td>${staff.address1}</td>
                            <td>${staff.address2}</td>
                            <td>${staff.city1}</td>
                        </tr>
                    `;
                tbody.innerHTML += row;

            });
            document.getElementById('managerdetails').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch manager details", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch manager details'}`);  // Show error message

            // Log the response text for debugging purposes
            if (xhr.responseText) {
                console.log("Error Response Text:", xhr.responseText);
            }
        }
    });
});







$(document).ready(function () {
    // Handle form submission
    $('#updatePhoneForm').submit(function (e) {
        e.preventDefault(); // Prevent the default form submission behavior

        const storeId = $('#storeId0').val().trim();
        const newPhoneNumber = $('#newPhoneNumber').val().trim();

        // Validate input fields
        if (!storeId || !newPhoneNumber) {
            alert("Please enter both Store ID and New Phone Number.");
            return;
        }

        const token = getAuthToken();
        if (!token) {
            alert("Authorization token is missing.");
            return;
        }

        // Prepare the URL with query parameters for the PUT request
        const url = `https://localhost:7239/api/Store/updatephone?storeid=${encodeURIComponent(storeId)}&phone=${encodeURIComponent(newPhoneNumber)}`;

        console.log('Sending PUT request to:', url); // Debugging log to ensure the URL is correct
        console.log('Using token:', token); // Debugging log to ensure the token is correct

        // Perform AJAX request
        $.ajax({
            url: url,
            type: 'PUT',
            headers: {
                Authorization: `Bearer ${token}`, // Authorization token for the API
                "accept": "*/*", // Accept any response type
            },
            success: function (response) {
                // Handle successful response
                console.log('Response:', response); // Log the successful response
                const messageElement = document.getElementById('updateStatusMessage');
                messageElement.textContent = "Phone number updated successfully!";
                messageElement.style.display = 'block';
                messageElement.classList.remove('text-danger');
                messageElement.classList.add('text-success');
            },
            error: function (xhr, status, error) {
                // Handle error response
                console.error("AJAX Error:", xhr, status, error); // Log the error for debugging

                // Check if the error response has more information
                const errorMessage = xhr.responseText || 'Unable to update phone number. Please try again later.';
                const messageElement = document.getElementById('updateStatusMessage');
                messageElement.textContent = `Error: ${errorMessage}`;
                messageElement.style.display = 'block';
                messageElement.classList.remove('text-success');
                messageElement.classList.add('text-danger');
            }
        });
    });
});

