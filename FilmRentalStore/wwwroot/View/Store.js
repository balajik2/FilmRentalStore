// Utility function to get the authentication token
function getAuthToken() {
    return localStorage.getItem("authToken"); // Assuming the token is stored in local storage
}

// Fetch all stores with authentication
/*document.getElementById('getStoresBtn').addEventListener('click', function () {
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
                        <td>${store.storeName}</td>
                        <td>${store.address}</td>
                        <td>${store.phone}</td>
                        <td>${store.city}</td>
                        <td>${store.country}</td>
                        <td>${store.managerName}</td>
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
*/
// Add a new store with authentication
document.getElementById('addStoreBtn').addEventListener('click', function () {
    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

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
    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    const city = document.getElementById('city').value.trim();

    $.ajax({
        url: `https://localhost:7239/api/Store/GetByCity=${encodeURIComponent(city)}`,
        type: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function (response) {
            const tbody = document.querySelector("#searchStoreTable tbody");
            tbody.innerHTML = ""; // Clear table rows

            if (response.length === 0) {
                alert("No stores found in this city.");
                document.getElementById('searchStoreTable').style.display = 'none';
                return;
            }

            response.forEach(store => {
                const row = `
                    <tr>
                        <td>${store.storeId}</td>
                        <td>${store.storeName}</td>
                        <td>${store.address}</td>
                        <td>${store.phone}</td>
                        <td>${store.city}</td>
                        <td>${store.country}</td>
                        <td>${store.managerName}</td>
                        <td>${store.lastUpdate}</td>
                    </tr>
                `;
                tbody.innerHTML += row;
            });

            document.getElementById('searchStoreTable').style.display = 'table';
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch stores by city", xhr, status, error);
            alert(`Error: ${xhr.responseText || 'Unable to fetch stores by city'}`);
        }
    });
});

/*// Update store phone number with authentication
document.getElementById('updateStorePhoneBtn').addEventListener('click', function () {
    const token = getAuthToken();
    if (!token) return; // If no token, do not proceed

    const storeId = document.getElementById('updateStoreId').value;
    const phone = document.getElementById('updateStorePhone').value.trim();

    $.ajax({
        url: `https://localhost:7239/api/Store/updatephone`,
        type: "PUT",
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json"
        },
        data: JSON.stringify({
            storeId: storeId,
            phone: phone
        }),
        success: function (response) {
            document.getElementById("storeResponseDetails").textContent = JSON.stringify(response, null, 2);
            alert("Store phone updated successfully!");
        },
        error: function (xhr, status, error) {
            console.error("Failed to update store phone", xhr, status, error);
           
            alert(`Error: ${xhr.responseText || 'Unable to update store phone'}`);
        }
    });
});
*/