using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;

using FilmRentalStore.Controllers;
using FilmRentalStore.DTO;
using FilmRentalStore.Services;
using FluentValidation;
using System;
using System.Threading.Tasks;
using FilmRentalStore.Models;


namespace FilmRentalStore.Tests
{
    public class InventoryTesting
    {
        private readonly Mock<IInventoryRepository> _inventoryRepositoryMock;
        private readonly Mock<IValidator<InventoryDTO>> _validatorMock;
        private readonly InventoryController _controller;

        public InventoryTesting()
        {
            _inventoryRepositoryMock = new Mock<IInventoryRepository>();
            _validatorMock = new Mock<IValidator<InventoryDTO>>();
            _controller = new InventoryController(_inventoryRepositoryMock.Object, _validatorMock.Object);
        }

       
        [Fact]
        public async Task AddFilm_ShouldReturnOk_WhenValidRequest()
        {
            // Arrange
            var inventoryDTO = new InventoryDTO
            {
                InventoryId = 1,
                FilmId = 2,
                StoreId = 3,
                LastUpdate = DateTime.Now
            };

            var validatorResult = new FluentValidation.Results.ValidationResult();
            _validatorMock.Setup(v => v.Validate(inventoryDTO)).Returns(validatorResult);
            _inventoryRepositoryMock.Setup(repo => repo.AddFilm(inventoryDTO)).Returns(Task.CompletedTask);

         
            var result = await _controller.AddFilm(inventoryDTO);

           
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("Record created successfully", okResult.Value);
        }

       
        [Fact]
        public async Task AddFilm_ShouldReturnBadRequest_WhenInvalidRequest()
        {
           
            var inventoryDTO = new InventoryDTO
            {
                InventoryId = 0,  
                FilmId = 0,      
                StoreId = 0,      
                LastUpdate = DateTime.Now
            };

            var validatorResult = new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure("FilmId", "FilmId must be greater than 0") }
            );
            _validatorMock.Setup(v => v.Validate(inventoryDTO)).Returns(validatorResult);

            
            var result = await _controller.AddFilm(inventoryDTO);

            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal(validatorResult.Errors, badRequestResult.Value);
        }
        // Test CountOfFilmasync method for a good request (successful operation)
        [Fact]

        public async Task CountOfFilmasync_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange: Define mock data that would be returned from the repository
            var expectedData = new List<KeyValuePair<string, int>>
    {
        new KeyValuePair<string, int>("Film 1", 10),
        new KeyValuePair<string, int>("Film 2", 20),
        new KeyValuePair<string, int>("Film 3", 30)
    };

            // Setup the mock to return the expected data when CountOfFilmasync is called
            _inventoryRepositoryMock.Setup(repo => repo.CountOfFilmasync())
                                    .ReturnsAsync(expectedData);

            // Act: Call the controller's method
            var result = await _controller.CountOfFilmasync();

            // Assert: Verify that the result is an OkObjectResult and matches the expected data
            Assert.IsType<OkObjectResult>(result);  // The result should be of type OkObjectResult
            var okResult = result as OkObjectResult;
            var inventoryData = okResult.Value as List<KeyValuePair<string, int>>;

            // Verify that the inventory data matches the expected data
            Assert.Equal(expectedData, inventoryData);
        }



        [Fact]
        public async Task CountOfFilmasync_ShouldReturnBadRequest_WhenExceptionThrown()
        {
          
            var exceptionMessage = "An error occurred while fetching the count.";
            _inventoryRepositoryMock.Setup(repo => repo.CountOfFilmasync()).ThrowsAsync(new Exception(exceptionMessage));

            
            var result = await _controller.CountOfFilmasync();

    
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal(exceptionMessage, badRequestResult.Value); 
        }
        [Fact]
        public async Task GetAllFilmsinaStore_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var storeId = 1;  // Example store ID
            var expectedData = new List<object>
    {
        new { FilmTitle = "Film 1", Copies = 10 },
        new { FilmTitle = "Film 2", Copies = 5 },
        new { FilmTitle = "Film 3", Copies = 8 }
    };

            // Setup mock for GetAllFilmsinaStore method in the repository
            _inventoryRepositoryMock.Setup(repo => repo.GetAllFilmsinaStore(storeId))
                                    .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.GetAllFilmsinaStore(storeId);

            // Assert
            Assert.IsType<OkObjectResult>(result);  
            var okResult = result as OkObjectResult;
            var actualData = okResult.Value as List<object>;

            Assert.Equal(expectedData.Count, actualData.Count);  
             
        }



        [Fact]
        public async Task GetAllFilmsinaStore_ShouldReturnBadRequest_WhenExceptionThrown()
        {
           
            var storeId = 1;  
            var exceptionMessage = "An error occurred while fetching films.";
            _inventoryRepositoryMock.Setup(repo => repo.GetAllFilmsinaStore(storeId)).ThrowsAsync(new Exception(exceptionMessage));  // Mock setup to throw an exception

        
            var result = await _controller.GetAllFilmsinaStore(storeId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); 
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal(exceptionMessage, badRequestResult.Value);  
        }
     
       
        [Fact]
        public async Task Getinventoryforallfilms_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var filmId = 1;  // Example film ID
            var exceptionMessage = "An error occurred while fetching inventory.";
            _inventoryRepositoryMock.Setup(repo => repo.Getinventoryforallfilms(filmId)).ThrowsAsync(new Exception(exceptionMessage));  // Mock setup to throw an exception

            // Act
            var result = await _controller.Getinventoryforallfilms(filmId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); // Expect BadRequestObjectResult
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal(exceptionMessage, badRequestResult.Value);  // Verify the error message matches the exception
        }
        [Fact]
        public async Task GetFilmCountInStore_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var filmId = 1;  // Example film ID
            var storeId = 1; // Example store ID
            var expectedFilmCount = 50;  // Example number of films in the store
            _inventoryRepositoryMock.Setup(repo => repo.GetFilmCountInStore(filmId, storeId)).ReturnsAsync(expectedFilmCount);  // Mock setup

            // Act
            var result = await _controller.GetFilmCountInStore(filmId, storeId);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Expect OkObjectResult
            var okResult = result as OkObjectResult;
            Assert.Equal(expectedFilmCount, okResult.Value);  // Verify the film count matches the expected count
        }

        // Test GetFilmCountInStore method for a bad request (exception thrown)
        [Fact]
        public async Task GetFilmCountInStore_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var filmId = 1;  // Example film ID
            var storeId = 1; // Example store ID
            var exceptionMessage = "An error occurred while fetching film count.";
            _inventoryRepositoryMock.Setup(repo => repo.GetFilmCountInStore(filmId, storeId)).ThrowsAsync(new Exception(exceptionMessage));  // Mock setup to throw an exception

            // Act
            var result = await _controller.GetFilmCountInStore(filmId, storeId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); // Expect BadRequestObjectResult
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal(exceptionMessage, badRequestResult.Value);  // Verify the error message matches the exception
        }
    }
}
