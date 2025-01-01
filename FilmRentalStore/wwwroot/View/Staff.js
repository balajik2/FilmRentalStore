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


function showLoadingState(button) {

    // Show loading state

    document.getElementById('loadingState').style.display = 'block';

    button.disabled = true;

}

function hideLoadingState(button, buttonText) {

    // Hide loading state

    document.getElementById('loadingState').style.display = 'none';

    button.disabled = false;

    button.textContent = buttonText;

}

function handleAuthError(xhr) {

    if (xhr.status === 401) {

        alert('Authentication expired. Please log in again.');

        localStorage.removeItem('jwtToken');

        window.location.href = '/View/login.html'; // Redirect to login page or show login modal

    }

}


//get all staff

document.getElementById('getStaffBtn').addEventListener('click', function () {

    const token = getAuthToken();

    if (!token) {

        alert("Authorization token missing!");

        return;

    }

    const button = this;

    button.disabled = true; // Disable the button while fetching data

    button.textContent = 'Loading...'; // Update the button text to indicate the process

    // Make the AJAX request

    let xhr = new XMLHttpRequest();

    xhr.open("GET", "https://localhost:7239/api/Staff/GetStaff", true);

    xhr.setRequestHeader("Authorization", `Bearer ${token}`);

    xhr.onreadystatechange = function () {

        if (xhr.readyState === 4) {

            button.disabled = false; // Enable the button again

            button.textContent = 'Get Staff'; // Restore original text

            if (xhr.status === 200) {

                let staffMembers = JSON.parse(xhr.responseText);

                let tbody = document.querySelector("#staffTable tbody");

                tbody.innerHTML = '';

                if (staffMembers.length === 0) {

                    alert('No staff found.');

                    document.getElementById('staffTable').style.display = 'none';

                    return;

                }

                staffMembers.forEach(staff => {

                    let row = `
<tr>
<td>${staff.staffId}</td>
<td>${staff.firstName}</td>
<td>${staff.lastName}</td>
<td>${staff.addressId}</td>
<td>${staff.email}</td>
<td>${staff.storeId}</td>
<td>${staff.active}</td>
<td>${staff.lastUpdate}</td>
<td>${staff.urlPath}</td>
</tr>

                    `;

                    tbody.innerHTML += row;

                });

                document.getElementById('staffTable').style.display = 'table';

            } else {

                alert(`Error: ${xhr.responseText || xhr.statusText || 'Unable to fetch staff data'}`);

            }

        }

    };

    xhr.send();

});



// Add a customer with authentication

document.getElementById('addStaffBtn').addEventListener('click', function () {

    const token = getAuthToken();

    if (!token) return;

    const requestBody = document.getElementById('requestBody').value;

    try {

        const parsedData = JSON.parse(requestBody);

        $.ajax({

            url: 'https://localhost:7239/api/Staff/AddStaff',

            type: 'POST',

            contentType: 'application/json',

            data: JSON.stringify(parsedData),

            headers: {

                Authorization: `Bearer ${token}`

            },

            success: function (response) {

                console.log("POST Request Successful", response);

                const formattedResponse = JSON.stringify(response, null, 2);

                document.getElementById('responseDetails').textContent = formattedResponse;

                // Optionally refresh the customer list after adding

                document.getElementById('addStaffBtn').click();

            },

            error: function (xhr, status, error) {

                if (xhr.status === 401) {

                    handleAuthError(xhr);

                }

                console.error("POST Request Failed", xhr, status, error);

                alert(`Error: ${xhr.responseText || 'Unable to add customer'}`);

            }

        });

    } catch (error) {

        alert("Invalid JSON input. Please ensure the request body is properly formatted.");

    }

});




//Get staff by last name

document.getElementById('getStaffByLastNameBtn').addEventListener('click', function () {

    const lastName = document.getElementById('lastName').value.trim();

    if (!lastName) {

        alert("Please enter a last name.");

        return;

    }

    const token = getAuthToken();

    if (!token) {

        alert("Authentication token is missing.");

        return;

    }

    showLoadingState(this);

    $.ajax({

        url: `https://localhost:7239/api/Staff/GetStaffByLastName?lastname=${encodeURIComponent(lastName)}`,

        type: "GET",

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function (response) {

            hideLoadingState(document.getElementById('getStaffByLastNameBtn'), 'Get Staff By Last Name');

            const tbody = document.querySelector("#searchStaffTable tbody");

            tbody.innerHTML = "";

            if (!response || response.length === 0) {

                alert("No staff found with the last name.");

                document.getElementById('searchStaffTable').style.display = 'none';

                return;

            }

            response.forEach(staff => {

                const row = `
<tr>
<td>${staff.staffId}</td>
<td>${staff.firstName}</td>
<td>${staff.lastName}</td>
<td>${staff.addressId}</td>
<td>${staff.email}</td>
<td>${staff.storeId}</td>
<td>${staff.active}</td>
<td>${staff.lastUpdate}</td>
<td>${staff.urlPath}</td>
</tr>

                `;

                tbody.innerHTML += row;

            });

            document.getElementById('searchStaffTable').style.display = 'table';

        },

        error: function (xhr, status, error) {

            hideLoadingState(document.getElementById('getStaffByLastNameBtn'), 'Get Staff By Last Name');

            console.error("Error Details:", xhr.responseText || status || error);

            alert("Failed to fetch staff by last name.");

        }

    });

});

// Mock functions for demo purposes (replace with actual implementations)

function getAuthToken() {

    // Replace with your token retrieval logic

    return "your_auth_token_here";

}

function showLoadingState(button) {

    button.disabled = true;

    button.innerText = "Loading...";

}

function hideLoadingState(button, originalText) {

    button.disabled = false;

    button.innerText = originalText;

}


//get staff by first name

document.getElementById('getStaffByFirstNameBtn').addEventListener('click', function () {

    const firstName = document.getElementById('firstName').value.trim();

    if (!firstName) {

        alert("Please enter a first name.");

        return;

    }

    const token = getAuthToken();

    if (!token) {

        alert("Authentication token is missing.");

        return;

    }

    showLoadingState(this);

    $.ajax({

        url: `https://localhost:7239/api/Staff/GetStaffByFirstName?firstname=${encodeURIComponent(firstName)}`,

        type: "GET",

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function (response) {

            hideLoadingState(document.getElementById('getStaffByFirstNameBtn'), 'Get Staff By First Name');

            const tbody = document.querySelector("#searchStaffTable tbody");

            tbody.innerHTML = "";

            if (!response || response.length === 0) {

                alert("No staff found with the first name.");

                document.getElementById('searchStaffTable').style.display = 'none';

                return;

            }

            response.forEach(staff => {

                const row = `
<tr>
<td>${staff.staffId}</td>
<td>${staff.firstName}</td>
<td>${staff.lastName}</td>
<td>${staff.addressId}</td>
<td>${staff.email}</td>
<td>${staff.storeId}</td>
<td>${staff.active}</td>
<td>${staff.lastUpdate}</td>
<td>${staff.urlPath}</td>
</tr>

                `;

                tbody.innerHTML += row;

            });

            document.getElementById('searchStaffTable').style.display = 'table';

        },

        error: function (xhr, status, error) {

            hideLoadingState(document.getElementById('getStaffByFirstNameBtn'), 'Get Staff By First Name');

            console.error("Error Details:", xhr.responseText || status || error);

            alert("Failed to fetch staff by first name.");

        }

    });

});



document.getElementById('updateStaffAddressBtn').addEventListener('click', function () {

    const staffId = document.getElementById('updateStaffIdAddress').value.trim();

    const newAddressId = document.getElementById('updateAddressId').value.trim();

    if (!staffId || !newAddressId) {

        alert("Please provide both Staff ID and new Address ID.");

        return;

    }

    const token = getAuthToken(); // Assuming you have a function to retrieve the auth token

    if (!token) return; // If no token, do not proceed

    const url = `https://localhost:7239/api/Staff/UpdateAddressById/${encodeURIComponent(staffId)}?addressId=${encodeURIComponent(newAddressId)}`;

    $.ajax({

        url: url,

        type: 'PUT',

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function () {

            console.log("Staff address ID updated successfully.");

            fetchUpdatedStaffDetailsByAddress(newAddressId); // Fetch and display updated details

        },

        error: function (xhr, status, error) {

            console.error("Failed to update staff address ID", xhr, status, error);

            alert(`Error: ${xhr.responseText || 'Unable to update address ID'}`);

        }

    });

});

// Fetch updated staff details by address ID and display in the table

function fetchUpdatedStaffDetailsByAddress(addressId) {

    const token = getAuthToken(); // Assuming you have a function to retrieve the auth token

    if (!token) return; // If no token, do not proceed

    $.ajax({

        url: `https://localhost:7239/api/Staff/address/${encodeURIComponent(addressId)}`,

        type: "GET",

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function (response) {

            const tbody = document.querySelector("#updatedStaffAddressTable tbody");

            tbody.innerHTML = ""; // Clear existing rows

            if (!response || response.length === 0) {

                alert("No staff found with that address ID.");

                document.getElementById('updatedStaffAddressTable').style.display = 'none';

                return;

            }

            const staff = response[0]; // Assuming API returns an array with at least one staff

            const row = `
<tr>
<td>${staff.staffId}</td>
<td>${staff.firstName}</td>
<td>${staff.lastName}</td>
<td>${staff.email}</td>
<td>${staff.addressId}</td>
<td>${staff.storeId}</td>
<td>${staff.active ? 'Yes' : 'No'}</td>
<td>${staff.lastUpdate}</td>
<td>${staff.urlPath}</td>
</tr>

            `;

            tbody.innerHTML += row;

            document.getElementById('updatedStaffAddressTable').style.display = 'table';

        },

        error: function (xhr, status, error) {

            console.error("Failed to fetch updated staff details", xhr, status, error);

            alert(`Error: ${xhr.responseText || 'Unable to fetch updated staff details'}`);

        }

    });

}


document.getElementById('getStaffByCityBtn').addEventListener('click', function () {

    const city = document.getElementById('city').value.trim();

    if (!city) {

        alert("Please enter a city.");

        return;

    }

    const token = getAuthToken();

    if (!token) {

        alert("Authentication token is missing.");

        return;

    }

    showLoadingState(this);

    $.ajax({

        url: `https://localhost:7239/api/Staff/GetStaffByCity?city=${encodeURIComponent(city)}`,

        type: "GET",

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function (response) {

            hideLoadingState(document.getElementById('getStaffByCityBtn'), 'Get Staff By City');

            const tbody = document.querySelector("#searchStaffTable tbody");

            tbody.innerHTML = "";

            if (!response || response.length === 0) {

                alert("No staff found in the specified city.");

                document.getElementById('searchStaffTable').style.display = 'none';

                return;

            }

            response.forEach(staff => {

                const row = `
<tr>
<td>${staff.staffId}</td>
<td>${staff.firstName}</td>
<td>${staff.lastName}</td>
<td>${staff.addressId}</td>
<td>${staff.email}</td>
<td>${staff.storeId}</td>
<td>${staff.active}</td>
<td>${staff.lastUpdate}</td>
<td>${staff.urlPath}</td>
</tr>

                `;

                tbody.innerHTML += row;

            });

            document.getElementById('searchStaffTable').style.display = 'table';

        },

        error: function (xhr, status, error) {

            hideLoadingState(document.getElementById('getStaffByCityBtn'), 'Get Staff By City');

            console.error("Error Details:", xhr.responseText || status || error);

            alert("Failed to fetch staff by city.");

        }

    });

});


document.getElementById('getStaffByCountryBtn').addEventListener('click', function () {

    const country = document.getElementById('staffCountry').value.trim();

    if (!country) {

        alert("Please enter a country.");

        return;

    }

    const token = getAuthToken(); // Assuming you have a function to retrieve the auth token

    if (!token) return; // If no token, do not proceed

    const url = `https://localhost:7239/api/Staff/GetStaffByCountry/${encodeURIComponent(country)}`;

    // Show loading state (optional, can use a spinner or message)

    showLoadingState(this);

    $.ajax({

        url: url,

        type: 'GET',

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function (response) {

            hideLoadingState(document.getElementById('getStaffByCountryBtn'), 'Get Staff by Country');

            const tbody = document.querySelector("#staffCountryTable tbody");

            tbody.innerHTML = ""; // Clear previous search results

            if (!response || response.length === 0) {

                alert("No staff found in the given country.");

                document.getElementById('staffCountryTable').style.display = 'none';

                return;

            }

            // Loop through each staff and add to the table

            response.forEach(staff => {

                const row = `
<tr>
<td>${staff.staffId}</td>
<td>${staff.firstName}</td>
<td>${staff.lastName}</td>
<td>${staff.email}</td>
<td>${staff.phoneNumber}</td>
<td>${staff.addressId}</td>
<td>${staff.storeId}</td>
<td>${staff.active ? 'Yes' : 'No'}</td>
<td>${staff.lastUpdate}</td>
<td>${staff.urlPath}</td>
</tr>

                `;

                tbody.innerHTML += row; // Add staff row to the table

            });

            // Show the table once staff records are available

            document.getElementById('staffCountryTable').style.display = 'table';

        },

        error: function (xhr, status, error) {

            hideLoadingState(document.getElementById('getStaffByCountryBtn'), 'Get Staff by Country');

            console.error("Error Details:", xhr.responseText || status || error);

            alert("Failed to fetch staff by country.");

        }

    });

});


document.getElementById('getStaffByEmailBtn').addEventListener('click', function () {

    const email = document.getElementById('email').value.trim();

    // Validate email input

    if (!email) {

        alert("Please enter an email address.");

        return;

    }

    const token = getAuthToken(); // Assuming you have a function to retrieve the auth token

    if (!token) {

        alert("Authentication token is missing.");

        return;

    }

    showLoadingState(this); // Show loading spinner or message

    // AJAX request to fetch staff by email

    $.ajax({

        url: `https://localhost:7239/api/Staff/GetStaffByEmail?email=${encodeURIComponent(email)}`,

        type: "GET",

        headers: {

            Authorization: `Bearer ${token}` // Send the authentication token

        },

        success: function (response) {

            hideLoadingState(document.getElementById('getStaffByEmailBtn'), 'Get Staff By Email'); // Hide loading spinner

            const tbody = document.querySelector("#searchStaffTable tbody");

            tbody.innerHTML = ""; // Clear previous search results

            // Check if response is empty or null

            if (!response || response.length === 0) {

                alert("No staff found with the given email.");

                document.getElementById('searchStaffTable').style.display = 'none'; // Hide table if no results

                return;

            }

            // Loop through each staff and add to the table

            response.forEach(staff => {

                const row = `
<tr>
<td>${staff.staffId}</td>
<td>${staff.firstName}</td>
<td>${staff.lastName}</td>
<td>${staff.addressId}</td>
<td>${staff.email}</td>
<td>${staff.storeId}</td>
<td>${staff.active}</td>
<td>${staff.lastUpdate}</td>
<td>${staff.urlPath}</td>
</tr>

                `;

                tbody.innerHTML += row; // Add staff row to the table

            });

            // Show the table once staff records are available

            document.getElementById('searchStaffTable').style.display = 'table';

        },

        error: function (xhr, status, error) {

            hideLoadingState(document.getElementById('getStaffByEmailBtn'), 'Get Staff By Email');

            console.error("Error Details:", xhr.responseText || status || error);

            alert("Failed to fetch staff by email.");

        }

    });

});



document.getElementById('getStaffByPhoneBtn').addEventListener('click', function () {

    const phoneNumber = document.getElementById('staffPhoneNumber').value.trim();

    if (!phoneNumber) {

        alert("Please enter a phone number.");

        return;

    }

    const token = getAuthToken(); // Assuming you have a function to retrieve the auth token

    if (!token) return; // If no token, do not proceed

    const url = `https://localhost:7239/api/Staff/GetStaffByPhone/${encodeURIComponent(phoneNumber)}`;

    // Show loading state (optional, can use a spinner or message)

    showLoadingState(this);

    $.ajax({

        url: url,

        type: 'GET',

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function (response) {

            hideLoadingState(document.getElementById('getStaffByPhoneBtn'), 'Get Staff by Phone');

            const tbody = document.querySelector("#staffPhoneTable tbody");

            tbody.innerHTML = ""; // Clear previous search results

            if (!response || response.length === 0) {

                alert("No staff found with the given phone number.");

                document.getElementById('staffPhoneTable').style.display = 'none';

                return;

            }

            // Loop through each staff and add to the table

            response.forEach(staff => {

                const row = `
<tr>
<td>${staff.staffId}</td>
<td>${staff.firstName}</td>
<td>${staff.lastName}</td>
<td>${staff.email}</td>
<td>${staff.phoneNumber}</td>
<td>${staff.addressId}</td>
<td>${staff.storeId}</td>
<td>${staff.active ? 'Yes' : 'No'}</td>
<td>${staff.lastUpdate}</td>
<td>${staff.urlPath}</td>
</tr>

                `;

                tbody.innerHTML += row; // Add staff row to the table

            });

            // Show the table once staff records are available

            document.getElementById('staffPhoneTable').style.display = 'table';

        },

        error: function (xhr, status, error) {

            hideLoadingState(document.getElementById('getStaffByPhoneBtn'), 'Get Staff by Phone');

            console.error("Error Details:", xhr.responseText || status || error);

            alert("Failed to fetch staff by phone number.");

        }

    });

});


document.getElementById('updateStaffLastNameBtn').addEventListener('click', function () {

    const staffId = document.getElementById('updateStaffIdLastName').value.trim();

    const newLastName = document.getElementById('updateLastName').value.trim();

    if (!staffId || !newLastName) {

        alert("Please provide both Staff ID and new last name.");

        return;

    }

    const token = getAuthToken(); // Assuming you have a function to retrieve the auth token

    if (!token) return; // If no token, do not proceed

    const url = `https://localhost:7239/api/Staff/UpdateLastNameById/${encodeURIComponent(staffId)}?lastName=${encodeURIComponent(newLastName)}`;

    $.ajax({

        url: url,

        type: 'PUT',

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function () {

            console.log("Staff last name updated successfully.");

            fetchUpdatedStaffDetailsByLastName(newLastName); // Fetch and display updated details

        },

        error: function (xhr, status, error) {

            console.error("Failed to update staff last name", xhr, status, error);

            alert(`Error: ${xhr.responseText || 'Unable to update last name'}`);

        }

    });

});

// Fetch updated staff details by last name and display in the table

function fetchUpdatedStaffDetailsByLastName(lastName) {

    const token = getAuthToken(); // Assuming you have a function to retrieve the auth token

    if (!token) return; // If no token, do not proceed

    $.ajax({

        url: `https://localhost:7239/api/Staff/lastname/${encodeURIComponent(lastName)}`,

        type: "GET",

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function (response) {

            const tbody = document.querySelector("#updatedStaffLastNameTable tbody");

            tbody.innerHTML = ""; // Clear existing rows

            if (!response || response.length === 0) {

                alert("No staff found with that last name.");

                document.getElementById('updatedStaffLastNameTable').style.display = 'none';

                return;

            }

            const staff = response[0]; // Assuming API returns an array with at least one staff

            const row = `
<tr>
<td>${staff.staffId}</td>
<td>${staff.firstName}</td>
<td>${staff.lastName}</td>
<td>${staff.email}</td>
<td>${staff.addressId}</td>
<td>${staff.storeId}</td>
<td>${staff.active}</td>
<td>${staff.lastUpdate}</td>
<td>${staff.urlPath}</td>
</tr>

            `;

            tbody.innerHTML += row;

            document.getElementById('updatedStaffLastNameTable').style.display = 'table';

        },

        error: function (xhr, status, error) {

            console.error("Failed to fetch updated staff details", xhr, status, error);

            alert(`Error: ${xhr.responseText || 'Unable to fetch updated staff details'}`);

        }

    });

}



document.getElementById('updateStaffFirstNameBtn').addEventListener('click', function () {

    const staffId = document.getElementById('updateStaffIdFirstName').value.trim();

    const newFirstName = document.getElementById('updateFirstName').value.trim();

    if (!staffId || !newFirstName) {

        alert("Please provide both Staff ID and new first name.");

        return;

    }

    const token = getAuthToken(); // Assuming you have a function to retrieve the auth token

    if (!token) return; // If no token, do not proceed

    const url = `https://localhost:7239/api/Staff/UpdateFirstNameById/${encodeURIComponent(staffId)}?firstName=${encodeURIComponent(newFirstName)}`;

    $.ajax({

        url: url,

        type: 'PUT',

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function () {

            console.log("Staff first name updated successfully.");

            fetchUpdatedStaffDetailsByFirstName(newFirstName); // Fetch and display updated details

        },

        error: function (xhr, status, error) {

            console.error("Failed to update staff first name", xhr, status, error);

            alert(`Error: ${xhr.responseText || 'Unable to update first name'}`);

        }

    });

});

// Fetch updated staff details by first name and display in the table

function fetchUpdatedStaffDetailsByFirstName(firstName) {

    const token = getAuthToken(); // Assuming you have a function to retrieve the auth token

    if (!token) return; // If no token, do not proceed

    $.ajax({

        url: `https://localhost:7239/api/Staff/firstname/${encodeURIComponent(firstName)}`,

        type: "GET",

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function (response) {

            const tbody = document.querySelector("#updatedStaffFirstNameTable tbody");

            tbody.innerHTML = ""; // Clear existing rows

            if (!response || response.length === 0) {

                alert("No staff found with that first name.");

                document.getElementById('updatedStaffFirstNameTable').style.display = 'none';

                return;

            }

            const staff = response[0]; // Assuming API returns an array with at least one staff

            const row = `
<tr>
<td>${staff.staffId}</td>
<td>${staff.firstName}</td>
<td>${staff.lastName}</td>
<td>${staff.email}</td>
<td>${staff.addressId}</td>
<td>${staff.storeId}</td>
<td>${staff.active}</td>
<td>${staff.lastUpdate}</td>
<td>${staff.urlPath}</td>
</tr>

            `;

            tbody.innerHTML += row;

            document.getElementById('updatedStaffFirstNameTable').style.display = 'table';

        },

        error: function (xhr, status, error) {

            console.error("Failed to fetch updated staff details", xhr, status, error);

            alert(`Error: ${xhr.responseText || 'Unable to fetch updated staff details'}`);

        }

    });

}


document.getElementById('updateStaffEmailBtn').addEventListener('click', function () {

    const staffId = document.getElementById('updateStaffIdEmail').value.trim();

    const newEmail = document.getElementById('updateEmail').value.trim();

    if (!staffId || !newEmail) {

        alert("Please provide both Staff ID and new email.");

        return;

    }

    const token = getAuthToken(); // Assuming you have a function to retrieve the auth token

    if (!token) return; // If no token, do not proceed

    const url = `https://localhost:7239/api/Staff/UpdateEmailById/${encodeURIComponent(staffId)}?email=${encodeURIComponent(newEmail)}`;

    $.ajax({

        url: url,

        type: 'PUT',

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function () {

            console.log("Staff email updated successfully.");

            fetchUpdatedStaffDetailsByEmail(newEmail); // Fetch and display updated details

        },

        error: function (xhr, status, error) {

            console.error("Failed to update staff email", xhr, status, error);

            alert(`Error: ${xhr.responseText || 'Unable to update email'}`);

        }

    });

});

// Fetch updated staff details by email and display in the table

function fetchUpdatedStaffDetailsByEmail(email) {

    const token = getAuthToken(); // Assuming you have a function to retrieve the auth token

    if (!token) return; // If no token, do not proceed

    $.ajax({

        url: `https://localhost:7239/api/Staff/email/${encodeURIComponent(email)}`,

        type: "GET",

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function (response) {

            const tbody = document.querySelector("#updatedStaffEmailTable tbody");

            tbody.innerHTML = ""; // Clear existing rows

            if (!response || response.length === 0) {

                alert("No staff found with that email.");

                document.getElementById('updatedStaffEmailTable').style.display = 'none';

                return;

            }

            const staff = response[0]; // Assuming API returns an array with at least one staff

            const row = `
<tr>
<td>${staff.staffId}</td>
<td>${staff.firstName}</td>
<td>${staff.lastName}</td>
<td>${staff.email}</td>
<td>${staff.addressId}</td>
<td>${staff.storeId}</td>
<td>${staff.active}</td>
<td>${staff.lastUpdate}</td>
<td>${staff.urlPath}</td>
</tr>

            `;

            tbody.innerHTML += row;

            document.getElementById('updatedStaffEmailTable').style.display = 'table';

        },

        error: function (xhr, status, error) {

            console.error("Failed to fetch updated staff details", xhr, status, error);

            alert(`Error: ${xhr.responseText || 'Unable to fetch updated staff details'}`);

        }

    });

}


document.getElementById('updateStoreIdBtn').addEventListener('click', function () {

    const staffId = document.getElementById('updateStaffIdStoreId').value.trim();

    const newStoreId = document.getElementById('updateStoreId').value.trim();

    if (!staffId || !newStoreId) {

        alert("Please provide both Staff ID and new Store ID.");

        return;

    }

    const token = getAuthToken(); // Assuming you have a function to retrieve the auth token

    if (!token) return; // If no token, do not proceed

    const url = `https://localhost:7239/api/Staff/UpdateStoreIdByStaffId/${encodeURIComponent(staffId)}?storeId=${encodeURIComponent(newStoreId)}`;

    // Show loading state (optional, can use a spinner or message)

    showLoadingState(this);

    $.ajax({

        url: url,

        type: 'PUT',

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function () {

            console.log("Staff's store ID updated successfully.");

            fetchUpdatedStaffDetailsById(staffId); // Fetch the updated staff details by Staff ID

        },

        error: function (xhr, status, error) {

            hideLoadingState(document.getElementById('updateStoreIdBtn'), 'Update Store ID');

            console.error("Failed to update store ID", xhr, status, error);

            alert(`Error: ${xhr.responseText || 'Unable to update store ID'}`);

        }

    });

});

// Fetch updated staff details by staff ID and display in the table

function fetchUpdatedStaffDetailsById(staffId) {

    const token = getAuthToken();

    if (!token) return; // If no token, do not proceed

    $.ajax({

        url: `https://localhost:7239/api/Staff/GetStaffById/${encodeURIComponent(staffId)}`,

        type: "GET",

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function (response) {

            const tbody = document.querySelector("#updatedStaffStoreIdTable tbody");

            tbody.innerHTML = ""; // Clear existing rows

            if (!response) {

                alert("No staff found with that ID.");

                document.getElementById('updatedStaffStoreIdTable').style.display = 'none';

                return;

            }

            const staff = response; // Assuming the API returns a single staff member object

            const row = `
<tr>
<td>${staff.staffId}</td>
<td>${staff.firstName}</td>
<td>${staff.lastName}</td>
<td>${staff.email}</td>
<td>${staff.storeId}</td>
<td>${staff.active}</td>
<td>${staff.lastUpdate}</td>
</tr>

            `;

            tbody.innerHTML += row;

            document.getElementById('updatedStaffStoreIdTable').style.display = 'table';

        },

        error: function (xhr, status, error) {

            console.error("Failed to fetch updated staff details", xhr, status, error);

            alert(`Error: ${xhr.responseText || 'Unable to fetch updated staff details'}`);

        }

    });

}



document.getElementById('updatePhoneNumberBtn').addEventListener('click', function () {

    const staffId = document.getElementById('updateStaffIdPhoneNumber').value.trim();

    const newPhoneNumber = document.getElementById('updatePhoneNumber').value.trim();

    if (!staffId || !newPhoneNumber) {

        alert("Please provide both Staff ID and new phone number.");

        return;

    }

    const token = getAuthToken(); // Assuming you have a function to retrieve the auth token

    if (!token) return; // If no token, do not proceed

    const url = `https://localhost:7239/api/Staff/UpdatePhoneNumberByStaffId/${encodeURIComponent(staffId)}?phoneNumber=${encodeURIComponent(newPhoneNumber)}`;

    // Show loading state (optional, can use a spinner or message)

    showLoadingState(this);

    $.ajax({

        url: url,

        type: 'PUT',

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function () {

            console.log("Staff's phone number updated successfully.");

            fetchUpdatedStaffDetailsById(staffId); // Fetch the updated staff details by Staff ID

        },

        error: function (xhr, status, error) {

            hideLoadingState(document.getElementById('updatePhoneNumberBtn'), 'Update Phone Number');

            console.error("Failed to update phone number", xhr, status, error);

            alert(`Error: ${xhr.responseText || 'Unable to update phone number'}`);

        }

    });

});

// Fetch updated staff details by staff ID and display in the table

function fetchUpdatedStaffDetailsById(staffId) {

    const token = getAuthToken();

    if (!token) return; // If no token, do not proceed

    $.ajax({

        url: `https://localhost:7239/api/Staff/GetStaffById/${encodeURIComponent(staffId)}`,

        type: "GET",

        headers: {

            Authorization: `Bearer ${token}`

        },

        success: function (response) {

            const tbody = document.querySelector("#updatedStaffPhoneNumberTable tbody");

            tbody.innerHTML = ""; // Clear existing rows

            if (!response) {

                alert("No staff found with that ID.");

                document.getElementById('updatedStaffPhoneNumberTable').style.display = 'none';

                return;

            }

            const staff = response; // Assuming the API returns a single staff member object

            const row = `
<tr>
<td>${staff.staffId}</td>
<td>${staff.firstName}</td>
<td>${staff.lastName}</td>
<td>${staff.email}</td>
<td>${staff.phoneNumber}</td>
<td>${staff.storeId}</td>
<td>${staff.active}</td>
<td>${staff.lastUpdate}</td>
</tr>

            `;

            tbody.innerHTML += row;

            document.getElementById('updatedStaffPhoneNumberTable').style.display = 'table';

        },

        error: function (xhr, status, error) {

            console.error("Failed to fetch updated staff details", xhr, status, error);

            alert(`Error: ${xhr.responseText || 'Unable to fetch updated staff details'}`);

        }

    });

}
