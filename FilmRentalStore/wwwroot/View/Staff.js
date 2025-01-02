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
    xhr.open("GET", "https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/GetStaff", true);
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
            url: 'https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/AddStaff',
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

                // Optionally refresh the staff list after adding
                // document.getElementById('getStaffByLastNameBtn').click();
            },
            error: function (xhr, status, error) {
                if (xhr.status === 401) {
                    handleAuthError(xhr);
                }
                console.error("POST Request Failed", xhr, status, error);
                console.error("Error Details:", xhr.responseText);
                alert(`Error: ${xhr.responseText || 'Unable to add staff'}`);
            }
        });
    } catch (error) {
        alert("Invalid JSON input. Please ensure the request body is properly formatted.");
    }
});






// get staff by last name
// Get staff by last name
document.getElementById('getStaffByLastNameBtn').addEventListener('click', function () {
    const lastName = document.getElementById('lastName').value.trim();
    if (!lastName) {
        alert("Please enter a last name.");
        return;
    }

    const token = getAuthToken(); // Replace with your actual token retrieval logic
    if (!token) {
        alert("Authorization token is missing!");
        return;
    }

    showLoadingState(this); // Show loading indicator

    // Send GET request to fetch staff by last name
    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/GetStaffByLastName?lastname=${encodeURIComponent(lastName)}`,
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            hideLoadingState(document.getElementById('getStaffByLastNameBtn'), 'Get Staff By Last Name'); // Hide loading indicator
            const tbody = document.querySelector("#searchStaffTableByLastName tbody");
            tbody.innerHTML = ""; // Clear existing table rows

            if (response.length === 0) {
                alert("No staff found with the last name.");
                document.getElementById('searchStaffTableByLastName').style.display = 'none'; // Hide table if no staff found
                return;
            }

            // Populate the table with staff data
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

            document.getElementById('searchStaffTableByLastName').style.display = 'table'; // Show table with data
        },
        error: function (xhr, status, error) {
            hideLoadingState(document.getElementById('getStaffByLastNameBtn'), 'Get Staff By Last Name'); // Hide loading indicator
            console.error("GET Request Failed", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch staff by last name'}`);
        }
    });
});

// Example getAuthToken function (replace with actual implementation)
function getAuthToken() {
    // Replace with the logic to retrieve the token from your system
    return "your-auth-token"; // Replace with the actual token
}

// Loading state handling (if needed)
function showLoadingState(button) {
    button.disabled = true; // Disable the button while loading
    button.textContent = 'Loading...';
}

function hideLoadingState(button, originalText) {
    button.disabled = false; // Enable the button after loading
    button.textContent = originalText || 'Get Staff By Last Name';
}












//get staff by first name
// Get staff by first name
document.getElementById('getStaffByFirstNameBtn').addEventListener('click', function () {
    const firstName = document.getElementById('firstName').value.trim();
    if (!firstName) {
        alert("Please enter a first name.");
        return;
    }

    const token = getAuthToken(); // Replace with your actual token retrieval logic
    if (!token) {
        alert("Authorization token is missing!");
        return;
    }

    showLoadingState(this); // Show loading indicator

    // Send GET request to fetch staff by first name
    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/GetStaffByFirstName?firstname=${encodeURIComponent(firstName)}`,
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            hideLoadingState(document.getElementById('getStaffByFirstNameBtn'), 'Get Staff By First Name'); // Hide loading indicator
            const tbody = document.querySelector("#searchStaffTableByFirstName tbody");
            tbody.innerHTML = ""; // Clear existing table rows

            if (response.length === 0) {
                alert("No staff found with the first name.");
                document.getElementById('searchStaffTableByFirstName').style.display = 'none'; // Hide table if no staff found
                return;
            }

            // Populate the table with staff data
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

            document.getElementById('searchStaffTableByFirstName').style.display = 'table'; // Show table with data
        },
        error: function (xhr, status, error) {
            hideLoadingState(document.getElementById('getStaffByFirstNameBtn'), 'Get Staff By First Name'); // Hide loading indicator
            console.error("GET Request Failed", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch staff by first name'}`);
        }
    });
});

// Example getAuthToken function (replace with actual implementation)
function getAuthToken() {
    // Replace with the logic to retrieve the token from your system
    return "your-auth-token"; // Replace with the actual token
}

// Loading state handling (if needed)
function showLoadingState(button) {
    button.disabled = true; // Disable the button while loading
    button.textContent = 'Loading...';
}

function hideLoadingState(button, originalText) {
    button.disabled = false; // Enable the button after loading
    button.textContent = originalText || 'Get Staff By First Name';
}














// Get Staff By City
document.getElementById('getStaffByCityBtn').addEventListener('click', function () {
    const city = document.getElementById('city').value.trim();
    if (!city) {
        alert("Please enter a city.");
        return;
    }

    const token = getAuthToken(); // Replace with your actual token retrieval logic
    if (!token) {
        alert("Authorization token is missing!");
        return;
    }

    showLoadingState(this); // Show loading indicator

    // Send GET request to fetch staff by city
    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/GetStaffByCity?city=${encodeURIComponent(city)}`,
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            hideLoadingState(document.getElementById('getStaffByCityBtn'), 'Get Staff By City'); // Hide loading indicator
            const tbody = document.querySelector("#searchStaffTableByCity tbody");
            tbody.innerHTML = ""; // Clear existing table rows

            if (response.length === 0) {
                alert("No staff found in the specified city.");
                document.getElementById('searchStaffTableByCity').style.display = 'none'; // Hide table if no staff found
                return;
            }

            // Populate the table with staff data
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

            document.getElementById('searchStaffTableByCity').style.display = 'table'; // Show table with data
        },
        error: function (xhr, status, error) {
            hideLoadingState(document.getElementById('getStaffByCityBtn'), 'Get Staff By City'); // Hide loading indicator
            console.error("GET Request Failed", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch staff by city'}`);
        }
    });
});

// Example getAuthToken function (replace with actual implementation)
function getAuthToken() {
    // Replace with the logic to retrieve the token from your system
    return "your-auth-token"; // Replace with the actual token
}

// Loading state handling (if needed)
function showLoadingState(button) {
    button.disabled = true; // Disable the button while loading
    button.textContent = 'Loading...';
}

function hideLoadingState(button, originalText) {
    button.disabled = false; // Enable the button after loading
    button.textContent = originalText || 'Get Staff By City';
}








//Get Staff By Country
document.getElementById('getStaffByCountryBtn').addEventListener('click', function () {
    const country = document.getElementById('country').value.trim();
    if (!country) {
        alert("Please enter a country.");
        return;
    }

    const token = getAuthToken(); // Replace with your actual token retrieval logic
    if (!token) {
        alert("Authorization token is missing!");
        return;
    }

    showLoadingState(this); // Show loading indicator

    // Send GET request to fetch staff by country
    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/GetStaffByCountry?country=${encodeURIComponent(country)}`,
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            hideLoadingState(document.getElementById('getStaffByCountryBtn'), 'Get Staff By Country'); // Hide loading indicator
            const tbody = document.querySelector("#searchStaffTableByCountry tbody");
            tbody.innerHTML = ""; // Clear existing table rows

            if (response.length === 0) {
                alert("No staff found in the specified country.");
                document.getElementById('searchStaffTableByCountry').style.display = 'none'; // Hide table if no staff found
                return;
            }

            // Populate the table with staff data
            response.forEach(staff => {
                const row = `
                    <tr>
                        <td>${staff.staffId}</td>
                        <td>${staff.firstName}</td>
                        <td>${staff.lastName}</td>
                        <td>${staff.addressId}</td>
                        <td>${staff.email}</td>
                        <td>${staff.storeId}</td>
                        <td>${staff.active ? 'Yes' : 'No'}</td>
                        <td>${staff.lastUpdate}</td>
                        <td>${staff.urlPath}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            document.getElementById('searchStaffTableByCountry').style.display = 'table'; // Show table with data
        },
        error: function (xhr, status, error) {
            hideLoadingState(document.getElementById('getStaffByCountryBtn'), 'Get Staff By Country'); // Hide loading indicator
            console.error("GET Request Failed", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch staff by country'}`);
        }
    });
});

// Example getAuthToken function (replace with actual implementation)
function getAuthToken() {
    // Replace with the logic to retrieve the token from your system
    return "your-auth-token"; // Replace with the actual token
}

// Loading state handling (if needed)
function showLoadingState(button) {
    button.disabled = true; // Disable the button while loading
    button.textContent = 'Loading...';
}

function hideLoadingState(button, originalText) {
    button.disabled = false; // Enable the button after loading
    button.textContent = originalText || 'Get Staff By Country';
}












// get staff by email
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
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/GetStaffByEmail?email=${encodeURIComponent(email)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}` // Send the authentication token
        },
        success: function (response) {
            hideLoadingState(document.getElementById('getStaffByEmailBtn'), 'Get Staff By Email'); // Hide loading spinner
            const tbody = document.querySelector("#searchStaffTableByEmail tbody");
            tbody.innerHTML = ""; // Clear previous search results

            // Check if response is empty or null
            if (!response || response.length === 0) {
                alert("No staff found with the given email.");
                document.getElementById('searchStaffTableByEmail').style.display = 'none'; // Hide table if no results
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
            document.getElementById('searchStaffTableByEmail').style.display = 'table';
        },
        error: function (xhr, status, error) {
            hideLoadingState(document.getElementById('getStaffByEmailBtn'), 'Get Staff By Email');
            console.error("Error Details:", xhr.responseText || status || error);
            alert("Failed to fetch staff by email.");
        }
    });
});

// Example getAuthToken function (replace with actual implementation)
function getAuthToken() {
    // Replace with the logic to retrieve the token from your system
    return "your-auth-token"; // Replace with the actual token
}

// Loading state handling (if needed)
function showLoadingState(button) {
    button.disabled = true; // Disable the button while loading
    button.textContent = 'Loading...';
}

function hideLoadingState(button, originalText) {
    button.disabled = false; // Enable the button after loading
    button.textContent = originalText || 'Get Staff By Email';
}








// Get staff By Phone number
document.getElementById('getStaffByPhoneBtn').addEventListener('click', function () {
    const phone = document.getElementById('phone').value.trim();
    if (!phone) {
        alert("Please enter a phone number.");
        return;
    }

    const token = getAuthToken(); // Ensure you have a valid token retrieval function.
    if (!token) return;

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/GetStaffByPhoneNumber?phone=${encodeURIComponent(phone)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#searchStaffByPhoneTable tbody");
            tbody.innerHTML = ""; // Clear previous data.

            if (!response || response.length === 0) {
                alert("No staff found with the given phone number.");
                document.getElementById('searchStaffByPhoneTable').style.display = 'none';
                return;
            }

            response.forEach(staff => { // Iterate through the array of staff.
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

            document.getElementById('searchStaffByPhoneTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch staff by phone", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch staff by phone'}`);
        }
    });
});







document.getElementById('updateStaffAddressBtn').addEventListener('click', function () {
    const staffId = document.getElementById('updateStaffIdAddress').value.trim();
    const addressId = document.getElementById('updateAddressId').value.trim();

    if (!staffId || !addressId) {
        alert("Please provide both Staff ID and Address ID.");
        return;
    }

    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/AssignAddress?staffId=${encodeURIComponent(staffId)}&addressId=${encodeURIComponent(addressId)}`;

    $.ajax({
        url: url,
        type: 'PUT',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            console.log("Staff's address ID updated successfully.");
            displayAddressUpdateMessage(`Address ID ${addressId} successfully assigned to Staff ID ${staffId}.`, "success");
        },
        error: function (xhr, status, error) {
            console.error("Failed to update staff's address ID", xhr, status, error);

            let errorMessage = "Unable to update address ID.";
            if (xhr.responseText) {
                errorMessage = xhr.responseText;
            } else if (xhr.status === 401) {
                errorMessage = "Unauthorized access. Please log in again.";
            } else if (xhr.status === 404) {
                errorMessage = "Staff or Address not found.";
            }

            displayAddressUpdateMessage(errorMessage, "danger");
        }
    });
});

function displayAddressUpdateMessage(message, type) {
    const messageDiv = document.getElementById('updateStaffAddressMessage');
    messageDiv.textContent = message;
    messageDiv.className = `alert alert-${type}`;
    messageDiv.style.display = 'block';
}













// update staff by first name
document.getElementById('updateStaffFirstNameBtn').addEventListener('click', function () {
    const staffId = document.getElementById('updateStaffIdFirstName').value.trim();
    const newFirstName = document.getElementById('updateStaffFirstName').value.trim();

    if (!staffId || !newFirstName) {
        alert("Please provide both Staff ID and new first name.");
        return;
    }

    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/UpdateStaffByFirstName?staffId=${encodeURIComponent(staffId)}&newfirstname=${encodeURIComponent(newFirstName)}`;

    $.ajax({
        url: url,
        type: 'PUT',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function () {
            console.log("Staff's first name updated successfully.");
            fetchUpdatedStaffDetailsByFirstName(newFirstName); // Fetch updated details
        },
        error: function (xhr, status, error) {
            console.error("Failed to update staff's first name", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to update first name'}`);
        }
    });
});

// Fetch updated staff details by first name and display in the table
function fetchUpdatedStaffDetailsByFirstName(firstName) {
    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/firstname/${encodeURIComponent(firstName)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#updatedStaffFirstNameTable tbody");
            tbody.innerHTML = ""; // Clear existing rows

            if (!response || response.length === 0) {
                alert("No staff details found with that first name.");
                document.getElementById('updatedStaffFirstNameTable').style.display = 'none';
                return;
            }

            const staff = response[0]; // Assuming API returns an array with at least one staff member
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









document.getElementById('updateStaffLastNameBtn').addEventListener('click', function () {
    const staffId = document.getElementById('updateStaffIdLastName').value.trim();
    const newLastName = document.getElementById('updateStaffLastName').value.trim();

    if (!staffId || !newLastName) {
        alert("Please provide both Staff ID and new last name.");
        return;
    }

    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/UpdateStaffByLastName?staffId=${encodeURIComponent(staffId)}&newlastname=${encodeURIComponent(newLastName)}`;

    $.ajax({
        url: url,
        type: 'PUT',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function () {
            console.log("Staff's last name updated successfully.");
            displayUpdateMessage(`Staff's last name successfully updated to "${newLastName}".`, "success");
        },
        error: function (xhr, status, error) {
            console.error("Failed to update staff's last name", xhr, status, error);

            let errorMessage = "Unable to update last name.";
            if (xhr.responseText) {
                errorMessage = xhr.responseText;
            } else if (xhr.status === 401) {
                errorMessage = "Unauthorized access. Please log in again.";
            } else if (xhr.status === 404) {
                errorMessage = "No staff found with the provided ID.";
            }

            displayUpdateMessage(errorMessage, "danger");
        }
    });
});

function displayUpdateMessage(message, type) {
    const messageDiv = document.getElementById('updateStaffLastNameMessage');
    messageDiv.textContent = message;
    messageDiv.className = `alert alert-${type}`;
    messageDiv.style.display = 'block';
}








document.getElementById('updateStaffEmailBtn').addEventListener('click', function () {
    const staffId = document.getElementById('updateStaffIdEmail').value.trim();
    const newEmail = document.getElementById('updateStaffEmail').value.trim();

    if (!staffId || !newEmail) {
        alert("Please provide both Staff ID and new email.");
        return;
    }

    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    const url = `/api/Staff/UpdateStaffByEmail?staffId=${encodeURIComponent(staffId)}&email=${encodeURIComponent(newEmail)}`;

    $.ajax({
        url: url,
        type: 'PUT',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function () {
            console.log("Staff's email updated successfully.");
            displayEmailUpdateMessage(`Staff's email successfully updated to "${newEmail}".`, "success");
        },
        error: function (xhr, status, error) {
            console.error("Failed to update staff's email", xhr, status, error);

            let errorMessage = "Unable to update email.";
            if (xhr.responseText) {
                errorMessage = xhr.responseText;
            } else if (xhr.status === 401) {
                errorMessage = "Unauthorized access. Please log in again.";
            } else if (xhr.status === 404) {
                errorMessage = "No staff found with the provided ID.";
            }

            displayEmailUpdateMessage(errorMessage, "danger");
        }
    });
});

function displayEmailUpdateMessage(message, type) {
    const messageDiv = document.getElementById('updateStaffEmailMessage');
    messageDiv.textContent = message;
    messageDiv.className = `alert alert-${type}`;
    messageDiv.style.display = 'block';
}







document.getElementById('assignStoreToStaffBtn').addEventListener('click', function () {
    const staffId = document.getElementById('assignStaffId').value.trim();
    const storeId = document.getElementById('assignStoreId').value.trim();

    if (!staffId || !storeId) {
        alert("Please provide both Staff ID and Store ID.");
        return;
    }

    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/AssignStoreToStaff?staffId=${encodeURIComponent(staffId)}&storeId=${encodeURIComponent(storeId)}`;

    $.ajax({
        url: url,
        type: 'PUT',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function () {
            console.log("Store successfully assigned to staff.");
            displayStoreAssignmentMessage(`Store ${storeId} successfully assigned to Staff ID ${staffId}.`, "success");
        },
        error: function (xhr, status, error) {
            console.error("Failed to assign store to staff", xhr, status, error);

            let errorMessage = "Unable to assign store to staff.";
            if (xhr.responseText) {
                errorMessage = xhr.responseText;
            } else if (xhr.status === 401) {
                errorMessage = "Unauthorized access. Please log in again.";
            } else if (xhr.status === 404) {
                errorMessage = "Staff or Store not found.";
            }

            displayStoreAssignmentMessage(errorMessage, "danger");
        }
    });
});

function displayStoreAssignmentMessage(message, type) {
    const messageDiv = document.getElementById('assignStoreToStaffMessage');
    messageDiv.textContent = message;
    messageDiv.className = `alert alert-${type}`;
    messageDiv.style.display = 'block';
}










document.getElementById('updateStaffPhoneNumberBtn').addEventListener('click', function () {
    const staffId = document.getElementById('updateStaffIdPhoneNumber').value.trim();
    const newPhone = document.getElementById('updatePhoneNumber').value.trim();

    if (!staffId || !newPhone) {
        alert("Please provide both Staff ID and Phone Number.");
        return;
    }

    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/UpdatePhoneNumberByStaff?staffId=${encodeURIComponent(staffId)}&newPhone=${encodeURIComponent(newPhone)}`;

    $.ajax({
        url: url,
        type: 'PUT',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            console.log("Staff's phone number updated successfully.");
            displayPhoneNumberUpdateMessage(`Phone number successfully updated for Staff ID ${staffId}.`, "success");
        },
        error: function (xhr, status, error) {
            console.error("Failed to update staff's phone number", xhr, status, error);

            let errorMessage = "Unable to update phone number.";
            if (xhr.responseText) {
                errorMessage = xhr.responseText;
            } else if (xhr.status === 401) {
                errorMessage = "Unauthorized access. Please log in again.";
            } else if (xhr.status === 404) {
                errorMessage = "Staff not found.";
            }

            displayPhoneNumberUpdateMessage(errorMessage, "danger");
        }
    });
});

function displayPhoneNumberUpdateMessage(message, type) {
    const messageDiv = document.getElementById('updateStaffPhoneNumberMessage');
    messageDiv.textContent = message;
    messageDiv.className = `alert alert-${type}`;
    messageDiv.style.display = 'block';
}







