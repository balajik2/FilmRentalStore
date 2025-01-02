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

//Make Payment

$('#makePaymentBtn').click(function () {
    const paymentId = $('#paymentId').val().trim();
    const amount = $('#paymentAmount').val().trim();

    // Validate inputs
    if (!paymentId || !amount) {
        alert('Please fill in both Payment ID and Amount.');
        return;
    }

    // Construct the URL for MakePayment API
    const url = `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Payment/MakePayment?paymentId=${paymentId}&amount=${encodeURIComponent(amount)}`;
    console.log(`Request URL: ${url}`);

    // AJAX request to make the payment
    $.ajax({
        url: url,
        type: 'PUT', // Adjust the HTTP method based on your API
        contentType: 'application/json',
        headers: { 'Authorization': `Bearer ${getAuthToken()}` }, // Replace with your token retrieval logic
        success: function (response) {
            alert('Payment made successfully!');
            // Show success message
            $('#paymentResult')
                .removeClass('alert-danger')
                .addClass('alert-success')
                .text('Payment made successfully!')
                .fadeIn();

            // Hide success message after a few seconds
            setTimeout(() => $('#paymentResult').fadeOut(), 5000);
        },
        error: function (xhr, status, error) {
            // Handle error response
            const errorMessage = xhr.responseText || error;
            $('#paymentResult')
                .removeClass('alert-success')
                .addClass('alert-danger')
                .text(`Failed to make payment: ${errorMessage}`)
                .fadeIn();

            console.error('Error making payment:', errorMessage);
        }
    });
});


//Get cumulativeRevenue
$(document).ready(function () {
    $('#getCumulativeRevenueBtn').click(function () {
        // AJAX request to get cumulative revenue data
        $.ajax({
            url: 'https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Payment/GetCumulativeRevenueOfAllStores', // API endpoint
            type: 'GET',
            contentType: 'application/json',
            success: function (data) {
                // Hide any previous error message
                $('#errorMessage').hide();

                // Check if data is available
                if (data.length > 0) {
                    // Show the table
                    $('#revenueTable').show();

                    // Clear any previous rows in the table
                    $('#revenueTable tbody').empty();

                    // Loop through the data and add rows to the table
                    data.forEach(function (payment) {
                        const row = `
                            <tr>
                                <td>${payment.staffId}</td>
                                <td>${payment.paymentDate}</td>
                                <td>${payment.amount}</td>
                                <td>${payment.customerId}</td>
                                <td>${payment.rentalId}</td>
                                <td>${payment.lastUpdate}</td>
                            </tr>
                        `;
                        $('#revenueTable tbody').append(row);
                    });
                } else {
                    // If no data is returned, show an alert
                    $('#errorMessage').text('No cumulative revenue data available at the moment.').show();
                }
            },
            error: function (xhr, status, error) {
                // Handle any errors in the request
                console.error('Error fetching cumulative revenue:', error);

                // Show error message
                $('#errorMessage').text('Failed to fetch cumulative revenue. Please try again later.').show();

                // Hide the table
                $('#revenueTable').hide();
            }
        });
    });
});


//Get Store Revenue
$(document).ready(function () {
    $('#getStoreRevenueBtn').click(function () {
        const storeId = $('#storeId').val().trim();

        // Validate input
        if (!storeId) {
            alert('Please provide a valid Store ID.');
            return;
        }

        // AJAX request to get cumulative revenue for a specific store
        $.ajax({
            url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Payment/GetCumulativeRevenueForAStore?storeId=${storeId}`, // API endpoint
            type: 'GET',
            contentType: 'application/json',
            success: function (data) {
                // Hide any previous error message
                $('#revenueResult').hide();

                // Check if data is available
                if (data.length > 0) {
                    // Show the table
                    $('#revenueTable1').show();

                    // Clear any previous rows in the table
                    $('#revenueTable1 tbody').empty();

                    // Loop through the data and add rows to the table
                    data.forEach(function (payment) {
                        const row = `
                            <tr>
                                <td>${payment.staffId}</td>
                                <td>${payment.paymentDate}</td>
                                <td>${payment.amount}</td>
                                <td>${payment.customerId}</td>
                                <td>${payment.rentalId}</td>
                                <td>${payment.lastUpdate}</td>
                            </tr>
                        `;
                        $('#revenueTable1 tbody').append(row);
                    });
                } else {
                    // If no data is returned, show an alert
                    $('#revenueResult').text('No cumulative revenue data available for this store.').show();
                }
            },
            error: function (xhr, status, error) {
                // Handle any errors in the request
                console.error('Error fetching cumulative revenue for the store:', error);

                // Show error message
                $('#revenueResult').text('Failed to fetch cumulative revenue. Please try again later.').show();

                // Hide the table
                $('#revenueTable1').hide();
            }
        });
    });
});


//Get payments by film
$(document).ready(function () {
    $('#getPaymentsByFilmTitleBtn').click(function () {
        const filmTitle = $('#filmTitle').val().trim();

        // Validate input
        if (!filmTitle) {
            alert('Please provide a valid Film Title.');
            return;
        }

        // AJAX request to get payments by film title
        $.ajax({
            url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Payment/GetPaymentsByFilmTitle?filmTitle=${encodeURIComponent(filmTitle)}`, // API endpoint
            type: 'GET',
            contentType: 'application/json',
            success: function (data) {
                // Hide any previous error message
                $('#paymentsResult').hide();

                // Check if data is available
                if (data.length > 0) {
                    // Show the table
                    $('#paymentsTable').show();

                    // Clear any previous rows in the table
                    $('#paymentsTable tbody').empty();

                    // Loop through the data and add rows to the table
                    data.forEach(function (payment) {
                        const row = `
                            <tr>
                                <td>${payment.paymentId}</td>
                                <td>${payment.staffId}</td>
                                <td>${payment.amount}</td>
                                <td>${payment.customerId}</td>
                                <td>${payment.rentalId}</td>
                                <td>${payment.paymentDate}</td>
                            </tr>
                        `;
                        $('#paymentsTable tbody').append(row);
                    });
                } else {
                    // If no data is returned, show an alert
                    $('#paymentsResult').text('No payments found for the provided film title.').show();
                }
            },
            error: function (xhr, status, error) {
                // Handle any errors in the request
                console.error('Error fetching payments by film title:', error);

                // Show error message
                $('#paymentsResult').text('Failed to fetch payments. Please try again later.').show();

                // Hide the table
                $('#paymentsTable').hide();
            }
        });
    });
});


//Get Cumulative Revenue
//$(document).ready(function () {
//    $('#getCumulativesRevenueBtn').click(function () {
//        const storeId = $('#storeId2').val().trim();

//        // Validate input
//        if (!storeId) {
//            alert('Please provide a valid Store ID.');
//            return;
//        }

//        // AJAX request to get cumulative revenue for a store
//        $.ajax({
//            url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Payment/GetCumulativeRevenueStoreWise?storeid=${encodeURIComponent(storeId)}`, // API endpoint
//            type: 'GET',
//            contentType: 'application/json',
//            success: function (data) {
//                // Hide any previous error message
//                $('#revenueResult2').hide();

//                // Check if data is available
//                if (data.length > 0) {
//                    // Show the table
//                    $('#revenueTable2').show();

//                    // Clear any previous rows in the table
//                    $('#revenueTable2 tbody').empty();

//                    // Loop through the data and add rows to the table
//                    data.forEach(function (payment) {
//                        const row = `
//                            <tr>
//                                <td>${payment.storeId}</td>
//                                <td>${payment.paymentDate}</td>
//                                <td>${payment.amount}</td>
//                                <td>${payment.customerId}</td>
//                                <td>${payment.rentalId}</td>
//                                <td>${payment.lastUpdate}</td>
//                            </tr>
//                        `;
//                        $('#revenueTable2 tbody').append(row);
//                    });
//                } else {
//                    // If no data is returned, show an alert
//                    $('#revenueResult2').text('No revenue data found for the provided Store ID.').show();
//                }
//            },
//            error: function (xhr, status, error) {
//                // Handle any errors in the request
//                console.error('Error fetching cumulative revenue:', error);

//                // Show error message
//                $('#revenueResult2').text('Failed to fetch cumulative revenue. Please try again later.').show();

//                // Hide the table
//                $('#revenueTable2').hide();
//            }
//        });
//    });
//});

$(document).ready(function () {
    $('#getCumulativesRevenueBtn').click(function () {
        const storeId = $('#storeId2').val().trim();

        // Validate input
        if (!storeId) {
            alert('Please provide a valid Store ID.');
            return;
        }

        // AJAX request to get cumulative revenue for a store
        $.ajax({
            url: `https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Payment/GetCumulativeRevenueStoreWise?storeid=${encodeURIComponent(storeId)}`, // API endpoint
            type: 'GET',
            contentType: 'application/json',
            success: function (data) {
                // Hide any previous error message
                $('#revenueResult2').hide();

                // Check if data is available
                if (data.length > 0) {
                    // Show the table
                    $('#revenueTable2').show();

                    // Clear any previous rows in the table
                    $('#revenueTable2 tbody').empty();

                    // Loop through the data and add rows to the table
                    data.forEach(function (payment) {
                        // Check if each field exists and is defined
                        const storeId1 = storeId || "N/A";  // Use "N/A" if undefined
                        const paymentDate = payment.paymentDate || "N/A"; // Use "N/A" if undefined
                        const amount = payment.amount || 0;  // Default to 0 if undefined
                        const customerId = payment.customerId || "N/A"; // Use "N/A" if undefined
                        const rentalId = payment.rentalId || "N/A"; // Use "N/A" if undefined
                        const lastUpdate = payment.lastUpdate || "N/A"; // Use "N/A" if undefined

                        // Create a table row with the relevant data
                        const row = `
                            <tr>
                                <td>${storeId1}</td>
                                <td>${paymentDate}</td>
                                <td>${amount}</td>
                                <td>${customerId}</td>
                                <td>${rentalId}</td>
                                <td>${lastUpdate}</td>
                            </tr>
                        `;
                        // Append the row to the table body
                        $('#revenueTable2 tbody').append(row);
                    });
                } else {
                    // If no data is returned, show an alert
                    $('#revenueResult2').text('No revenue data found for the provided Store ID.').show();
                    $('#revenueTable2').hide();
                }
            },
            error: function (xhr, status, error) {
                // Handle any errors in the request
                console.error('Error fetching cumulative revenue:', error);

                // Show error message
                $('#revenueResult2').text('Failed to fetch cumulative revenue. Please try again later.').show();

                // Hide the table
                $('#revenueTable2').hide();
            }
        });
    });
});


$('#getssCumulativeRevenueBtn').click(function () {
    // Trigger AJAX request when the button is clicked
    $.ajax({
        url: 'https://filmrentalstorewebapp-dbhjcwhje2ekaxb3.canadacentral-01.azurewebsites.net/api/Payment/GetCumulativeRevenueAllFilmsByStore', // Replace with your actual API endpoint
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {
            console.log(data);  // Check the data in the console for debugging

            if (data && data.length > 0) {
                // Hide any previous alert
                $('#revenueResult8').hide();
                // Show the revenue table
                $('#revenueTable8').show();

                // Clear any existing rows in the table body
                $('#revenueTable8 tbody').empty();

                // Loop through the data and append rows to the table
                data.forEach(function (item) {
                    const row = `
                        <tr>
                            <td>${item.title || 'N/A'}</td>
                            <td>${item.paymentId || 'N/A'}</td>
                            <td>${item.customerId || 'N/A'}</td>
                            <td>${item.staffId || 'N/A'}</td>
                            <td>${item.rentalId || 'N/A'}</td>
                            <td>${item.amount || 'N/A'}</td>
                            <td>${item.paymentDate ? new Date(item.paymentDate).toLocaleString() : 'N/A'}</td>
                            <td>${item.lastUpdate ? new Date(item.lastUpdate).toLocaleString() : 'N/A'}</td>
                        </tr>
                    `;
                    $('#revenueTable8 tbody').append(row);
                });
            } else {
                // Show an error message if no data is returned
                $('#revenueResult8').show().text('No revenue data found for any film across stores.');
                $('#revenueTable8').hide();
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            // Show error message if AJAX request fails
            $('#revenueResult8').show().text('An error occurred while fetching the revenue data.');
            $('#revenueTable8').hide();
        }
    });
});






