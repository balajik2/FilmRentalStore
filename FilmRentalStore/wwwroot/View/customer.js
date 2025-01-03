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











//GET ALL CUSTOMER DETAILS

document.getElementById('getCustomersBtn').addEventListener('click', function () {
    const token = getAuthToken();
    if (!token) return;

    showLoadingState(this);

    let xhr = new XMLHttpRequest();
    xhr.open("GET", "https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/GetCustomer", true);
    xhr.setRequestHeader("Authorization", `Bearer ${token}`);

    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            hideLoadingState(document.getElementById('getCustomersBtn'), 'Get Customers');

            if (xhr.status === 200) {
                console.log("GET Request Successful", xhr.responseText);
                let customers = JSON.parse(xhr.responseText);
                let tbody = document.querySelector("#customerTable tbody");
                tbody.innerHTML = '';

                if (customers.length === 0) {
                    alert('No customers found.');
                    document.getElementById('customerTable').style.display = 'none';
                    return;
                }

                customers.forEach(customer => {
                    let row = `
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

                document.getElementById('customerTable').style.display = 'table';
            } else {
                handleAuthError(xhr);
                alert(`Error: ${xhr.responseText || xhr.statusText || 'Unable to fetch customers'}`);
            }
        }
    };
    xhr.send();
});





 //Add a customer with authentication
document.getElementById('addCustomerBtn').addEventListener('click', function () {
    const token = getAuthToken(); // Replace with your actual token retrieval logic
    if (!token) {
        alert("Authorization token is missing!");
        return;
    }

    const requestBody = document.getElementById('requestBody').value;

    try {
        const parsedData = JSON.parse(requestBody);

        // Ensure mandatory fields are correctly set
        if (!parsedData.active || parsedData.active === "string") {
            parsedData.active = "1"; // Default valid value for active
        }

        // Send POST request
        $.ajax({
            url: 'https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(parsedData),
            headers: {
                Authorization: `Bearer ${token}`
            },
            success: function (response) {
                console.log("POST Request Successful", response);
                document.getElementById('responseDetails').textContent = JSON.stringify(response, null, 2);
                document.getElementById('responseDetails').style.display = 'block';

                // Show success message
                const successMessage = document.getElementById('successMessage');
                successMessage.style.display = 'block';

                // Hide success message after 3 seconds
                setTimeout(function () {
                    successMessage.style.display = 'none';
                }, 3000);
            },
            error: function (xhr, status, error) {
                console.error("POST Request Failed", xhr, status, error);
                alert(`Error: ${xhr.responseText || 'Unable to add customer'}`);
            }
        });
    } catch (error) {
        alert("Invalid JSON input. Please ensure the request body is properly formatted.");
    }
});

// Example getAuthToken function (replace with actual implementation)
function getAuthToken() {
    // Replace with the logic to retrieve the token from your system
    return "your-auth-token";
}







// Get customers by last name with authentication
document.getElementById('getCustomerByLastNameBtn').addEventListener('click', function () {
    const lastName = document.getElementById('lastName').value.trim();
    if (!lastName) {
        alert("Please enter a last name.");
        return;
    }

    const token = getAuthToken();
    if (!token) return;

    showLoadingState(this);

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/lastname/${encodeURIComponent(lastName)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            hideLoadingState(document.getElementById('getCustomerByLastNameBtn'), 'Get Customer By Last Name');
            const tbody = document.querySelector("#searchCustomerTable tbody");
            tbody.innerHTML = "";

            if (response.length === 0) {
                alert("No customers found with the last name.");
                document.getElementById('searchCustomerTable').style.display = 'none';
                return;
            }

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

            document.getElementById('searchCustomerTable').style.display = 'table';
        },
        error: function () {
            hideLoadingState(document.getElementById('getCustomerByLastNameBtn'), 'Get Customer By Last Name');
            alert("Failed to fetch customers by last name.");
        }
    });
});




// Get customers by first name with authentication
document.getElementById('getCustomerByFirstNameBtn').addEventListener('click', function () {
    const firstName = document.getElementById('firstName').value.trim();
    if (!firstName) {
        alert("Please enter a first name.");
        return;
    }

    const token = getAuthToken();
    if (!token) return;

    showLoadingState(this);

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/firstname/${encodeURIComponent(firstName)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            hideLoadingState(document.getElementById('getCustomerByFirstNameBtn'), 'Get Customer By First Name');
            const tbody = document.querySelector("#searchCustomerByFirstNameTable tbody");
            tbody.innerHTML = "";

            if (response.length === 0) {
                alert("No customers found with the first name.");
                document.getElementById('searchCustomerByFirstNameTable').style.display = 'none';
                return;
            }

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

            document.getElementById('searchCustomerByFirstNameTable').style.display = 'table';
        },
        error: function () {
            hideLoadingState(document.getElementById('getCustomerByFirstNameBtn'), 'Get Customer By First Name');
            alert("Failed to fetch customers by first name.");
        }
    });
});





// Get customer by email with authentication
document.getElementById('getCustomerByEmailBtn').addEventListener('click', function () {
    const email = document.getElementById('email').value.trim();
    if (!email) {
        alert("Please enter an email.");
        return;
    }

    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/email/${encodeURIComponent(email)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#emailSearchCustomerTable tbody");
            tbody.innerHTML = ""; // Clear table rows

            if (!response) {
                alert("No customer found with the provided email.");
                document.getElementById('emailSearchCustomerTable').style.display = 'none';
                return;
            }

            const row = `
                <tr>
                    <td>${response.customerId}</td>
                    <td>${response.storeId}</td>
                    <td>${response.firstName}</td>
                    <td>${response.lastName}</td>
                    <td>${response.email}</td>
                    <td>${response.addressId}</td>
                    <td>${response.active}</td>
                    <td>${response.createDate}</td>
                    <td>${response.lastUpdate}</td>
                </tr>
            `;
            tbody.innerHTML += row;

            document.getElementById('emailSearchCustomerTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch customer by email", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch customer by email'}`);
        }
    });
});


// Get customers by city with authentication
document.getElementById('getCustomerByCityBtn').addEventListener('click', function () {
    const city = document.getElementById('city').value.trim();
    if (!city) {
        alert("Please enter a city name.");
        return;
    }

    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/city/${encodeURIComponent(city)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#citySearchCustomerTable tbody");
            tbody.innerHTML = ""; // Clear table rows

            if (response.length === 0) {
                alert("No customers found in the specified city.");
                document.getElementById('citySearchCustomerTable').style.display = 'none';
                return;
            }

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
                        <td>${customer.address1}</td>
                        <td>${customer.address2}</td>
                        <td>${customer.district}</td>
                        <td>${customer.cityId}</td>
                        <td>${customer.postalCode}</td>
                        <td>${customer.phone}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            document.getElementById('citySearchCustomerTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch customers by city", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch customers by city'}`);
        }
    });
});


// Get customers by country with authentication
document.getElementById('getCustomerByCountryBtn').addEventListener('click', function () {
    const country = document.getElementById('country').value.trim();
    if (!country) {
        alert("Please enter a country name.");
        return;
    }

    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/country/${encodeURIComponent(country)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#countrySearchCustomerTable tbody");
            tbody.innerHTML = ""; // Clear table rows

            if (response.length === 0) {
                alert("No customers found in the specified country.");
                document.getElementById('countrySearchCustomerTable').style.display = 'none';
                return;
            }

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

            document.getElementById('countrySearchCustomerTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch customers by country", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch customers by country'}`);
        }
    });
});


// Get all active customers with authentication
document.getElementById('getActiveCustomersBtn').addEventListener('click', function () {
    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/active`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#activeCustomersTable tbody");
            tbody.innerHTML = ""; // Clear table rows

            if (response.length === 0) {
                alert("No active customers found.");
                document.getElementById('activeCustomersTable').style.display = 'none';
                return;
            }

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

            document.getElementById('activeCustomersTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch active customers", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch active customers'}`);
        }
    });
});


// Get all inactive customers with authentication
document.getElementById('getInactiveCustomersBtn').addEventListener('click', function () {
    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/inactive`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#inactiveCustomersTable tbody");
            tbody.innerHTML = ""; // Clear table rows

            if (response.length === 0) {
                alert("No inactive customers found.");
                document.getElementById('inactiveCustomersTable').style.display = 'none';
                return;
            }

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

            document.getElementById('inactiveCustomersTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch inactive customers", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch inactive customers'}`);
        }
    });
});



//This is working code
// Get customer by phone with authentication
document.getElementById('getCustomerByPhoneBtn').addEventListener('click', function () {
    const phone = document.getElementById('phone').value.trim();
    if (!phone) {
        alert("Please enter a phone number.");
        return;
    }

    const token = getAuthToken(); // Ensure you have a valid token retrieval function.
    if (!token) return;

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/phone/${encodeURIComponent(phone)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#searchCustomerByPhoneTable tbody");
            tbody.innerHTML = ""; // Clear previous data.

            if (!response || response.length === 0) {
                alert("No customer found with the given phone number.");
                document.getElementById('searchCustomerByPhoneTable').style.display = 'none';
                return;
            }

            response.forEach(customer => { // Iterate through the array of customers.
                const row = `
                    <tr>
                        <td>${customer.customerId}</td>
                        <td>${customer.storeId}</td>
                        <td>${customer.firstName}</td>
                        <td>${customer.lastName}</td>
                        <td>${customer.email}</td>
                        <td>${customer.addressId}</td>
                        <td>${customer.active}</td>
                        <td>${new Date(customer.createDate).toLocaleString()}</td>
                        <td>${new Date(customer.lastUpdate).toLocaleString()}</td>
                        <td>${customer.address1 || ''}</td>
                        <td>${customer.address2 || ''}</td>
                        <td>${customer.district || ''}</td>
                        <td>${customer.cityId}</td>
                        <td>${customer.postalCode || ''}</td>
                        <td>${customer.phone}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            document.getElementById('searchCustomerByPhoneTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch customer by phone", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch customer by phone'}`);
        }
    });
});















//UPDATE customer by firstname with authentication


document.getElementById('updateCustomerFirstNameBtn').addEventListener('click', function () {
    const customerId = document.getElementById('updateCustomerIdFirstName').value.trim();
    const newFirstName = document.getElementById('updateFirstName').value.trim();

    if (!customerId || !newFirstName) {
        alert("Please provide both Customer ID and new first name.");
        return;
    }

    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

<<<<<<< HEAD
    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/UpdateFirstNameById/${encodeURIComponent(customerId)}?name=${encodeURIComponent(newFirstName)}`;
=======
    const url =`https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/UpdateFirstNameById/${encodeURIComponent(customerId)}?name=${encodeURIComponent(newFirstName)}`;
>>>>>>> origin/FilmRentalStore-2

    $.ajax({
        url: url,
        type: 'PUT',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function () {
            console.log("Customer's first name updated successfully.");
            fetchUpdatedCustomerDetailsByFirstName(newFirstName); // Fetch by new first name
        },
        error: function (xhr, status, error) {
            console.error("Failed to update customer's first name", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to update first name'}`);
        }
    });
});

// Fetch updated customer details by first name and display in the table
function fetchUpdatedCustomerDetailsByFirstName(firstName) {
    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/firstname/${encodeURIComponent(firstName)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#updatedCustomerFirstNameTable tbody");
            tbody.innerHTML = ""; // Clear existing rows

            if (!response || response.length === 0) {
                alert("No customer details found with that first name.");
                document.getElementById('updatedCustomerFirstNameTable').style.display = 'none';
                return;
            }

            const customer = response[0]; // Assuming API returns an array with at least one customer
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

            document.getElementById('updatedCustomerFirstNameTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch updated customer details", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch updated customer details'}`);
        }
    });
}



//update lastname for customer by customerid


document.getElementById('updateCustomerLastNameBtn').addEventListener('click', function () {
    const customerId = document.getElementById('updateLastNameCustomerId').value.trim();
    const newLastName = document.getElementById('updateLastName').value.trim();

    if (!customerId || !newLastName) {
        alert("Please provide both Customer ID and new last name.");
        return;
    }

    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    // Construct the URL to update last name by customerId
    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/UpdateLastNameById/${encodeURIComponent(customerId)}?lastname=${encodeURIComponent(newLastName)}`;

    // Send the PUT request to update the last name
    $.ajax({
        url: url,
        type: 'PUT',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function () {
            console.log("Customer's last name updated successfully.");
            fetchUpdatedCustomerDetailsByLastName(newLastName); // Fetch the updated customer details by last name
        },
        error: function (xhr, status, error) {
            console.error("Failed to update customer's last name", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to update last name'}`);
        }
    });
});

// Fetch updated customer details by last name and display in the table
function fetchUpdatedCustomerDetailsByLastName(lastName) {
    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    // Make a GET request to fetch customer details by last name
    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/lastname/${encodeURIComponent(lastName)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#updatedCustomerLastNameTable tbody");
            tbody.innerHTML = ""; // Clear existing rows

            if (!response || response.length === 0) {
                alert("No customer details found with that last name.");
                document.getElementById('updatedCustomerLastNameTable').style.display = 'none';
                return;
            }

            const customer = response[0]; // Assuming API returns an array with at least one customer
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

            document.getElementById('updatedCustomerLastNameTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch updated customer details", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch updated customer details'}`);
        }
    });
}




//UPDATE CUSTOMER EMAIL BY CUSTOMER ID

document.getElementById('updateCustomerEmailBtn').addEventListener('click', function () {
    const customerId = document.getElementById('updateCustomerIdEmail').value.trim();
    const newEmail = document.getElementById('updateEmail').value.trim();

    if (!customerId || !newEmail) {
        alert("Please provide both Customer ID and new email.");
        return;
    }

    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    // Construct the URL to update email by customerId
    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/UpdateEmailByid/${encodeURIComponent(customerId)}?email=${encodeURIComponent(newEmail)}`;

    // Send the PUT request to update the email
    $.ajax({
        url: url,
        type: 'PUT',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function () {
            console.log("Customer's email updated successfully.");
            fetchUpdatedCustomerDetailsByEmail(newEmail); // Fetch updated customer details by email
        },
        error: function (xhr, status, error) {
            console.error("Failed to update customer's email", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to update email'}`);
        }
    });
});



function fetchUpdatedCustomerDetailsByEmail(email) {
    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/email/${encodeURIComponent(email)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#updatedCustomerEmailTable tbody");
            tbody.innerHTML = ""; // Clear existing rows

            // Log the entire API response to understand its structure
            console.log("Fetched customer details by email:", response);

            // Check if the response is an array or an object
            if (Array.isArray(response) && response.length > 0) {
                const customer = response[0]; // Assuming it's an array and the first element is the customer
                populateCustomerTable(customer);
            } else if (response && response.customerId) {
                // If response is an object and contains customer data directly
                populateCustomerTable(response);
            } else {
                alert("No valid customer data returned.");
                document.getElementById('updatedCustomerEmailTable').style.display = 'none';
            }
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch updated customer details", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch updated customer details'}`);
        }
    });
}

function populateCustomerTable(customer) {
    const tbody = document.querySelector("#updatedCustomerEmailTable tbody");

    // Populate the table with the customer's updated data
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

    // Display the table with updated customer data
    document.getElementById('updatedCustomerEmailTable').style.display = 'table';
}





//update customer store by customerId






document.getElementById('updateCustomerStoreBtn').addEventListener('click', function () {
    const customerId = document.getElementById('updateCustomerIdStore').value.trim();
    const storeId = document.getElementById('updateStoreId').value.trim();

    // Validate input
    if (!customerId || !storeId) {
        displayMessage("Please provide both Customer ID and Store ID.", "alert-danger");
        return;
    }

    const token = getAuthToken();
    if (!token) {
        displayMessage("Authorization token is missing.", "alert-danger");
        return;
    }

    // Construct the URL to update storeId by customerId
    const updateUrl = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/AssignStoreToCustomer?customerid=${encodeURIComponent(customerId)}&storeid=${encodeURIComponent(storeId)}`;

    // Send the PUT request to update the storeId
    $.ajax({
        url: updateUrl,
        type: 'PUT',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function () {
            console.log("Customer's store updated successfully.");
            displayMessage("Store ID updated successfully.", "alert-success");
        },
        error: function (xhr, status, error) {
            console.error("Failed to update customer's store", xhr, status, error);
            const errorMessage = xhr.responseText || 'Unable to update store.';
            displayMessage(`Error: ${errorMessage}`, "alert-danger");
        }
    });
});

// Function to display status messages
function displayMessage(message, alertClass) {
    const statusDiv = document.getElementById('statusMessage');
    statusDiv.textContent = message;
    statusDiv.className = `alert ${alertClass}`;
    statusDiv.style.display = "block";

    // Automatically hide the message after 5 seconds
    setTimeout(() => {
        statusDiv.style.display = "none";
    }, 5000);
}


document.getElementById('updateCustomerAddressBtn').addEventListener('click', function () {
    const customerId = document.getElementById('updateCustomerIdAddress').value.trim();
    const addressId = document.getElementById('updateAddressId').value.trim();

    // Validate input
    if (!customerId || !addressId) {
        displayMessage("Please provide both Customer ID and Address ID.", "alert-danger");
        return;
    }

    const token = getAuthToken();
    if (!token) {
        displayMessage("Authorization token is missing.", "alert-danger");
        return;
    }

    // Construct the URL to update addressId by customerId
    const updateUrl = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/AsignAddress?id=${encodeURIComponent(customerId)}&addressid=${encodeURIComponent(addressId)}`;

    // Send the PUT request to update the addressId
    $.ajax({
        url: updateUrl,
        type: 'PUT',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function () {
            console.log("Customer's address updated successfully.");
            displayMessage("Address ID updated successfully.", "alert-success");
        },
        error: function (xhr, status, error) {
            console.error("Failed to update customer's address", xhr, status, error);
            const errorMessage = xhr.responseText || 'Unable to update address.';
            displayMessage(`Error: ${errorMessage}`, "alert-danger");
        }
    });
});

// Function to display status messages
function displayMessage(message, alertClass) {
    const statusDiv = document.getElementById('statusMessage');
    statusDiv.textContent = message;
    statusDiv.className = `alert ${alertClass}`;
    statusDiv.style.display = "block";

    // Automatically hide the message after 5 seconds
    setTimeout(() => {
        statusDiv.style.display = "none";
    }, 5000);
}

// Mock function to simulate getting a token
//function getAuthToken() {
//    return "mock-token"; 
//}


document.getElementById('updatePhoneBtn').addEventListener('click', function () {
    const customerId = document.getElementById('customerId').value.trim();
    const newPhoneNumber = document.getElementById('newPhoneNumber').value.trim();

    if (!customerId || !newPhoneNumber) {
        alert("Please enter both Customer ID and new phone number.");
        return;
    }

    const token = getAuthToken(); // Ensure you have a valid token retrieval function.
    if (!token) return;

    $.ajax({
        url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Customers/UpdatePhoneNumberByid/${encodeURIComponent(customerId)}?phone=${encodeURIComponent(newPhoneNumber)}`,
        type: "PUT", // HTTP PUT method for updates.
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function () {
            const messageElement = document.getElementById('updateStatusMessage');
            messageElement.textContent = "Phone number updated successfully!";
            messageElement.style.display = 'block';
            messageElement.classList.remove('text-danger');
            messageElement.classList.add('text-success');
        },
        error: function (xhr, status, error) {
            console.error("Failed to update phone number", xhr, status, error);
            const messageElement = document.getElementById('updateStatusMessage');
            messageElement.textContent = `Error: ${xhr.responseText || 'Unable to update phone number'}`;
            messageElement.style.display = 'block';
            messageElement.classList.remove('text-success');
            messageElement.classList.add('text-danger');
        }
    });
});










