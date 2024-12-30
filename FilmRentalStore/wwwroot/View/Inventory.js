$(document).ready(function () {
    $('#addInventoryBtn').on('click', function () {
        // Get the values from the input fields
        const filmId = $('#filmId').val();
        const storeId = $('#storeId').val();

        // Get the current system time as ISO string for 'lastUpdate'
        const lastUpdate = new Date().toISOString();

        // Construct the JSON object from the input values
        const inventoryData = {
            filmId: filmId,
            storeId: storeId,
            lastUpdate: lastUpdate  // Automatically generated system time
        };

        // Make an AJAX request to add the inventory
        $.ajax({
            url: '/add-inventory',  // Assuming this is your endpoint to handle inventory addition
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(inventoryData),
            success: function (response) {
                // Show the response in the "responseDetails" section
                $('#responseDetails').text(JSON.stringify(response, null, 2));
                // Show a success alert
                alert('Inventory added successfully!');
            },
            error: function (xhr, status, error) {
                // Handle error
                alert('Failed to add inventory. Please try again.');
            }
        });
    });

    // Example button to fetch inventory data
 
});
//get count of no of copies of title
$(document).ready(function () {
    $('#getInventoryCountBtn').on('click', function () {
        //// Get the values from the input fields
        //const key = $('#key').val();
        //const value = $('#value').val();

        //// Construct the request data (JSON)
        //const requestData = {
        //    key: key,
        //    value: value
        //};

        // Make the GET request to the API with the required parameters
        $.ajax({
            url: 'https://localhost:7239/api/Inventory/Count',  // API endpoint
            type: 'GET',
            data: requestData,  // Send the data as query parameters
            success: function (response) {
                // Display the response in the "responseDetails" section
                $('#responseDetails').text(JSON.stringify(response, null, 2));
            },
            error: function (xhr, status, error) {
                // Handle error and display a message
                alert('Error fetching inventory count: ' + error);
            }
        });
    });
});
