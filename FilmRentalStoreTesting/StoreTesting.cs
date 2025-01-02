
﻿using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FilmRentalStore.Controllers;
using FilmRentalStore.DTO;
using FilmRentalStore.Services;
using FluentValidation;

using System;
using System.Threading.Tasks;
using FilmRentalStore.Models;

namespace FilmRentalStore.Tests
{
    public class StoreControllerTests
    {
        private readonly Mock<IStoreRepository> _storeRepositoryMock;
        private readonly Mock<IValidator<StoreDTO>> _storeValidatorMock;
        private readonly StoreController _controller;

        public StoreControllerTests()
        {
            _storeRepositoryMock = new Mock<IStoreRepository>();
            _storeValidatorMock = new Mock<IValidator<StoreDTO>>();
            _controller = new StoreController(_storeRepositoryMock.Object, _storeValidatorMock.Object);
        }
        #region AddStore
        [Fact]
        public async Task AddStore_ValidStoreDTO_ReturnsOk()
        {
            // Arrange
            var storeDto = new StoreDTO
            {
                StoreId = 1,
                ManagerStaffId = 2,
                AddressId = 3,
                LastUpdate = DateTime.UtcNow
            };

            // Mocking the validator to return a valid result (no validation errors)
            _storeValidatorMock.Setup(v => v.Validate(storeDto)).Returns(new FluentValidation.Results.ValidationResult());

            // Act
            var result = await _controller.Addstore(storeDto);
            var OkResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Record created successfully", OkResult.Value);

            // Assert

        }
        [Fact]
        public async Task AddStore_InvalidManagerStaffId_ReturnsBadRequest()
        {
            // Arrange
            var storeDto = new StoreDTO
            {
                StoreId = 1,
                ManagerStaffId = 0, // Invalid ManagerStaffId
                AddressId = 3,
                LastUpdate = DateTime.UtcNow
            };


            var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>

using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FilmRentalStoreTesting
{
    public class StoreTesting
    {
       
            private readonly Mock<IStoreRepository> _storeRepositoryMock;
            private readonly Mock<IValidator<StoreDTO>> _storeValidatorMock;
            private readonly StoreController _controller;

            public StoreTesting()
            {
                _storeRepositoryMock = new Mock<IStoreRepository>();
                _storeValidatorMock = new Mock<IValidator<StoreDTO>>();
                _controller = new StoreController(_storeRepositoryMock.Object, _storeValidatorMock.Object);
            }
            #region Add
            [Fact]
            public async Task AddStore_ValidStoreDTO_ReturnsOk()
            {
                // Arrange
                var storeDto = new StoreDTO
                {
                    StoreId = 1,
                    ManagerStaffId = 2,
                    AddressId = 3,
                    LastUpdate = DateTime.UtcNow
                };

                // Mocking the validator to return a valid result (no validation errors)
                _storeValidatorMock.Setup(v => v.Validate(storeDto)).Returns(new FluentValidation.Results.ValidationResult());

                // Act
                var result = await _controller.Addstore(storeDto);
                var OkResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal("Record created successfully", OkResult.Value);

                // Assert

            }
            [Fact]
            public async Task AddStore_InvalidManagerStaffId_ReturnsBadRequest()
            {
                // Arrange
                var storeDto = new StoreDTO
                {
                    StoreId = 1,
                    ManagerStaffId = 0, // Invalid ManagerStaffId
                    AddressId = 3,
                    LastUpdate = DateTime.UtcNow
                };


                var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>

    {
        new FluentValidation.Results.ValidationFailure("ManagerStaffId", "ManagerStaffId must be greater than 0.")
    });



            _storeValidatorMock.Setup(v => v.Validate(storeDto)).Returns(validationResult);

            // Act
            var result = await _controller.Addstore(storeDto);

            // Assert
            // Assert that the result is a BadRequestObjectResult
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            // Assert that the value of the BadRequestObjectResult is a List<ValidationFailure>
            var validationErrors = Assert.IsType<List<FluentValidation.Results.ValidationFailure>>(badRequestResult.Value);


            Assert.Single(validationErrors);
            var validationFailure = validationErrors[0];
            Assert.Equal("ManagerStaffId", validationFailure.PropertyName);
            Assert.Equal("ManagerStaffId must be greater than 0.", validationFailure.ErrorMessage);
        }
        #endregion

        #region getbycity
        [Fact]


        public async Task GetStoreByCity_ValidCity_ReturnsOk()
        {
            // Arrange
            var city = "New York";  // Valid city name
            var expectedStores = new List<StoreDTO>

                _storeValidatorMock.Setup(v => v.Validate(storeDto)).Returns(validationResult);

                // Act
                var result = await _controller.Addstore(storeDto);

                // Assert
                // Assert that the result is a BadRequestObjectResult
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

                // Assert that the value of the BadRequestObjectResult is a List<ValidationFailure>
                var validationErrors = Assert.IsType<List<FluentValidation.Results.ValidationFailure>>(badRequestResult.Value);


                Assert.Single(validationErrors);
                var validationFailure = validationErrors[0];
                Assert.Equal("ManagerStaffId", validationFailure.PropertyName);
                Assert.Equal("ManagerStaffId must be greater than 0.", validationFailure.ErrorMessage);
            }
            #endregion
            #region getbycity
            [Fact]


            public async Task GetStoreByCity_ValidCity_ReturnsOk()
            {
                // Arrange
                var city = "New York";  // Valid city name
                var expectedStores = new List<StoreDTO>

        {
            new StoreDTO { StoreId = 1, ManagerStaffId = 1, AddressId = 1, LastUpdate = DateTime.UtcNow },
            new StoreDTO { StoreId = 2, ManagerStaffId = 2, AddressId = 2, LastUpdate = DateTime.UtcNow }
        };


            // Mock the repository method to return the expected list of stores for the valid city
            _storeRepositoryMock.Setup(repo => repo.GetStoreByCity(city)).ReturnsAsync(expectedStores);

            // Act
            var result = await _controller.GetStoreByCity(city);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);  // Ensure the result is OK
            var returnedStores = Assert.IsType<List<StoreDTO>>(okResult.Value);  // Ensure the result is a List<StoreDTO>
            Assert.Equal(expectedStores.Count, returnedStores.Count);  // Ensure the count matches the expected stores
        }
        public async Task GetStoreByCity_InvalidCity_ReturnsBadRequest()
        {
            // Arrange
            var city = "";  // Invalid city name (empty string)
            var exceptionMessage = "City name cannot be empty.";

            // Mock the repository method to throw an exception when the city is invalid (empty string in this case)
            _storeRepositoryMock.Setup(repo => repo.GetStoreByCity(city)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetStoreByCity(city);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);  // Ensure the result is BadRequest
            Assert.Equal(exceptionMessage, badRequestResult.Value);  // Ensure the error message matches the exception
        }


        #endregion

        #region getcountry
        [Fact]
        public async Task GetStoreByCountry_ValidCountry_ReturnsOk()
        {
            // Arrange
            var country = "USA";
            var expectedStores = new List<StoreDTO>

                // Mock the repository method to return the expected list of stores for the valid city
                _storeRepositoryMock.Setup(repo => repo.GetStoreByCity(city)).ReturnsAsync(expectedStores);

                // Act
                var result = await _controller.GetStoreByCity(city);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);  // Ensure the result is OK
                var returnedStores = Assert.IsType<List<StoreDTO>>(okResult.Value);  // Ensure the result is a List<StoreDTO>
                Assert.Equal(expectedStores.Count, returnedStores.Count);  // Ensure the count matches the expected stores
            }
            [Fact]
            public async Task GetStoreByCity_InvalidCity_ReturnsBadRequest()
            {
                // Arrange
                var city = "";  // Invalid city name (empty string)
                var exceptionMessage = "City name cannot be empty.";

                // Mock the repository method to throw an exception when the city is invalid (empty string in this case)
                _storeRepositoryMock.Setup(repo => repo.GetStoreByCity(city)).ThrowsAsync(new Exception(exceptionMessage));

                // Act
                var result = await _controller.GetStoreByCity(city);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);  // Ensure the result is BadRequest
                Assert.Equal(exceptionMessage, badRequestResult.Value);  // Ensure the error message matches the exception
            }


            #endregion
            #region getcountry
            [Fact]
            public async Task GetStoreByCountry_ValidCountry_ReturnsOk()
            {
                // Arrange
                var country = "USA";
                var expectedStores = new List<StoreDTO>

    {
        new StoreDTO { StoreId = 1, ManagerStaffId = 1, AddressId = 1, LastUpdate = DateTime.UtcNow },
        new StoreDTO { StoreId = 2, ManagerStaffId = 2, AddressId = 2, LastUpdate = DateTime.UtcNow }
    };


            _storeRepositoryMock.Setup(repo => repo.GetStoreByCountry(country))
                                .ReturnsAsync(expectedStores);

            // Act
            var result = await _controller.GetStoreByCountry(country);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<StoreDTO>>(okResult.Value);
            Assert.Equal(expectedStores.Count, returnValue.Count);
        }
        [Fact]
        public async Task GetStoreByCountry_EmptyCountry_ReturnsBadRequest()
        {
            // Arrange

            // Arrange
            var country = "";  // Invalid city name (empty string)
            var exceptionMessage = "Country name cannot be empty.";

            // Mock the repository method to throw an exception when the city is invalid (empty string in this case)
            _storeRepositoryMock.Setup(repo => repo.GetStoreByCountry(country)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetStoreByCountry(country);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);  // Ensure the result is BadRequest
            Assert.Equal(exceptionMessage, badRequestResult.Value);  // Ensure the error message matches the exception
        }
        #endregion
        #region GetAllStaff
        [Fact]
        public async Task GetAllStaffOfStore__ReturnsOkWithStaffData()
        {
            // Arrange
            var storeId = 3;
            var expectedStaffData = new List<StaffDTO>

                _storeRepositoryMock.Setup(repo => repo.GetStoreByCountry(country))
                                    .ReturnsAsync(expectedStores);

                // Act
                var result = await _controller.GetStoreByCountry(country);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<List<StoreDTO>>(okResult.Value);
                Assert.Equal(expectedStores.Count, returnValue.Count);
            }
            [Fact]
            public async Task GetStoreByCountry_EmptyCountry_ReturnsBadRequest()
            {
                // Arrange

                // Arrange
                var country = "";  // Invalid city name (empty string)
                var exceptionMessage = "Country name cannot be empty.";

                // Mock the repository method to throw an exception when the city is invalid (empty string in this case)
                _storeRepositoryMock.Setup(repo => repo.GetStoreByCountry(country)).ThrowsAsync(new Exception(exceptionMessage));

                // Act
                var result = await _controller.GetStoreByCountry(country);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);  // Ensure the result is BadRequest
                Assert.Equal(exceptionMessage, badRequestResult.Value);  // Ensure the error message matches the exception
            }
            #endregion
            #region GetAllStaff
            [Fact]
            public async Task GetAllStaffOfStore_ValidStoreId_SingleStaff_ReturnsOkWithStaffData()
            {
                // Arrange
                var storeId = 3;
                var expectedStaffData = new List<StaffDTO>

    {
        new StaffDTO { StaffId = 1, FirstName = "Alice", LastName = "Johnson"}
    };


            _storeRepositoryMock.Setup(repo => repo.GetAllStaffOfStore(storeId))
                                .ReturnsAsync(expectedStaffData);

            // Act
            var result = await _controller.GetAllStaffOfStore(storeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<StaffDTO>>(okResult.Value);
            Assert.Single(returnValue); // Assert that the list contains only one staff member
        }
        [Fact]

        public async Task GetAllStaffOfStore_ReturnsEmptyList()
        {
            // Arrange
            var storeId = 5;  // A store ID that exists but has no staff
            var expectedStaffData = new List<StaffDTO>();  // Empty list


            _storeRepositoryMock.Setup(repo => repo.GetAllStaffOfStore(storeId))
                                  .ReturnsAsync(expectedStaffData);

            // Act
            var result = await _controller.GetAllStaffOfStore(storeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<StaffDTO>>(okResult.Value);
            Assert.Empty(returnValue);  // Assert that the list is empty

        }

        #endregion

        #region Assignadddress
        [Fact]
        public async Task AssignAddress_ValidData_ReturnsOkResult()
        {
            // Arrange
            int storeId = 1;
            var addressDto = new AddressDTO
            {
                AddressId = 101,
                Address1 = "123 Main St",
                Address2 = "Suite 4",
                District = "Downtown",
                CityId = 5,
                PostalCode = "12345",
                Phone = "555-1234",
                LastUpdate = DateTime.Now
            };
            _storeRepositoryMock.Setup(repo => repo.AssignAddress(storeId, addressDto))
                                .Returns(Task.CompletedTask);  // Simulate repository call

            // Act
            var result = await _controller.AssignAddress(storeId, addressDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("store & address details", okResult.Value);
        }

        [Fact]
        public async Task AssignAddress_InvalidStoreId_ReturnsBadRequest()
        {
            // Arrange
            int storeId = 9999;  // Non-existent store ID
            var addressDto = new AddressDTO
            {
                AddressId = 101,
                Address1 = "123 Main St",
                District = "Downtown",
                CityId = 5,
                PostalCode = "12345",
                Phone = "555-1234",
                LastUpdate = DateTime.Now
            };
            _storeRepositoryMock.Setup(repo => repo.AssignAddress(storeId, addressDto))
                                .Throws(new Exception("Store not found"));  // Simulate repository failure

            // Act
            var result = await _controller.AssignAddress(storeId, addressDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Store not found", badRequestResult.Value);
        }
        #endregion
        #region
        [Fact]

        public async Task GetAllCustomers_ValidStoreId_SingleCustomer_ReturnsOkResultWithSingleCustomer()
        {
            // Arrange
            int storeId = 1;  // Valid store ID
            var customers = new List<CustomerDTO>

                _storeRepositoryMock.Setup(repo => repo.GetAllStaffOfStore(storeId))
                                    .ReturnsAsync(expectedStaffData);

                // Act
                var result = await _controller.GetAllStaffOfStore(storeId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<List<StaffDTO>>(okResult.Value);
                Assert.Single(returnValue); // Assert that the list contains only one staff member
            }
            [Fact]

            public async Task GetAllStaffOfStore_StoreIdNotFound_ReturnsEmptyList()
            {
                // Arrange
                var storeId = 5;  // A store ID that exists but has no staff
                var expectedStaffData = new List<StaffDTO>();  // Empty list


                _storeRepositoryMock.Setup(repo => repo.GetAllStaffOfStore(storeId))
                                      .ReturnsAsync(expectedStaffData);

                // Act
                var result = await _controller.GetAllStaffOfStore(storeId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<List<StaffDTO>>(okResult.Value);
                Assert.Empty(returnValue);  // Assert that the list is empty
            }

            #endregion

            #region Assignadddress
            [Fact]
            public async Task AssignAddress_ValidData_ReturnsOkResult()
            {
                // Arrange
                int storeId = 1;
                var addressDto = new AddressDTO
                {
                    AddressId = 101,
                    Address1 = "123 Main St",
                    Address2 = "Suite 4",
                    District = "Downtown",
                    CityId = 5,
                    PostalCode = "12345",
                    Phone = "555-1234",
                    LastUpdate = DateTime.Now
                };
                _storeRepositoryMock.Setup(repo => repo.AssignAddress(storeId, addressDto))
                                    .Returns(Task.CompletedTask);  // Simulate repository call

                // Act
                var result = await _controller.AssignAddress(storeId, addressDto);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal("store & address details", okResult.Value);
            }

            [Fact]
            public async Task AssignAddress_InvalidStoreId_ReturnsBadRequest()
            {
                // Arrange
                int storeId = 9999;  // Non-existent store ID
                var addressDto = new AddressDTO
                {
                    AddressId = 101,
                    Address1 = "123 Main St",
                    District = "Downtown",
                    CityId = 5,
                    PostalCode = "12345",
                    Phone = "555-1234",
                    LastUpdate = DateTime.Now
                };
                _storeRepositoryMock.Setup(repo => repo.AssignAddress(storeId, addressDto))
                                    .Throws(new Exception("Store not found"));  // Simulate repository failure

                // Act
                var result = await _controller.AssignAddress(storeId, addressDto);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Store not found", badRequestResult.Value);
            }
            #endregion
            #region
            [Fact]

            public async Task GetAllCustomers_ValidStoreId_SingleCustomer_ReturnsOkResultWithSingleCustomer()
            {
                // Arrange
                int storeId = 1;  // Valid store ID
                var customers = new List<CustomerDTO>

    {
        new CustomerDTO { CustomerId = 1, FirstName = "John", LastName = "Doe" }
    };


            _storeRepositoryMock.Setup(repo => repo.GetAllCustomers(storeId))
                                .ReturnsAsync(customers);  // Simulate repository returning a single customer

            // Act
            var result = await _controller.GetAllCustomers(storeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CustomerDTO>>(okResult.Value);
            Assert.Single(returnValue);  // Ensure only 1 customer is returned
        }


        [Fact]
        public async Task GetAllCustomers_ZeroStoreId_ReturnsBadRequest()
        {
            // Arrange
            int storeId = 0;  // Store ID is zero, which is invalid
            _storeRepositoryMock.Setup(repo => repo.GetAllCustomers(storeId))
                                .Throws(new ArgumentException("Invalid store ID"));  // Simulate exception

            // Act
            var result = await _controller.GetAllCustomers(storeId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid store ID", badRequestResult.Value);
        }


        #endregion
        #region UpdatePhone
        [Fact]
        public async Task UpdatePhoneBystoreid_ValidStoreIdAndPhone_ReturnsOkResult()
        {
            // Arrange
            int storeId = 1;  // Valid store ID
            string newPhone = "123-456-7890";  // New phone number
            _storeRepositoryMock.Setup(repo => repo.UpdatePhoneBystoreid(storeId, newPhone))
                                .Returns(Task.CompletedTask);  // Simulate successful phone number update

            // Act
            var result = await _controller.UpdatePhoneBystoreid(storeId, newPhone);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Phone number updated successfully", okResult.Value);
        }

        [Fact]
        public async Task UpdatePhoneBystoreid_InvalidPhoneFormat_ReturnsBadRequest()
        {
            // Arrange
            int storeId = 1;  // Valid store ID
            string newPhone = "InvalidPhone";  // Invalid phone number format
            _storeRepositoryMock.Setup(repo => repo.UpdatePhoneBystoreid(storeId, newPhone))
                                .ThrowsAsync(new FormatException("Invalid phone number format"));  // Simulate invalid phone format

            // Act
            var result = await _controller.UpdatePhoneBystoreid(storeId, newPhone);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid phone number format", badRequestResult.Value);
        }
        #region 

        [Fact]
        public async Task GetAllManagerOfStore_ValidStoreIdWithNoStaff_ReturnsOkResult()
        {
            // Arrange
            int storeId = 1;  // Valid store ID
            var staffData = new List<StaffDTO>();  // Empty list, no staff

            // Mock the repository method to return an empty staff list
            _storeRepositoryMock.Setup(repo => repo.GetAllStaffOfStore(storeId))
                                .ReturnsAsync(staffData);

            // Act
            var result = await _controller.GetAllManagerOfStore(storeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<StaffDTO>>(okResult.Value);
            Assert.Empty(returnValue);  // Verify the list is empty
        }


        #endregion
        public async Task GetAllStaffOfStore_StoreIdNotFound_ReturnsEmptyList()
        {
            // Arrange
            var storeId = 5;  // A store ID that exists but has no staff
            var expectedStaffData = new List<StaffDTO>();  // Empty list


            _storeRepositoryMock.Setup(repo => repo.GetAllStaffOfStore(storeId))
                                  .ReturnsAsync(expectedStaffData);

            // Act
            var result = await _controller.GetAllStaffOfStore(storeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<StaffDTO>>(okResult.Value);
            Assert.Empty(returnValue);  // Assert that the list is empty

        }

        #endregion

        #region
        [Fact]
        public async Task AssignManager_ValidStoreIdAndStaffDTO_ReturnsOkResult()
        {
            // Arrange
            int storeId = 1;  // Valid store ID
            var staffDto = new StaffDTO
            {
                StaffId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Active = true,
                LastUpdate = DateTime.UtcNow,

            };

            _storeRepositoryMock.Setup(repo => repo.AssignManager(storeId, staffDto))
                                .Returns(Task.CompletedTask);  // Simulate successful manager assignment

            // Act
            var result = await _controller.AssignManager(storeId, staffDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("assigned successfully", okResult.Value);
        }
        [Fact]
        public async Task AssignManager_InvalidEmailFormat_ReturnsBadRequest()
        {
            // Arrange
            int storeId = 1;  // Valid store ID
            var staffDto = new StaffDTO
            {
                StaffId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "invalid-email",  // Invalid email format
                AddressId = 101,
                Active = true
            };

            _storeRepositoryMock.Setup(repo => repo.AssignManager(storeId, staffDto))
                                .Throws(new ArgumentException("Invalid email format"));

            // Act
            var result = await _controller.AssignManager(storeId, staffDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid email format", badRequestResult.Value);
        }
        #endregion


        [Fact]
        public async Task GetAllStaffAndStore_ReturnsOkResult()
        {
            // Arrange
            var mockData = new List<JoinDTO>

                _storeRepositoryMock.Setup(repo => repo.GetAllCustomers(storeId))
                                    .ReturnsAsync(customers);  // Simulate repository returning a single customer

                // Act
                var result = await _controller.GetAllCustomers(storeId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<List<CustomerDTO>>(okResult.Value);
                Assert.Single(returnValue);  // Ensure only 1 customer is returned
            }


            [Fact]
            public async Task GetAllCustomers_ZeroStoreId_ReturnsBadRequest()
            {
                // Arrange
                int storeId = 0;  // Store ID is zero, which is invalid
                _storeRepositoryMock.Setup(repo => repo.GetAllCustomers(storeId))
                                    .Throws(new ArgumentException("Invalid store ID"));  // Simulate exception

                // Act
                var result = await _controller.GetAllCustomers(storeId);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid store ID", badRequestResult.Value);
            }


            #endregion
            #region UpdatePhone
            [Fact]
            public async Task UpdatePhoneBystoreid_ValidStoreIdAndPhone_ReturnsOkResult()
            {
                // Arrange
                int storeId = 1;  // Valid store ID
                string newPhone = "123-456-7890";  // New phone number
                _storeRepositoryMock.Setup(repo => repo.UpdatePhoneBystoreid(storeId, newPhone))
                                    .Returns(Task.CompletedTask);  // Simulate successful phone number update

                // Act
                var result = await _controller.UpdatePhoneBystoreid(storeId, newPhone);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal("Phone number updated successfully", okResult.Value);
            }

            [Fact]
            public async Task UpdatePhoneBystoreid_InvalidPhoneFormat_ReturnsBadRequest()
            {
                // Arrange
                int storeId = 1;  // Valid store ID
                string newPhone = "InvalidPhone";  // Invalid phone number format
                _storeRepositoryMock.Setup(repo => repo.UpdatePhoneBystoreid(storeId, newPhone))
                                    .ThrowsAsync(new FormatException("Invalid phone number format"));  // Simulate invalid phone format

                // Act
                var result = await _controller.UpdatePhoneBystoreid(storeId, newPhone);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid phone number format", badRequestResult.Value);
            }


            [Fact]
            public async Task GetAllManagerOfStore_ValidStoreIdWithNoStaff_ReturnsOkResult()
            {
                // Arrange
                int storeId = 1;  // Valid store ID
                var staffData = new List<StaffDTO>();  // Empty list, no staff

                // Mock the repository method to return an empty staff list
                _storeRepositoryMock.Setup(repo => repo.GetAllStaffOfStore(storeId))
                                    .ReturnsAsync(staffData);

                // Act
                var result = await _controller.GetAllManagerOfStore(storeId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<List<StaffDTO>>(okResult.Value);
                Assert.Empty(returnValue);  // Verify the list is empty
            }


            #endregion
            #region AssignManager
            [Fact]
            public async Task AssignManager_ValidStoreIdAndStaffDTO_ReturnsOkResult()
            {
                // Arrange
                int storeId = 1;  // Valid store ID
                var staffDto = new StaffDTO
                {
                    StaffId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Active = true,
                    LastUpdate = DateTime.UtcNow,

                };

                _storeRepositoryMock.Setup(repo => repo.AssignManager(storeId, staffDto))
                                    .Returns(Task.CompletedTask);  // Simulate successful manager assignment

                // Act
                var result = await _controller.AssignManager(storeId, staffDto);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal("assigned successfully", okResult.Value);
            }
            [Fact]
            public async Task AssignManager_InvalidEmailFormat_ReturnsBadRequest()
            {
                // Arrange
                int storeId = 1;  // Valid store ID
                var staffDto = new StaffDTO
                {
                    StaffId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "invalid-email",  // Invalid email format
                    AddressId = 101,
                    Active = true
                };

                _storeRepositoryMock.Setup(repo => repo.AssignManager(storeId, staffDto))
                                    .Throws(new ArgumentException("Invalid email format"));

                // Act
                var result = await _controller.AssignManager(storeId, staffDto);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid email format", badRequestResult.Value);
            }
        #endregion

        #region GetAllStaffAndStore
        [Fact]
            public async Task GetAllStaffAndStore_ReturnsOkResult()
            {
                // Arrange
                var mockData = new List<JoinDTO>

        {
            new JoinDTO { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", City1 = "City A" },
            new JoinDTO { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", City1 = "City B" }
        };


            _storeRepositoryMock.Setup(repo => repo.GetAllStaffAndStore()).ReturnsAsync(mockData);

            // Act
            var result = await _controller.GetAllStaffAndStore();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnData = Assert.IsAssignableFrom<List<JoinDTO>>(okResult.Value);
            Assert.Equal(2, returnData.Count);
        }

        [Fact]
        public async Task GetAllStaffAndStore_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            _storeRepositoryMock.Setup(repo => repo.GetAllStaffAndStore()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetAllStaffAndStore();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }
    }
}


                _storeRepositoryMock.Setup(repo => repo.GetAllStaffAndStore()).ReturnsAsync(mockData);

                // Act
                var result = await _controller.GetAllStaffAndStore();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnData = Assert.IsAssignableFrom<List<JoinDTO>>(okResult.Value);
                Assert.Equal(2, returnData.Count);
            }

            [Fact]
            public async Task GetAllStaffAndStore_ReturnsBadRequest_WhenExceptionThrown()
            {
                // Arrange
                _storeRepositoryMock.Setup(repo => repo.GetAllStaffAndStore()).ThrowsAsync(new Exception("Test exception"));

                // Act
                var result = await _controller.GetAllStaffAndStore();

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Test exception", badRequestResult.Value);
            }
        #endregion
    }


}


