using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmRentalStore.Controllers;
using FilmRentalStore.DTO;
using FilmRentalStore.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FilmRentalStoreTesting
{
    public class RentalControllerTest
    {

        private readonly Mock<IRentalRepository> _mockRentalRepository;

        private readonly Mock<IValidator<RentalDTO>> _mockValidator;
        private readonly RentalController _controller;

        public RentalControllerTest()
        {
            _mockRentalRepository = new Mock<IRentalRepository>();

            _mockValidator = new Mock<IValidator<RentalDTO>>();
            _controller = new RentalController(_mockRentalRepository.Object, _mockValidator.Object);
        }
        // Renting a film
        [Fact]
        public async Task RentFilm_ValidRentalDTO_ReturnsOkResult()
        {
            // Arrange
            var rentalDTO = new RentalDTO
            {
                InventoryId = 22,
                CustomerId=25,
                StaffId=2,
                RentalDate = DateTime.Now
            };

            _mockRentalRepository.Setup(r => r.RentFilm(It.IsAny<RentalDTO>())).ReturnsAsync(rentalDTO);

            // Act
            var result = await _controller.RentFilm(rentalDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Check that the result is OkObjectResult
            Assert.Equal("Record Created Sucessfully", okResult.Value); // Assert the success message
        }


        [Fact]
        public async Task RentFilm_NullRentalDTO_ReturnsBadRequest()
        {
            // Arrange
            RentalDTO rentalDTO = null;

            // Act
            var result = await _controller.RentFilm(rentalDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Check that the result is BadRequest
            Assert.Equal("Rental data is required.", badRequestResult.Value); // Assert the error message
        }

        // GetFilmsRentedByCustomer
        [Fact]
        public async Task GetFilmsRentedByCustomer_ValidCustomerId_ReturnsListFilmDTO()
        {
            // Arrange
            var customerId = 1;
            var filmDTOs = new List<FilmDTO> { new FilmDTO() }; // Mock a sample list of FilmDTO
            _mockRentalRepository
                .Setup(r => r.GetFilmsRentedByCustomer(customerId))
                .ReturnsAsync(filmDTOs);

            // Act
            var result = await _controller.GetFilmsRentedByCustomer(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Check if the result is OkObjectResult
            Assert.NotNull(okResult.Value); // Ensure the response body is not null
            Assert.IsType<List<FilmDTO>>(okResult.Value); // Ensure the value is a list of FilmDTO
        }

        [Fact]
        public async Task GetFilmsRentedByCustomer_InvalidCustomerId_ReturnsBadRequest()
        {
            // Arrange
            var customerId = -1; // Example of invalid ID
            _mockRentalRepository
                .Setup(r => r.GetFilmsRentedByCustomer(customerId))
                .ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.GetFilmsRentedByCustomer(customerId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Ensure it's a BadRequestObjectResult
            Assert.NotNull(badRequestResult.Value); // Ensure the response body contains the error message
        }
        //GetTopTenRentedFilms
        [Fact]
        public async Task GetTopTenRentedFilms_ValidData_ReturnsListTop10RentedFilmDTO()
        {
            // Arrange
            var top10RentedFilmDTOs = new List<Top10RentedFilmDTO>
    {
        new Top10RentedFilmDTO { FilmId = 1, Title = "Film 1", RentalCount = 100 },
        new Top10RentedFilmDTO { FilmId = 2, Title = "Film 2", RentalCount = 80 }
    };
            _mockRentalRepository
                .Setup(r => r.GetTopTenRentedFilms())
                .ReturnsAsync(top10RentedFilmDTOs);

            // Act
            var result = await _controller.GetTopTenRentedFilms();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Ensure the result is OkObjectResult
            Assert.NotNull(okResult.Value); // Ensure the response body is not null
            Assert.IsType<List<Top10RentedFilmDTO>>(okResult.Value); // Ensure the value is a list of Top10RentedFilmDTO
            Assert.Equal(2, ((List<Top10RentedFilmDTO>)okResult.Value).Count); // Verify the count of returned items
        }

        [Fact]
        public async Task GetTopTenRentedFilms_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            _mockRentalRepository
                .Setup(r => r.GetTopTenRentedFilms())
                .ThrowsAsync(new Exception("Database error")); // Simulate an exception

            // Act
            var result = await _controller.GetTopTenRentedFilms();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Ensure the result is BadRequestObjectResult
            Assert.NotNull(badRequestResult.Value); // Ensure the response body contains the error message
            Assert.Equal("Database error", badRequestResult.Value); // Verify the error message
        }
        //GetTopTenFilmsByStore
        [Fact]
        public async Task GetTopTenFilmsByStore_ValidStoreId_ReturnsListTop10RentedFilmDTO()
        {
            // Arrange
            var storeId = 1;
            var top10RentedFilmDTOs = new List<Top10RentedFilmDTO>
    {
        new Top10RentedFilmDTO { FilmId = 1, Title = "Film 1", RentalCount = 50 },
        new Top10RentedFilmDTO { FilmId = 2, Title = "Film 2", RentalCount = 30 }
    };
            _mockRentalRepository
                .Setup(r => r.GetTopTenRentedFilmsByStore(storeId))
                .ReturnsAsync(top10RentedFilmDTOs);

            // Act
            var result = await _controller.GetTopTenFilmsByStore(storeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Ensure the result is OkObjectResult
            Assert.NotNull(okResult.Value); // Ensure the response body is not null
            Assert.IsType<List<Top10RentedFilmDTO>>(okResult.Value); // Ensure the value is a list of Top10RentedFilmDTO
            Assert.Equal(2, ((List<Top10RentedFilmDTO>)okResult.Value).Count); // Verify the count of returned items
        }

        [Fact]
        public async Task GetTopTenFilmsByStore_InvalidStoreId_ReturnsNotFound()
        {
            // Arrange
            var storeId = 99; // Example of an invalid store ID
            _mockRentalRepository
                .Setup(r => r.GetTopTenRentedFilmsByStore(storeId))
                .ReturnsAsync(new List<Top10RentedFilmDTO>()); // Simulate no data for the store

            // Act
            var result = await _controller.GetTopTenFilmsByStore(storeId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result); // Ensure the result is NotFoundObjectResult
            Assert.NotNull(notFoundResult.Value); // Ensure the response body contains the error message
            Assert.Equal("No rentals found for the given store.", notFoundResult.Value); // Verify the error message
        }


        [Fact]
        public async Task GetCustomersWithDueRentalsByStore_ValidStoreId_ReturnsListCustomerDTO()
        {
            // Arrange
            var storeId = 1;
            var customerDTOs = new List<CustomerDTO>
    {
        new CustomerDTO { CustomerId = 1, FirstName = "Customer 1" },
        new CustomerDTO { CustomerId = 2, FirstName = "Customer 2" }
    };
            _mockRentalRepository.Setup(r => r.GetCustomersWithDueRentalsByStore(storeId)).ReturnsAsync(customerDTOs);

            // Act
            var result = await _controller.GetCustomersWithDueRentalsByStore(storeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Ensure the result is OkObjectResult
            Assert.NotNull(okResult.Value); // Ensure the response body is not null
            Assert.IsType<List<CustomerDTO>>(okResult.Value); // Ensure the value is a list of CustomerDTO
            Assert.Equal(2, ((List<CustomerDTO>)okResult.Value).Count); // Verify the count of returned items
        }

        [Fact]
        public async Task GetCustomersWithDueRentalsByStore_InvalidStoreId_ReturnsNotFound()
        {
            // Arrange
            var storeId = 1;
            _mockRentalRepository.Setup(r => r.GetCustomersWithDueRentalsByStore(storeId)).ReturnsAsync(new List<CustomerDTO>()); // Simulate no customers with due rentals

            // Act
            var result = await _controller.GetCustomersWithDueRentalsByStore(storeId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result); // Ensure the result is NotFoundObjectResult
            Assert.NotNull(notFoundResult.Value); // Ensure the response body contains the error message
            Assert.Equal("No customers with due rentals found for the given store.", notFoundResult.Value); // Verify the error message
        }

        // UpdateReturnDate
        [Fact]
        public async Task UpdateReturnDate_ValidIdAndReturnDate_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var returnDate = DateTime.Now;
            var rentalDTO = new RentalDTO
            {
                RentalId = id,
                ReturnDate = returnDate,
                LastUpdate = DateTime.Now
            };
            _mockRentalRepository.Setup(r => r.UpdateReturnDate(id, returnDate)).ReturnsAsync(rentalDTO); // Mock the repository to return a valid RentalDTO

            // Act
            var result = await _controller.UpdateReturnDate(id, returnDate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Ensure the result is OkObjectResult
            Assert.NotNull(okResult.Value); // Check that the response body is not null
            Assert.IsType<RentalDTO>(okResult.Value); // Ensure the response body is of type RentalDTO
            var returnedRental = (RentalDTO)okResult.Value;
            Assert.Equal(id, returnedRental.RentalId); // Verify that the RentalId is the same as the input
        }

        [Fact]
        public async Task UpdateReturnDate_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            var returnDate = DateTime.Now;
            _mockRentalRepository.Setup(r => r.UpdateReturnDate(id, returnDate)).ReturnsAsync((RentalDTO)null); // Simulate the rental not being found

            // Act
            var result = await _controller.UpdateReturnDate(id, returnDate);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result); // Ensure the result is NotFound
            Assert.NotNull(notFoundResult.Value); // Check that the response body contains the error message
            Assert.Equal("Rental not found.", notFoundResult.Value); // Verify the error message
        }





    }
}
