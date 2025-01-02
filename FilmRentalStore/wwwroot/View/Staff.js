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





// Get customers by last name with authentication
//document.getElementById('getStaffByLastNameBtn').addEventListener('click', function () {
//    const lastName = document.getElementById('lastName').value.trim();
//    if (!lastName) {
//        alert("Please enter a last name.");
//        return;
//    }

//    const token = getAuthToken();
//    if (!token) return;

//    showLoadingState(this);

//    $.ajax({
//        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/GetStaffByLastName?lastname=${encodeURIComponent(lastName)}`,
//        type: "GET",
//        headers: {
//            Authorization: `Bearer ${token}`
//        },
//        success: function (response) {
//            hideLoadingState(document.getElementById('getStaffByLastNameBtn'), 'Get Staff By Last Name');
//            const tbody = document.querySelector("#searchStaffTable tbody");
//            tbody.innerHTML = "";

//            if (response.length === 0) {
//                alert("No staff found with the last name.");
//                document.getElementById('searchStaffTable').style.display = 'none';
//                return;
//            }

//            response.forEach(staff => {
//                const row = `
//                    <tr>
//                        <td>${staff.staffId}</td>
//                            <td>${staff.firstName}</td>
//                            <td>${staff.lastName}</td>
//                            <td>${staff.addressId}</td>
//                            <td>${staff.email}</td>
//                            <td>${staff.storeId}</td>
//                            <td>${staff.active}</td>
//                            <td>${staff.lastUpdate}</td>
//                            <td>${staff.urlPath}</td>
//                    </tr>
//                `;
//                tbody.innerHTML += row;
//            });

//            document.getElementById('searchStaffTable').style.display = 'table';
//        },
//        error: function () {
//            hideLoadingState(document.getElementById('getStaffByLastNameBtn'), 'Get Staff By Last Name');
//            alert("Failed to fetch staff by last name.");
//        }
//    });
//});







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
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Staff/GetStaffByLastName?lastname=${encodeURIComponent(lastName)}`,
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
                        <td>${staff.active ? 'Yes' : 'No'}</td>
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
