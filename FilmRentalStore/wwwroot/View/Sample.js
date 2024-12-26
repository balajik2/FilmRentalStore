$(document).ready(function () {
    // Fetch Countries when the button is clicked
    $('#GetAllCountries').click(function () {
        // Retrieve token from localStorage
        const token = localStorage.getItem('jwtToken');
        if (!token) {
            alert('You are not authenticated. Please log in.');
            return;
        }

        $.ajax({
            url: 'https://localhost:7239/api/Customers/GetCustomer', // Replace with your Country API endpoint
            type: 'GET',
            headers: {
                Authorization: `Bearer ${token}` // Attach the token in the Authorization header
            },
            success: function (data) {
                let list = '';
                // Loop through the country data and create list items
                data.forEach(customer => {
                    list += `<li>${customer.customerId} (CountryId: ${customer.firstName})</li>`;
                });
                $('#countryList').html(list); // Update the country list in the DOM
            },
            error: function (error) {
                console.error('Error fetching customer:', error);
                if (error.status === 401) {
                    alert('Authentication failed. Please log in again.');
                } else {
                    alert('Failed to fetch country data. Please try again.');
                }
            }
        });
        
    });
});