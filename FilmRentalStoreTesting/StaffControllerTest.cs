using System.Linq.Expressions;
using AutoMapper;
using FilmRentalStore.Controllers;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using FilmRentalStore.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FilmRentalStoreTesting
{
    public class StaffControllerTest
    {
        private readonly Mock<IStaffRepository> _mockStaffRepository;

        private readonly Mock<IValidator<StaffDTO>> _mockValidator;
        private readonly StaffController _controller;

        public StaffControllerTest()
        {
            _mockStaffRepository = new Mock<IStaffRepository>();

            _mockValidator = new Mock<IValidator<StaffDTO>>();
            _controller = new StaffController(_mockStaffRepository.Object, _mockValidator.Object);
        }
        //get staff
        [Fact]
        public async Task GetStaff_ReturnsOkResult_WhenStaffListExists()
        {
            // Arrange
            var staffList = new List<StaffDTO>
    {
        new StaffDTO { StaffId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
        new StaffDTO { StaffId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
    };

            // Mock the repository to return the staff list
            _mockStaffRepository.Setup(repo => repo.GetStaff())
                                .ReturnsAsync(staffList);

            // Act
            var result = await _controller.GetStaff();

            // Assert
            Assert.IsType<OkObjectResult>(result); // The response should be Ok
            var okResult = result as OkObjectResult;

            // Verify that the returned result contains the expected staff list
            Assert.NotNull(okResult.Value); // Ensure the response object is not null
            var returnedStaffList = okResult.Value as List<StaffDTO>;
            Assert.Equal(staffList.Count, returnedStaffList.Count); // Ensure the staff list count matches
            Assert.Equal(staffList, returnedStaffList); // Ensure the staff data matches
        }
        [Fact]
        public async Task GetStaff_ReturnsOk_WhenNoStaffExists()
        {
            // Arrange
            var staffList = new List<StaffDTO>(); // Empty staff list

            // Mock the repository to return an empty staff list
            _mockStaffRepository.Setup(repo => repo.GetStaff())
                                .ReturnsAsync(staffList);

            // Act
            var result = await _controller.GetStaff();

            // Assert
            Assert.IsType<OkObjectResult>(result); // The response should be Ok
            var okResult = result as OkObjectResult;

            // Ensure that the returned value is an empty list
            Assert.NotNull(okResult.Value); // Check that the result value is not null
            Assert.Empty(okResult.Value as List<StaffDTO>); // Check that the list is empty
        }



        // Adding Staff
        [Fact]
        public async Task AddStaff_ReturnsCreatedAtAction_WhenStaffIsValid()
        {
            // Arrange
            var staffDTO = new StaffDTO { StaffId = 1, FirstName = "John", LastName = "Doe" };

            // Mock the validation result to be valid
            var validationResult = new ValidationResult();
            _mockValidator.Setup(v => v.Validate(It.IsAny<StaffDTO>())).Returns(validationResult);

            // Mock the AddStaff method to simulate successful insertion
            _mockStaffRepository.Setup(repo => repo.AddStaff(It.IsAny<StaffDTO>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddStaff(staffDTO);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result); // Ensure it returns CreatedAtAction
            var createdResult = result as CreatedAtActionResult;
            Assert.Equal("GetStaff", createdResult.ActionName); // Ensure correct action name
            Assert.Equal(staffDTO, createdResult.Value); // Ensure the staffDTO is returned
        }


        [Fact]
        public async Task AddStaff_ReturnsBadRequest_WhenStaffIsInvalid()
        {
            // Arrange
            var staffDTO = new StaffDTO { StaffId = 1, FirstName = "", LastName = "Doe" }; // Invalid staffDTO (FirstName is empty)

            // Mock the validation result to be invalid
            var validationResult = new ValidationResult(new List<ValidationFailure>
    {
        new ValidationFailure("FirstName", "FirstName is required")
    });
            _mockValidator.Setup(v => v.Validate(It.IsAny<StaffDTO>())).Returns(validationResult);

            // Act
            var result = await _controller.AddStaff(staffDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); // Ensure it returns BadRequest
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal(validationResult.Errors, badRequestResult.Value); // Ensure the validation errors are returned
        }

        // get by lastname
        [Fact]
        public async Task GetStaffByLastName_ReturnsOkResult_WithEmptyList_WhenStaffDoesNotExist()
        {
            // Arrange
            _mockStaffRepository.Setup(repo => repo.GetStaffByLastName("Unknown"))
                .ReturnsAsync(new List<StaffDTO>());

            // Act
            var result = await _controller.GetStaffByLastName("Unknown");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Empty(okResult.Value as List<StaffDTO>);
        }

        [Fact]
        public async Task GetStaffByLastName_ReturnsBadRequest_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockStaffRepository.Setup(repo => repo.GetStaffByLastName("Unknown"))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetStaffByLastName("Unknown");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Database connection failed", badRequestResult.Value);
        }

        //GetStaffByFirstName
        [Fact]
        public async Task GetStaffByFirstName_ReturnsOkResult_WhenStaffExists()
        {
            // Arrange
            var staffList = new List<StaffDTO>
    {
        new StaffDTO { StaffId = 1, FirstName = "John", LastName = "Doe" },
        new StaffDTO { StaffId = 2, FirstName = "John", LastName = "Smith" }
    };

            _mockStaffRepository.Setup(repo => repo.GetStaffByFirstName("John"))
                .ReturnsAsync(staffList);

            // Act
            var result = await _controller.GetStaffByFirstName("John");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(staffList, okResult.Value);
        }
        [Fact]
        public async Task GetStaffByFirstName_ReturnsBadRequest_WhenFirstNameIsNullOrEmpty()
        {
            // Act
            var result = await _controller.GetStaffByFirstName(string.Empty);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("First name is required", badRequest.Value);
        }

        // GetStaffByEmail
        [Fact]
        public void GetStaffByEmail_ReturnsOkResult_WhenStaffExists()
        {
            // Arrange
            var staffList = new List<StaffDTO>
    {
        new StaffDTO { StaffId = 1, Email = "john.doe@example.com" },
        new StaffDTO { StaffId = 2, Email = "jane.doe@example.com" }
    };
            _mockStaffRepository.Setup(repo => repo.GetStaffByEmail("john.doe@example.com"))
                .ReturnsAsync(staffList);

            // Act
            var result = _controller.GetStaffByEmail("john.doe@example.com").Result;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(staffList, okResult.Value);
        }

        [Fact]
        public async Task GetStaffByEmail_ReturnsBadRequest_WhenEmailIsNullOrEmpty()
        {
            // Arrange
            string email = "";  // Empty email string

            // Act
            var result = await _controller.GetStaffByEmail(email);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); // Ensure it returns BadRequest
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Email is required", badRequestResult.Value); // Ensure the correct error message is returned
        }


        //AssignAddress
        [Fact]
        public async Task AssignAddress_ReturnsOkResult_WhenStaffIdAndAddressIdAreValid()
        {
            // Arrange
            var staffDTO = new StaffDTO { StaffId = 1, AddressId = 123 };
            var staffList = new List<StaffDTO> { new StaffDTO { StaffId = 1, AddressId = 123 } };

            _mockStaffRepository.Setup(repo => repo.AssignAddress(staffDTO))
                .ReturnsAsync(staffList);

            // Act
            var result = await _controller.AssignAddress(staffDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Expecting Ok result
            var okResult = result as OkObjectResult;
            Assert.Equal(staffList, okResult.Value); // Ensure the correct staff details are returned
        }
        [Fact]
        public async Task AssignAddress_ReturnsBadRequest_WhenStaffIdOrAddressIdIsInvalid()
        {
            // Arrange
            var invalidStaffDTO = new StaffDTO { StaffId = 0, AddressId = 0 }; // Invalid StaffId and AddressId

            // Act
            var result = await _controller.AssignAddress(invalidStaffDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); // Ensure it returns BadRequest
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("StaffId and AddressId are required.", badRequestResult.Value); // Ensure the correct error message is returned
        }
        //get staff by city
        [Fact]
        public async Task GetStaffByCity_ReturnsOkResult_WhenCityIsValid()
        {
            // Arrange
            var city = "Anytown";
            var staffList = new List<StaffDTO>
    {
        new StaffDTO { StaffId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", AddressId = 1 },
        new StaffDTO { StaffId = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", AddressId = 2 }
    };

            _mockStaffRepository.Setup(repo => repo.GetStaffByCity(city)).ReturnsAsync(staffList);

            // Act
            var result = await _controller.GetStaffByCity(city);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Expecting Ok result
            var okResult = result as OkObjectResult;
            Assert.Equal(staffList, okResult.Value); // Ensure the correct staff list is returned
        }
        [Fact]
        public async Task GetStaffByCity_ReturnsBadRequest_WhenCityIsInvalidOrEmpty()
        {
            // Arrange
            var city = ""; // Empty city

            // Act
            var result = await _controller.GetStaffByCity(city);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); // Expecting BadRequest result
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("City is required", badRequestResult.Value); // Ensure the correct error message is returned
        }
        // GetStaffByCountry

        [Fact]
        public async Task GetStaffByCountry_ReturnsOkResult_WhenStaffExists()
        {
            // Arrange
            var staffList = new List<StaffDTO>
    {
        new StaffDTO { StaffId = 1, FirstName = "John Doe", AddressId = 708 },
        new StaffDTO { StaffId = 2, FirstName= "Jane Doe", AddressId = 435 }
    };
            _mockStaffRepository.Setup(repo => repo.GetStaffByCountry("USA")).ReturnsAsync(staffList);

            // Act
            var result = await _controller.GetStaffByCountry("USA");

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(staffList, okResult.Value);
        }

        [Fact]
        public async Task GetStaffByCountry_ReturnsBadRequest_WhenCountryIsInvalidOrEmpty()
        {
            // Arrange
            var country = ""; // Empty country string

            // Act
            var result = await _controller.GetStaffByCountry(country);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); // Expecting BadRequest result
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("City is required", badRequestResult.Value); // Ensure the correct error message is returned
        }
        //GetStaffByPhoneNumber

        [Fact]
        public async Task GetStaffByPhoneNumber_ReturnsOkResult_WhenPhoneExists()
        {
            // Arrange
            var phone = "555-555-5555"; // Valid phone number
            var staffList = new List<StaffDTO>
    {
        new StaffDTO { StaffId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", AddressId = 1 },
        new StaffDTO { StaffId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", AddressId = 2 }
    };

            // Mock the repository call to return a staff list matching the phone number
            _mockStaffRepository.Setup(repo => repo.GetStaffByPhoneNumber(phone)).ReturnsAsync(staffList);

            // Act
            var result = await _controller.GetStaffByPhoneNumber(phone);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Expecting OkObjectResult
            var okResult = result as OkObjectResult;
            Assert.Equal(staffList, okResult.Value); // Ensure the correct staff list is returned
        }

        [Fact]
        public async Task GetStaffByPhoneNumber_ReturnsBadRequest_WhenPhoneIsInvalidOrEmpty()
        {
            // Arrange
            var phone = ""; // Empty phone string

            // Act
            var result = await _controller.GetStaffByPhoneNumber(phone);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); // Expecting BadRequest result
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("City is required", badRequestResult.Value); // Ensure the correct error message is returned
        }

        //
        [Fact]
        public async Task UpdateStaffByFirstName_ReturnsOkResult_WhenStaffDoesNotExist()
        {
            // Arrange
            var staffId = 999; // A non-existent staff ID
            var newFirstName = "John Updated"; // New first name
            _mockStaffRepository.Setup(repo => repo.UpdateStaffByFirstName(staffId, newFirstName))
                                .ReturnsAsync((List<StaffDTO>)null);  // Simulating no staff found

            // Act
            var result = await _controller.UpdateStaffByFirstName(staffId, newFirstName);

            // Assert
            Assert.IsType<OkObjectResult>(result); // The controller still returns Ok
            var okResult = result as OkObjectResult;
            Assert.Null(okResult.Value);  // No value is returned since staff does not exist
        }



        [Fact]
        public async Task UpdateStaffByFirstName_ReturnsOkResult_WhenStaffExists()
        {
            // Arrange
            var staffId = 1;
            var newFirstName = "John Updated"; // New valid first name
            var staff = new StaffDTO { StaffId = staffId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", AddressId = 1 };
            var updatedStaff = new StaffDTO { StaffId = staffId, FirstName = newFirstName, LastName = "Doe", Email = "john.doe@example.com", AddressId = 1 };

            // Mock the UpdateStaffByFirstName method to return updated staff
            _mockStaffRepository.Setup(repo => repo.UpdateStaffByFirstName(staffId, newFirstName))
                                .ReturnsAsync(new List<StaffDTO> { updatedStaff });

            // Act
            var result = await _controller.UpdateStaffByFirstName(staffId, newFirstName);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Expecting OkObjectResult
            var okResult = result as OkObjectResult;
            var returnedStaff = okResult.Value as List<StaffDTO>;

            // Ensure the updated staff is returned and the first name is updated
            Assert.Single(returnedStaff);  // Check only one staff item is returned
            Assert.Equal(newFirstName, returnedStaff[0].FirstName);  // Ensure the first name is updated
        }
        //UpdateStaffByLastName
        [Fact]
        public async Task UpdateStaffByLastName_ReturnsOkResult_WhenStaffExists()
        {
            // Arrange
            var staffId = 1; // Existing staff ID
            var newLastName = "Doe Updated"; // New last name
            var updatedStaffList = new List<StaffDTO>
    {
        new StaffDTO { StaffId = staffId, FirstName = "John", LastName = newLastName, Email = "john.doe@example.com", AddressId = 1 }
    };
            _mockStaffRepository.Setup(repo => repo.UpdateStaffByLastName(staffId, newLastName))
                                .ReturnsAsync(updatedStaffList);

            // Act
            var result = await _controller.UpdateStaffByLastName(staffId, newLastName);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Should return Ok
            var okResult = result as OkObjectResult;
            Assert.Equal(updatedStaffList, okResult.Value); // Should return the updated staff list
        }
        [Fact]
        public async Task UpdateStaffByLastName_ReturnsOkResult_WhenStaffDoesNotExist()
        {
            // Arrange
            var staffId = 999; // Non-existent staff ID
            var newLastName = "Doe Updated"; // New last name
            _mockStaffRepository.Setup(repo => repo.UpdateStaffByLastName(staffId, newLastName))
                                .ReturnsAsync((List<StaffDTO>)null); // Simulating no staff found

            // Act
            var result = await _controller.UpdateStaffByLastName(staffId, newLastName);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Controller still returns Ok
            var okResult = result as OkObjectResult;
            Assert.Null(okResult.Value); // The Value should be null since no staff was found
        }
        //UpdateStaffByEmail
        [Fact]
        public async Task UpdateStaffByEmail_ReturnsOkResult_WhenStaffExists()
        {
            // Arrange
            var staffId = 1; // Existing staff ID
            var email = "john.updated@example.com"; // New email address
            var updatedStaffList = new List<StaffDTO>
    {
        new StaffDTO { StaffId = staffId, FirstName = "John", LastName = "Doe", Email = email, AddressId = 1 }
    };
            _mockStaffRepository.Setup(repo => repo.UpdateStaffByEmail(staffId, email))
                                .ReturnsAsync(updatedStaffList);

            // Act
            var result = await _controller.UpdateStaffByEmail(staffId, email);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Should return Ok
            var okResult = result as OkObjectResult;
            Assert.Equal(updatedStaffList, okResult.Value); // Should return the updated staff list
        }

        [Fact]
        public async Task UpdateStaffByEmail_ReturnsOkResult_WhenStaffDoesNotExist()
        {
            // Arrange
            var staffId = 999; // Non-existent staff ID
            var email = "nonexistent@example.com"; // New email address
            _mockStaffRepository.Setup(repo => repo.UpdateStaffByEmail(staffId, email))
                                .ReturnsAsync((List<StaffDTO>)null); // Simulating no staff found

            // Act
            var result = await _controller.UpdateStaffByEmail(staffId, email);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Controller still returns Ok
            var okResult = result as OkObjectResult;
            Assert.Null(okResult.Value); // The Value should be null since no staff was found
        }
        //AssignStoreToStaff
        [Fact]
        public async Task AssignStoreToStaff_ReturnsOkResult_WhenStaffAndStoreExist()
        {
            // Arrange
            var staffId = 1; // Existing staff ID
            var storeId = 1; // Existing store ID
            var updatedStaffList = new List<StaffDTO>
    {
        new StaffDTO
        {
            StaffId = staffId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            AddressId = 2 // Example Address ID
        }
    };

            var storeDetails = new StoreDTO
            {
                StoreId = storeId,
                
            };

            // Mock repository to return updated staff and store details
            _mockStaffRepository.Setup(repo => repo.AssignStoreToStaff(staffId, storeId))
                                .ReturnsAsync(updatedStaffList);

            // Act
            var result = await _controller.AssignStoreToStaff(staffId, storeId);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Should return Ok
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(updatedStaffList, okResult.Value); // Verify updated staff list is returned
        }

        [Fact]
        public async Task AssignStoreToStaff_ReturnsOkResultWithEmptyData_WhenStaffOrStoreDoesNotExist()
        {
            // Arrange
            var staffId = 99; // Non-existent staff ID
            var storeId = 99; // Non-existent store ID
            var emptyList = new List<StaffDTO>(); // Repository returns an empty list

            // Mock repository to return an empty list when staff or store does not exist
            _mockStaffRepository.Setup(repo => repo.AssignStoreToStaff(staffId, storeId))
                                .ReturnsAsync(emptyList);

            // Act
            var result = await _controller.AssignStoreToStaff(staffId, storeId);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Should still return Ok
            var okResult = result as OkObjectResult;
            Assert.Equal(emptyList, okResult.Value); // Should return an empty list
        }


        //UpdatePhoneNumberByStaff
       
     








        [Fact]
        public async Task UpdatePhoneNumberByStaff_ReturnsBadRequest_WhenPhoneNumberIsNullOrEmpty()
        {
            // Arrange
            var staffId = 1; // Existing staff ID
            string newPhone = null; // Invalid phone number

            // Act
            var result = await _controller.UpdatePhoneNumberByStaff(staffId, newPhone);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); // Should return BadRequest
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Phone number is required.", badRequestResult.Value); // Verify error message
        }






    }

}