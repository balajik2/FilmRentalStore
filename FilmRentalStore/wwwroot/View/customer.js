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

// Get all customers with authentication
document.getElementById('getCustomersBtn').addEventListener('click', function () {
    const token = getAuthToken();
    if (!token) return;

    showLoadingState(this);

    let xhr = new XMLHttpRequest();
    xhr.open("GET", "https://localhost:7239/api/Customers/GetCustomer", true);
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

// Add a customer with authentication
document.getElementById('addCustomerBtn').addEventListener('click', function () {
    const token = getAuthToken();
    if (!token) return;

    const requestBody = document.getElementById('requestBody').value;
    try {
        const parsedData = JSON.parse(requestBody);

        $.ajax({
            url: 'https://localhost:7239/api/Customers',
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
                document.getElementById('getCustomersBtn').click();
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
        url: `https://localhost:7239/api/Customers/lastname/${encodeURIComponent(lastName)}`,
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
        alert("Please enter a First name.");
        return;
    }

    const token = getAuthToken();
    if (!token) return;

    showLoadingState(this);

    $.ajax({
        url: `https://localhost:7239/api/Customers/firstname/${encodeURIComponent(firstName)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            hideLoadingState(document.getElementById('getCustomerByFirsttNameBtn'), 'Get Customer By First Name');
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
            hideLoadingState(document.getElementById('getCustomerByFirstNameBtn'), 'Get Customer By First Name');
            alert("Failed to fetch customers by Firstt name.");
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
        url: `https://localhost:7239/api/Customers/email/${encodeURIComponent(email)}`,
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
        url: `https://localhost:7239/api/Customers/city/${encodeURIComponent(city)}`,
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
        url: `https://localhost:7239/api/Customers/country/${encodeURIComponent(country)}`,
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
        url: `https://localhost:7239/api/Customers/active`,
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
        url: `https://localhost:7239/api/Customers/inactive`,
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

// Get customer by phone with authentication
document.getElementById('getCustomerByPhoneBtn').addEventListener('click', function () {
    const phone = document.getElementById('phone').value.trim();
    if (!phone) {
        alert("Please enter a phone number.");
        return;
    }

    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    $.ajax({
        url: `https://localhost:7239/api/Customers/phone/${encodeURIComponent(phone)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#searchCustomerByPhoneTable tbody");
            tbody.innerHTML = ""; // Clear existing rows

            if (!response || response.length === 0) {
                alert("No customer found with the given phone number.");
                document.getElementById('searchCustomerByPhoneTable').style.display = 'none';
                return;
            }

            const customer = response; // Assuming API returns a single customer object
            const row = `
                <tr>
                    <td>${customer.customerId}</td>
                    <td>${customer.storeId}</td>
                    <td>${customer.firstName}</td>
                    <td>${customer.lastName}</td>
                    <td>${customer.email}</td>
                    <td>${customer.addressId}</td>
                    <td>${customer.phone}</td>
                    <td>${customer.active}</td>
                    <td>${customer.createDate}</td>
                    <td>${customer.lastUpdate}</td>
                </tr>
            `;
            tbody.innerHTML += row;

            document.getElementById('searchCustomerByPhoneTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch customer by phone", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch customer by phone'}`);
        }
    });
});


// Update customer's first name by customer ID with authentication
document.getElementById('updateCustomerFirstNameBtn').addEventListener('click', function () {
    const customerId = document.getElementById('updateCustomerId').value.trim();
    const newFirstName = document.getElementById('updateFirstName').value.trim();

    if (!customerId || !newFirstName) {
        alert("Please provide both Customer ID and new first name.");
        return;
    }

    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    const url = `https://localhost:7239/api/Customers/UpdateFirstNameById/${encodeURIComponent(customerId)}?name=${encodeURIComponent(newFirstName)}`;

    $.ajax({
        url: url,
        type: 'PUT',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function () {
            console.log("Customer's first name updated successfully.");
            fetchUpdatedCustomerDetails(customerId);
        },
        error: function (xhr, status, error) {
            console.error("Failed to update customer's first name", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to update first name'}`);
        }
    });
});

// Fetch updated customer details by ID and display in the table
function fetchUpdatedCustomerDetails(customerId) {
    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    $.ajax({
        url: `https://localhost:7239/api/Customers/${encodeURIComponent(customerId)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#updatedCustomerTable tbody");
            tbody.innerHTML = ""; // Clear existing rows

            if (!response) {
                alert("No customer details found.");
                document.getElementById('updatedCustomerTable').style.display = 'none';
                return;
            }

            const customer = response; // Assuming API returns a single customer object
            const row = `
                <tr>
                    <td>${customer.customerId}</td>
                    <td>${customer.storeId}</td>
                    <td>${customer.firstName}</td>
                    <td>${customer.lastName}</td>
                    <td>${customer.email}</td>
                    <td>${customer.addressId}</td>
                    <td>${customer.phone}</td>
                    <td>${customer.active}</td>
                    <td>${customer.createDate}</td>
                    <td>${customer.lastUpdate}</td>
                </tr>
            `;
            tbody.innerHTML += row;

            document.getElementById('updatedCustomerTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch updated customer details", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch updated customer details'}`);
        }
    });
}