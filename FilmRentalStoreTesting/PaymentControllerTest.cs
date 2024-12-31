using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmRentalStore.Controllers;
using FilmRentalStore.DTO;
using FilmRentalStore.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FilmRentalStoreTesting
{
    public  class PaymentControllerTest
    {

        private readonly Mock<IPaymentRepository> _mockPaymentRepository;

        private readonly Mock<IValidator<PaymentDTO>> _mockValidator;
        private readonly PaymentController _controller;

        public PaymentControllerTest()
        {
            _mockPaymentRepository = new Mock<IPaymentRepository>();

            _mockValidator = new Mock<IValidator<PaymentDTO>>();
            _controller = new PaymentController(_mockPaymentRepository.Object, _mockValidator.Object);
        }
        //
        [Fact]
        public async Task MakePayment_ReturnsOkResult_WhenPaymentExistsAndAmountIsValid()
        {
            // Arrange
            var paymentId = 1; // Existing payment ID
            var amount = 200.00m; // New payment amount
            var updatedPaymentList = new List<PaymentDTO>
    {
        new PaymentDTO { PaymentId = paymentId, Amount = amount, StaffId = 1 }
    };

            _mockPaymentRepository.Setup(repo => repo.MakePayment(paymentId, amount))
                                  .ReturnsAsync(updatedPaymentList);

            // Act
            var result = await _controller.MakePayment(paymentId, amount);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Should return Ok
            var okResult = result as OkObjectResult;

            // Cast the result value to dynamic for accessing properties
            var response = okResult.Value as List<PaymentDTO>;

            // Assert the response contains the updated payment details
            Assert.NotNull(response);
            Assert.Single(response);
            Assert.Equal(paymentId, response[0].PaymentId);
            Assert.Equal(amount, response[0].Amount);
        }
        [Fact]
        public async Task MakePayment_ThrowsInvalidOperationException_WhenAmountIsSameAsCurrentAmount()
        {
            // Arrange
            var paymentId = 1; // Existing payment ID
            var currentAmount = 100.00m; // Current amount in the payment
            var sameAmount = currentAmount; // New amount is the same as current amount

            _mockPaymentRepository.Setup(repo => repo.MakePayment(paymentId, sameAmount))
                                  .ThrowsAsync(new InvalidOperationException("The new amount is the same as the current amount. No update needed."));

            // Act
            var result = await _controller.MakePayment(paymentId, sameAmount);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); // Should return BadRequest
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("The new amount is the same as the current amount. No update needed.", badRequestResult.Value);
        }
        //
        [Fact]
        public async Task GetCumulativeRevenueOfAllStores_ReturnsOkResult_WhenDataExists()
        {
            // Arrange
            var revenueData = new List<PaymentDTO>
    {
        new PaymentDTO
        {
            StaffId = 1,
            PaymentDate = DateTime.Now.Date,
            Amount = 100.00m,
            CustomerId = 1,
            RentalId = 1,
            LastUpdate = DateTime.Now
        }
    };

            // Mock the data returned from the database
            _mockPaymentRepository.Setup(repo => repo.GetCumulativeRevenueOfAllStores())
                                  .ReturnsAsync(revenueData);

            // Act
            var result = await _controller.GetCumulativeRevenueOfAllStores();

            // Assert
            Assert.IsType<OkObjectResult>(result); // Should return Ok
            var okResult = result as OkObjectResult;
            Assert.Equal(revenueData, okResult.Value); // Should return the expected revenue data
        }
        [Fact]
        public async Task GetCumulativeRevenueOfAllStores_ReturnsNotFoundResult_WhenNoDataExists()
        {
            // Arrange
            var revenueData = new List<PaymentDTO>(); // Empty list to simulate no data

            // Mock the data returned from the database
            _mockPaymentRepository.Setup(repo => repo.GetCumulativeRevenueOfAllStores())
                                  .ReturnsAsync(revenueData);

            // Act
            var result = await _controller.GetCumulativeRevenueOfAllStores();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result); // Should return NotFound
            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal("No revenue data found.", notFoundResult.Value); // Validate message
        }
        //GetCumulativeRevenueForAStore
        [Fact]
        public async Task GetCumulativeRevenueForAStore_ReturnsOkResult_WhenDataExists()
        {
            // Arrange
            var storeId = 1; // Existing store ID
            var revenueData = new List<PaymentDTO>
    {
        new PaymentDTO
        {
            StaffId = storeId,
            PaymentDate = DateTime.Now.Date,
            Amount = 100.00m,
            CustomerId = 1,
            RentalId = 1,
            LastUpdate = DateTime.Now
        }
    };

            // Mock the repository method to return the expected data
            _mockPaymentRepository.Setup(repo => repo.GetCumulativeRevenueForAStore(storeId))
                                  .ReturnsAsync(revenueData);

            // Act
            var result = await _controller.GetCumulativeRevenueForAStore(storeId);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Should return Ok
            var okResult = result as OkObjectResult;
            Assert.Equal(revenueData, okResult.Value); // Verify the returned data
        }

        [Fact]
        public async Task GetCumulativeRevenueForAStore_ReturnsOkResult_WhenNoDataExistsForStore()
        {
            // Arrange
            var storeId = 1; // Store with no data
            var revenueData = new List<PaymentDTO>(); // Empty list simulating no data

            // Mock the repository method to return an empty list
            _mockPaymentRepository.Setup(repo => repo.GetCumulativeRevenueForAStore(storeId))
                                  .ReturnsAsync(revenueData);

            // Act
            var result = await _controller.GetCumulativeRevenueForAStore(storeId);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Should return Ok (not NotFound)
            var okResult = result as OkObjectResult;

            // Verify the returned empty list (simulating no data found)
            Assert.Equal(revenueData, okResult.Value); // Should return the empty list
        }
        //GetPaymentsByFilmTitle
        [Fact]
        public async Task GetPaymentsByFilmTitle_ReturnsOkResult_WhenFilmTitleExists()
        {
            // Arrange
            var filmTitle = "Inception"; // Existing film title
            var payments = new List<PaymentWithFilmDTO>
    {
        new PaymentWithFilmDTO
        {
            Title = "Inception",
            PaymentId = 1,
            CustomerId = 1,
            StaffId = 2,
            RentalId = 3,
            Amount = 10.0M,
            PaymentDate = DateTime.Now,
            LastUpdate = DateTime.Now
        }
    };

            // Mock the repository to return the payments list
            _mockPaymentRepository.Setup(repo => repo.GetPaymentsByFilmTitle(filmTitle))
                                  .ReturnsAsync(payments);

            // Act
            var result = await _controller.GetPaymentsByFilmTitle(filmTitle);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Should return Ok
            var okResult = result as OkObjectResult;

            // Verify the data returned in the response
            Assert.Equal(payments, okResult.Value); // Should return the payments list
        }
        [Fact]
        public async Task GetPaymentsByFilmTitle_ReturnsNotFoundResult_WhenFilmTitleDoesNotExist()
        {
            // Arrange
            var filmTitle = "NonExistentFilm"; // Non-existing film title
            var payments = new List<PaymentWithFilmDTO>(); // Empty list as no payments are found

            // Mock the repository to return an empty list
            _mockPaymentRepository.Setup(repo => repo.GetPaymentsByFilmTitle(filmTitle))
                                  .ReturnsAsync(payments);

            // Act
            var result = await _controller.GetPaymentsByFilmTitle(filmTitle);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Should return Ok (even if the list is empty)
            var okResult = result as OkObjectResult;

            // Verify the empty list response
            Assert.Empty(okResult.Value as List<PaymentWithFilmDTO>); // Should return an empty list
        }
        //GetCumulativeRevenueStoreWise
        [Fact]
        public async Task GetCumulativeRevenueStoreWise_ReturnsNotFoundResult_WhenStoreHasNoPayments()
        {
            // Arrange
            var storeId = 1; // Store with no payments
            var payments = new List<PaymentWithAddressDTO>(); // Empty list as no payments exist

            // Mock the repository method to return an empty list
            _mockPaymentRepository.Setup(repo => repo.GetCumulativeRevenueStoreWise(storeId))
                                  .ReturnsAsync(payments);

            // Act
            var result = await _controller.GetCumulativeRevenueStoreWise(storeId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result); // Should return NotFound
            var notFoundResult = result as NotFoundObjectResult;

            // Verify the NotFound message
            Assert.Equal($"No payments found for Store with ID {storeId}.", notFoundResult.Value); // Ensure the correct error message is returned
        }
        [Fact]
        public async Task GetCumulativeRevenueStoreWise_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var storeId = 999; // Assuming this store ID does not exist in the database

            // Mock the repository method to throw an exception
            _mockPaymentRepository.Setup(repo => repo.GetCumulativeRevenueStoreWise(storeId))
                                  .ThrowsAsync(new Exception("An error occurred while fetching cumulative revenue."));

            // Act
            var result = await _controller.GetCumulativeRevenueStoreWise(storeId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); // Should return BadRequest
            var badRequestResult = result as BadRequestObjectResult;

            // Verify the error message
            Assert.Equal("Error occurred: An error occurred while fetching cumulative revenue.", badRequestResult.Value); // Ensure the error message is returned
        }
        // GetCumulativeRevenueAllFilmsByStore
        [Fact]
        public async Task GetCumulativeRevenueAllFilmsByStore_ReturnsOkResult_WhenRevenueDataExists()
        {
            // Arrange
            var filmsRevenue = new List<PaymentWithFilmDTO>
    {
        new PaymentWithFilmDTO
        {
            Title = "Film A",
            PaymentId = 1,
            CustomerId = 1,
            StaffId = 1,
            RentalId = 1,
            Amount = 50.0M,
            PaymentDate = DateTime.Now,
            LastUpdate = DateTime.Now
        },
        new PaymentWithFilmDTO
        {
            Title = "Film B",
            PaymentId = 2,
            CustomerId = 2,
            StaffId = 1,
            RentalId = 2,
            Amount = 40.0M,
            PaymentDate = DateTime.Now,
            LastUpdate = DateTime.Now
        }
    };

            // Mock the repository to return the list of revenue data
            _mockPaymentRepository.Setup(repo => repo.GetCumulativeRevenueAllFilmsByStore())
                                  .ReturnsAsync(filmsRevenue);

            // Act
            var result = await _controller.GetCumulativeRevenueAllFilmsByStore();

            // Assert
            Assert.IsType<OkObjectResult>(result); // Should return Ok
            var okResult = result as OkObjectResult;

            // Verify the returned films revenue data
            Assert.Equal(filmsRevenue, okResult.Value); // Should return the correct revenue data
        }
        [Fact]
        public async Task GetCumulativeRevenueAllFilmsByStore_ReturnsNotFoundResult_WhenNoDataExists()
        {
            // Arrange
            var filmsRevenue = new List<PaymentWithFilmDTO>(); // Empty list as no revenue data exists

            // Mock the repository to return an empty list
            _mockPaymentRepository.Setup(repo => repo.GetCumulativeRevenueAllFilmsByStore())
                                  .ReturnsAsync(filmsRevenue);

            // Act
            var result = await _controller.GetCumulativeRevenueAllFilmsByStore();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result); // Should return NotFound
            var notFoundResult = result as NotFoundObjectResult;

            // Verify the NotFound message
            Assert.Equal("No revenue data found for any film across stores.", notFoundResult.Value); // Ensure the correct error message is returned
        }


    }
}
