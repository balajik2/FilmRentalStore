using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FilmRentalStore.Controllers;
using FilmRentalStore.DTO;
using FilmRentalStore.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using Xunit;

namespace FilmRentalStoreTesting
{
    public class CustomerTesting
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IValidator<CustomerDTO>> _mockValidator;
        private readonly CustomerController _controller;

        public CustomerTesting()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockValidator = new Mock<IValidator<CustomerDTO>>();
            _controller = new CustomerController(_mockCustomerRepository.Object, _mockValidator.Object);
        }



        #region AddCustomer

        [Fact]
        public async Task AddCustomer_ReturnsCreatedAtAction_WhenCustomerIsValid()
        {
            // Arrange
            var customerDto = new CustomerDTO
            {
                CustomerId = 609,
                StoreId = 2,
                FirstName = "MITCHELL",
                LastName = "STARC",
                Email = "MITCHEL.SMITH@sakilacustomer.org",
                AddressId = 6,
                Active = "1",
                CreateDate = DateTime.Parse("2006-02-14T22:04:36"),
                LastUpdate = DateTime.Parse("2006-02-15T04:57:20")
            };

            _mockValidator
                .Setup(validator => validator.Validate(It.IsAny<CustomerDTO>()))
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockCustomerRepository
                .Setup(repo => repo.AddCustomer(It.IsAny<CustomerDTO>()))
                .ReturnsAsync(customerDto);

            // Act
            var result = await _controller.AddCustomer(customerDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetCustomer", createdAtActionResult.ActionName);
            var returnedCustomer = Assert.IsType<CustomerDTO>(createdAtActionResult.Value);
            Assert.Equal(customerDto.CustomerId, returnedCustomer.CustomerId);
            Assert.Equal(customerDto.FirstName, returnedCustomer.FirstName);
            Assert.Equal(customerDto.LastName, returnedCustomer.LastName);
        }



        [Fact]
        public async Task AddCustomer_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var customerDto = new CustomerDTO
            {
                CustomerId = 1,
                StoreId = 0, // Invalid StoreId (required, non-zero)
                FirstName = "John",
                LastName = "conner",
                Email = "john@google.com",
                AddressId = 0, // Invalid AddressId (required, non-zero)
                Active = "1",
                CreateDate = DateTime.Now,
                LastUpdate = DateTime.Now
            };

            var validationFailures = new List<ValidationFailure>
    {
        new ValidationFailure("StoreId", "'Store Id' must not be empty."),
        new ValidationFailure("AddressId", "AddressId is required")
    };

            var validationResult = new FluentValidation.Results.ValidationResult(validationFailures);

            _mockValidator
                .Setup(validator => validator.Validate(It.IsAny<CustomerDTO>()))
                .Returns(validationResult);

            // Act
            var result = await _controller.AddCustomer(customerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<List<ValidationFailure>>(badRequestResult.Value);

            Assert.Equal(2, errors.Count);
            Assert.Equal("StoreId", errors[0].PropertyName);
            Assert.Equal("'Store Id' must not be empty.", errors[0].ErrorMessage);
            Assert.Equal("AddressId", errors[1].PropertyName);
            Assert.Equal("AddressId is required", errors[1].ErrorMessage);
        }

        #endregion




        #region GetCustomerByLastName

        [Fact]
        public async Task GetCustomerByLastName_ReturnsOk_WhenLastNameIsValid()
        {
            // Arrange
            var lastName = "MILLER";
            var customers = new List<CustomerDTO>
    {
        new CustomerDTO
        {
            CustomerId = 7,
            StoreId = 1,
            FirstName = "MARIA",
            LastName = "MILLER",
            Email = "MARIA.MILLER@sakilacustomer.org",
            AddressId = 11,
            Active = "1",
            CreateDate = DateTime.Parse("2006-02-14T22:04:36"),
            LastUpdate = DateTime.Parse("2006-02-15T04:57:20")
        }
    };

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomerByLastName(lastName))
                .ReturnsAsync(customers);

            // Act
            var result = await _controller.GetCustomerByLastName(lastName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsType<List<CustomerDTO>>(okResult.Value);
            Assert.Single(returnedCustomers);

            var customer = returnedCustomers.First();
            Assert.Equal("MARIA", customer.FirstName);
            Assert.Equal("MILLER", customer.LastName);
            Assert.Equal(7, customer.CustomerId);
            Assert.Equal("MARIA.MILLER@sakilacustomer.org", customer.Email);
            Assert.Equal(1, customer.StoreId);
            Assert.Equal(11, customer.AddressId);
            Assert.Equal("1", customer.Active);
            Assert.Equal(DateTime.Parse("2006-02-14T22:04:36"), customer.CreateDate);
            Assert.Equal(DateTime.Parse("2006-02-15T04:57:20"), customer.LastUpdate);
        }



        [Fact]
        public async Task GetCustomerByLastName_ReturnsBadRequest_WhenLastNameIsNotProvided()
        {
            // Arrange: Pass an empty or null value for lastname (which is required)
            string lastName = string.Empty;  // Simulate a missing last name

            // Act: Call the controller with an invalid last name (empty string or null)
            var result = await _controller.GetCustomerByLastName(lastName);

            // Assert: Check if the controller returns a BadRequest response
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);

            // Assert: Ensure the error message is as expected
            Assert.Equal("'lastname' is required.", errorMessage);
        }

        #endregion


        #region GetCustomerByFirstName
        [Fact]
        public async Task GetCustomerByFirstName_ReturnsOk_WhenFirstNameIsValid()
        {
            // Arrange
            var firstName = "MARIA";
            var customers = new List<CustomerDTO>
    {
        new CustomerDTO
        {
            CustomerId = 7,
            StoreId = 1,
            FirstName = "MARIA",
            LastName = "MILLER",
            Email = "MARIA.MILLER@sakilacustomer.org",
            AddressId = 11,
            Active = "1",
            CreateDate = DateTime.Parse("2006-02-14T22:04:36"),
            LastUpdate = DateTime.Parse("2006-02-15T04:57:20")
        }
    };

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomerByFirstName(firstName))
                .ReturnsAsync(customers);

            // Act
            var result = await _controller.GetCustomerByFirstName(firstName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsType<List<CustomerDTO>>(okResult.Value);
            Assert.Single(returnedCustomers);

            var customer = returnedCustomers.First();
            Assert.Equal("MARIA", customer.FirstName);
            Assert.Equal("MILLER", customer.LastName);
            Assert.Equal(7, customer.CustomerId);
            Assert.Equal("MARIA.MILLER@sakilacustomer.org", customer.Email);
            Assert.Equal(1, customer.StoreId);
            Assert.Equal(11, customer.AddressId);
            Assert.Equal("1", customer.Active);
            Assert.Equal(DateTime.Parse("2006-02-14T22:04:36"), customer.CreateDate);
            Assert.Equal(DateTime.Parse("2006-02-15T04:57:20"), customer.LastUpdate);
        }



        [Fact]
        public async Task GetCustomerByFirstName_ReturnsBadRequest_WhenFirstNameIsNotProvided()
        {
            // Arrange: Pass an empty or null value for firstName (which is required)
            string firstName = string.Empty;  // Simulate a missing first name

            // Act: Call the controller with an invalid first name (empty string or null)
            var result = await _controller.GetCustomerByFirstName(firstName);

            // Assert: Check if the controller returns a BadRequest response
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);

            // Assert: Ensure the error message is as expected
            Assert.Equal("'firstName' is required.", errorMessage);
        }

        #endregion



        #region  GetCustomerByEmail

        [Fact]
        public async Task GetCustomerByEmail_ReturnsOk_WhenEmailIsValid()
        {
            // Arrange
            var email = "MARIA.MILLER@sakilacustomer.org";
            var customer = new CustomerDTO
            {
                CustomerId = 7,
                StoreId = 1,
                FirstName = "MARIA",
                LastName = "MILLER",
                Email = "MARIA.MILLER@sakilacustomer.org",
                AddressId = 11,
                Active = "1",
                CreateDate = DateTime.Parse("2006-02-14T22:04:36"),
                LastUpdate = DateTime.Parse("2006-02-15T04:57:20")
            };

            // Setup mock to return a single CustomerDTO
            _mockCustomerRepository
                .Setup(repo => repo.GetCustomerByEmail(email))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerByEmail(email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomer = Assert.IsType<CustomerDTO>(okResult.Value);
            Assert.Equal("MARIA", returnedCustomer.FirstName);
            Assert.Equal("MILLER", returnedCustomer.LastName);
            Assert.Equal(7, returnedCustomer.CustomerId);
            Assert.Equal("MARIA.MILLER@sakilacustomer.org", returnedCustomer.Email);
            Assert.Equal(1, returnedCustomer.StoreId);
            Assert.Equal(11, returnedCustomer.AddressId);
            Assert.Equal("1", returnedCustomer.Active);
            Assert.Equal(DateTime.Parse("2006-02-14T22:04:36"), returnedCustomer.CreateDate);
            Assert.Equal(DateTime.Parse("2006-02-15T04:57:20"), returnedCustomer.LastUpdate);

        }



        [Fact]
        public async Task GetCustomerByEmail_ReturnsBadRequest_WhenEmailIsNotProvided()
        {
            // Arrange: Pass an empty or null value for email (which is required)
            string email = string.Empty;  // Simulate a missing or invalid email

            // Act: Call the controller with an invalid email (empty string or null)
            var result = await _controller.GetCustomerByEmail(email);

            // Assert: Check if the controller returns a BadRequest response
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);

            // Assert: Ensure the error message is as expected
            Assert.Equal("'email' is required.", errorMessage);
        }


        #endregion



        #region GetCustomerByCity

        [Fact]
        public async Task GetCustomerByCity_ReturnsOk_WhenCustomersFound()
        {
            // Arrange
            var city = "Kimberley";
            var mockCustomers = new List<CustomerwithAddressDTO>
    {
        new CustomerwithAddressDTO
        {
            CustomerId = 19,
            StoreId = 1,
            FirstName = "RUTH",
            LastName = "MARTINEZ",
            Email = "RUTH.MARTINEZ@sakilacustomer.org",
            AddressId = 23,
            Active = "1",
            CreateDate = new DateTime(2006, 2, 14),
            LastUpdate = new DateTime(2006, 2, 15),
            Address1 = "1417 Lancaster Avenue",
            Address2 = null,
            District = " ",
            CityId = 267,
            PostalCode = "72192",
            Phone = " "
        }
    };

            _mockCustomerRepository.Setup(repo => repo.GetCustomerByCity(city)).ReturnsAsync(mockCustomers);

            // Act
            var result = await _controller.GetCustomerByCity(city);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsType<List<CustomerwithAddressDTO>>(okResult.Value);
            Assert.Equal(mockCustomers, returnedCustomers);
        }





        [Fact]
        public async Task GetCustomerByCity_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var city = "Kimberley";
            var errorMessage = "An error occurred while fetching customers by city.";

            _mockCustomerRepository.Setup(repo => repo.GetCustomerByCity(city))
                                   .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.GetCustomerByCity(city);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }



        #endregion


        #region GetCustomerByCountry

        [Fact]
        public async Task GetCustomerByCountry_ReturnsOk_WhenCustomersFound()
        {
            // Arrange
            var country = "AUSTRALIA";
            var mockCustomers = new List<CustomerwithAddressDTO>
    {
        new CustomerwithAddressDTO
        {
            CustomerId = 1600,
            StoreId = 2,
            FirstName = "Tomas",
            LastName = "TOMAR",
            Email = "string",
            AddressId = 2,
            Active = "1",
            CreateDate = new DateTime(2024, 12, 24, 10, 29, 16, 250),
            LastUpdate = new DateTime(2024, 12, 27, 18, 16, 9, 233),
            Address1 = "string",
            Address2 = "string",
            District = "string",
            CityId = 576,
            PostalCode = "string",
            Phone = "99839924930"
        },
        new CustomerwithAddressDTO
        {
            CustomerId = 1601,
            StoreId = 2,
            FirstName = "JOHN",
            LastName = "CARTER",
            Email = "JOHN.CARTER@sakilacustomer.org",
            AddressId = 2,
            Active = "1",
            CreateDate = new DateTime(2006, 2, 14, 22, 4, 36),
            LastUpdate = new DateTime(2024, 12, 27, 18, 16, 9, 233),
            Address1 = "string",
            Address2 = "string",
            District = "string",
            CityId = 576,
            PostalCode = "string",
            Phone = "99839924930"
        }
    };

            _mockCustomerRepository.Setup(repo => repo.GetCustomerByCountry(country)).ReturnsAsync(mockCustomers);

            // Act
            var result = await _controller.GetCustomerByCountry(country);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsType<List<CustomerwithAddressDTO>>(okResult.Value);
            Assert.Equal(mockCustomers, returnedCustomers);
        }




        [Fact]
        public async Task GetCustomerByCountry_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var country = "AUSTRALIA";
            var errorMessage = "An error occurred while fetching customers by country.";

            _mockCustomerRepository.Setup(repo => repo.GetCustomerByCountry(country))
                                   .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.GetCustomerByCountry(country);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }

        #endregion



        #region GetActiveCustomers


        [Fact]
        public async Task GetActiveCustomers_ReturnsOk_WhenCustomersFound()
        {
            // Arrange
            var mockActiveCustomers = new List<CustomerDTO>
    {
        new CustomerDTO
        {
            CustomerId = 1,
            StoreId = 2,
            FirstName = "MARIENE",
            LastName = "SMITHS",
            Email = "MARIENE.SMITHS@sakilacustomer.org",
            AddressId = 5,
            Active = "1",
            CreateDate = new DateTime(2006, 2, 14, 22, 4, 36),
            LastUpdate = new DateTime(2006, 2, 15, 4, 57, 20)
        },
        new CustomerDTO
        {
            CustomerId = 2,
            StoreId = 2,
            FirstName = "PATRISIA",
            LastName = "JACKSON",
            Email = "PATRISIA.JACKSON@sakilacustomer.org",
            AddressId = 6,
            Active = "1",
            CreateDate = new DateTime(2006, 2, 14, 22, 4, 36),
            LastUpdate = new DateTime(2006, 2, 15, 4, 57, 20)
        }
    };

            _mockCustomerRepository.Setup(repo => repo.GetActiveCustomers())
                                   .ReturnsAsync(mockActiveCustomers);

            // Act
            var result = await _controller.GetActiveCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsType<List<CustomerDTO>>(okResult.Value);
            Assert.Equal(mockActiveCustomers, returnedCustomers);
        }



        [Fact]
        public async Task GetActiveCustomers_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var errorMessage = "An error occurred while fetching active customers.";
            _mockCustomerRepository.Setup(repo => repo.GetActiveCustomers())
                                   .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.GetActiveCustomers();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }

        #endregion


        #region GetInActiveCustomers

        [Fact]
        public async Task GetInActiveCustomers_ReturnsOk_WhenCustomersFound()
        {
            // Arrange
            var mockInActiveCustomers = new List<CustomerDTO>
    {
        new CustomerDTO
        {
            CustomerId = 16,
            StoreId = 2,
            FirstName = "SANDRA",
            LastName = "MARTIN",
            Email = "SANDRA.MARTIN@sakilacustomer.org",
            AddressId = 20,
            Active = "0",
            CreateDate = new DateTime(2006, 2, 14, 22, 4, 36),
            LastUpdate = new DateTime(2006, 2, 15, 4, 57, 20)
        },
        new CustomerDTO
        {
            CustomerId = 64,
            StoreId = 2,
            FirstName = "JUDITH",
            LastName = "COX",
            Email = "JUDITH.COX@sakilacustomer.org",
            AddressId = 68,
            Active = "0",
            CreateDate = new DateTime(2006, 2, 14, 22, 4, 36),
            LastUpdate = new DateTime(2006, 2, 15, 4, 57, 20)
        }
    };

            _mockCustomerRepository.Setup(repo => repo.GetInActiveCustomers())
                                   .ReturnsAsync(mockInActiveCustomers);

            // Act
            var result = await _controller.GetInActiveCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsType<List<CustomerDTO>>(okResult.Value);
            Assert.Equal(mockInActiveCustomers, returnedCustomers);
        }




        [Fact]
        public async Task GetInActiveCustomers_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var errorMessage = "An error occurred while fetching inactive customers.";
            _mockCustomerRepository.Setup(repo => repo.GetInActiveCustomers())
                                   .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.GetInActiveCustomers();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }


        #endregion



        #region GetCustomerByPhone

        [Fact]
        public async Task GetCustomerByPhone_ReturnsOk_WhenCustomersFound()
        {
            // Arrange
            var phone = "9243137345";
            var mockCustomers = new List<CustomerwithAddressDTO>
    {
        new CustomerwithAddressDTO
        {
            CustomerId = 2,
            StoreId = 2,
            FirstName = "PATRISIA",
            LastName = "JACKSON",
            Email = "PATRISIA.JACKSON@sakilacustomer.org",
            AddressId = 6,
            Active = "1",
            CreateDate = new DateTime(2006, 2, 14, 22, 4, 36),
            LastUpdate = new DateTime(2006, 2, 15, 4, 57, 20),
            Address1 = "1121 Loja Avenue",
            Address2 = null,
            District = " ",
            CityId = 449,
            PostalCode = "17886",
            Phone = "9243137345"
        }
    };

            _mockCustomerRepository.Setup(repo => repo.GetCustomerByPhone(phone))
                                   .ReturnsAsync(mockCustomers);

            // Act
            var result = await _controller.GetCustomerByPhone(phone);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsType<List<CustomerwithAddressDTO>>(okResult.Value);
            Assert.Equal(mockCustomers, returnedCustomers);
        }



        [Fact]
        public async Task GetCustomerByPhone_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var phone = "9243137345";
            var errorMessage = "An error occurred while fetching customers by phone.";
            _mockCustomerRepository.Setup(repo => repo.GetCustomerByPhone(phone))
                                   .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.GetCustomerByPhone(phone);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }

        #endregion




        #region UpdateFirstNameById

        [Fact]
        public async Task UpdateFirstNameById_ReturnsOk_WhenCustomerUpdated()
        {
            // Arrange
            var id = 1;
            var newName = "MARRIENE";
            var mockUpdatedCustomers = new List<CustomerDTO>
    {
        new CustomerDTO
        {
            CustomerId = 1,
            StoreId = 2,
            FirstName = "MARRIENE",
            LastName = "SMITHS",
            Email = "MARIENE.SMITHS@sakilacustomer.org",
            AddressId = 5,
            Active = "1",
            CreateDate = new DateTime(2006, 2, 14, 22, 4, 36),
            LastUpdate = new DateTime(2006, 2, 15, 4, 57, 20)
        }
    };

            // Setup mock to return the list of updated customers
            _mockCustomerRepository.Setup(repo => repo.UpdateFirstNameById(id, newName))
                                   .ReturnsAsync(mockUpdatedCustomers);

            // Act
            var result = await _controller.UpdateFirstNameById(id, newName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsType<List<CustomerDTO>>(okResult.Value);
            Assert.Equal(mockUpdatedCustomers.Count, returnedCustomers.Count);
            Assert.Equal(mockUpdatedCustomers[0].FirstName, returnedCustomers[0].FirstName);
            Assert.Equal(mockUpdatedCustomers[0].LastName, returnedCustomers[0].LastName);
        }


        [Fact]
        public async Task UpdateFirstNameById_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var id = 1;
            var newName = "MARRIENE";
            var errorMessage = "An error occurred while updating the first name.";
            _mockCustomerRepository.Setup(repo => repo.UpdateFirstNameById(id, newName))
                                   .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.UpdateFirstNameById(id, newName);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }

        #endregion



        #region UpdateLastNameById

        [Fact]
        public async Task UpdateLastNameById_ReturnsOk_WhenCustomerUpdated()
        {
            // Arrange
            var id = 1;
            var newLastName = "SMITHS";
            var mockUpdatedCustomers = new List<CustomerDTO>
    {
        new CustomerDTO
        {
            CustomerId = 1,
            StoreId = 2,
            FirstName = "MARIENE",
            LastName = "SMITHS",
            Email = "MARIENE.SMITHS@sakilacustomer.org",
            AddressId = 5,
            Active = "1",
            CreateDate = new DateTime(2006, 2, 14, 22, 4, 36),
            LastUpdate = new DateTime(2006, 2, 15, 4, 57, 20)
        }
    };

            // Setup mock to return the list of updated customers
            _mockCustomerRepository.Setup(repo => repo.UpdateLastNameById(id, newLastName))
                                   .ReturnsAsync(mockUpdatedCustomers);

            // Act
            var result = await _controller.UpdateLastNameById(id, newLastName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsType<List<CustomerDTO>>(okResult.Value);
            Assert.Equal(mockUpdatedCustomers.Count, returnedCustomers.Count);
            Assert.Equal(mockUpdatedCustomers[0].LastName, returnedCustomers[0].LastName);
        }



        [Fact]
        public async Task UpdateLastNameById_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var id = 1;
            var newLastName = "SMITHS";
            var errorMessage = "An error occurred while updating the last name.";

            // Setup mock to throw an exception when UpdateLastNameById is called
            _mockCustomerRepository.Setup(repo => repo.UpdateLastNameById(id, newLastName))
                                   .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.UpdateLastNameById(id, newLastName);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }


        #endregion



        #region UpdateEmailCustomer

        [Fact]
        public async Task UpdateEmailCustomer_ReturnsOk_WhenEmailUpdated()
        {
            // Arrange
            var id = 1;
            var newEmail = "newemail@example.com";

            // Mock the updated customer object
            var mockUpdatedCustomer = new CustomerDTO
            {
                CustomerId = id,
                StoreId = 2,
                FirstName = "MARIENE",
                LastName = "SMITHS",
                Email = newEmail,  // Updated email
                AddressId = 5,
                Active = "1",
                CreateDate = new DateTime(2006, 2, 14),
                LastUpdate = new DateTime(2006, 2, 15)
            };

            // Setup the mock to return the updated customer
            _mockCustomerRepository.Setup(repo => repo.UpdateEmailCustomer(id, newEmail))
                                   .ReturnsAsync(new List<CustomerDTO> { mockUpdatedCustomer });

            // Act
            var result = await _controller.UpdateEmailCustomer(id, newEmail);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsType<List<CustomerDTO>>(okResult.Value);
            Assert.Single(returnedCustomers);  // Only one customer should be returned
            Assert.Equal(mockUpdatedCustomer.Email, returnedCustomers[0].Email);  // Verify the email was updated
        }



        [Fact]
        public async Task UpdateEmailCustomer_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var id = 1;
            var newEmail = "newemail@example.com";
            var errorMessage = "An error occurred while updating the email.";

            // Setup mock to throw an exception when UpdateEmailCustomer is called
            _mockCustomerRepository.Setup(repo => repo.UpdateEmailCustomer(id, newEmail))
                                   .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.UpdateEmailCustomer(id, newEmail);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);  // Check if the error message is returned correctly
        }

        #endregion


        #region AssignStoreToCustomer

        [Fact]
        public async Task AssignStoreToCustomer_ReturnsOk_WhenStoreAssignedSuccessfully()
        {
            // Arrange
            var customerId = 1;
            var storeId = 1;

            // Mock the updated customer object with the assigned store
            var mockUpdatedCustomer = new CustomerDTO
            {
                CustomerId = customerId,
                StoreId = storeId,
                FirstName = "MARIENE",
                LastName = "SMITHS",
                Email = "MARIENE.SMITHS@sakilacustomer.org",
                AddressId = 5,
                Active = "1",
                CreateDate = new DateTime(2006, 2, 14),
                LastUpdate = new DateTime(2006, 2, 15)
            };

            // Setup the mock to return the updated customer
            _mockCustomerRepository.Setup(repo => repo.AssignStoreToCustomer(customerId, storeId))
                                   .ReturnsAsync(new List<CustomerDTO> { mockUpdatedCustomer });

            // Act
            var result = await _controller.AssignStoreToCustomer(customerId, storeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsType<List<CustomerDTO>>(okResult.Value);
            Assert.Single(returnedCustomers);  // Only one customer should be returned
            Assert.Equal(mockUpdatedCustomer.StoreId, returnedCustomers[0].StoreId);  // Verify the store was assigned
        }



        [Fact]
        public async Task AssignStoreToCustomer_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var customerId = 1;
            var storeId = 1;
            var errorMessage = "An error occurred while assigning store to customer.";

            // Setup mock to throw an exception when AssignStoreToCustomer is called
            _mockCustomerRepository.Setup(repo => repo.AssignStoreToCustomer(customerId, storeId))
                                   .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.AssignStoreToCustomer(customerId, storeId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);  // Check if the error message is returned correctly
        }

        #endregion



        #region UpdatePhoneCustomer

        [Fact]
        
        public async Task UpdatePhoneCustomer_ReturnsOk_WhenPhoneUpdatedSuccessfully()
        {
            // Arrange
            var id = 4;  // Example customer ID
            var phone = "123-446-7000";  // Example new phone number
            var mockUpdatedCustomer = new CustomerDTO
            {
                CustomerId = id,
                StoreId = 2,
                FirstName = "BARBARA",
                LastName = "JONES",
                Email = "BARBARA.JONES@sakilacustomer.org",
                AddressId = 8,
                Active = "1",
                CreateDate = new DateTime(2006, 2, 14),
                LastUpdate = new DateTime(2006, 2, 15)
            };

            // Set up mock behavior for the repository
            _mockCustomerRepository.Setup(repo => repo.UpdatePhoneCustomer(id, phone))
                .ReturnsAsync(new List<CustomerDTO> { mockUpdatedCustomer });

            // Act
            var result = await _controller.UpdatePhoneCustomer(id, phone);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);  // Ensure the result is an OkObjectResult
            var returnValue = Assert.IsType<List<CustomerDTO>>(okResult.Value);  // Ensure the returned value is a List<CustomerDTO>

            // Check if the returned customer details are correct
            var customerData = returnValue;
            Assert.NotNull(customerData);
            Assert.Single(customerData);  // Ensure only one customer is returned
            Assert.Equal(id, customerData[0].CustomerId);  // Verify that the customer ID is correct
            Assert.Equal("BARBARA", customerData[0].FirstName);  // Verify the first name
            Assert.Equal("JONES", customerData[0].LastName);  // Verify the last name
            Assert.Equal("BARBARA.JONES@sakilacustomer.org", customerData[0].Email);  // Verify the email
            Assert.Equal("1", customerData[0].Active);  // Verify if the customer is active
        }



        [Fact]
        public async Task UpdatePhoneCustomer_ReturnsBadRequest_WhenCustomerNotFound()
        {
            // Arrange
            var id = 9999;  // Non-existent customer ID
            var phone = "123-446-7000";  // Example new phone number

            // Set up mock behavior to simulate the customer not being found
            _mockCustomerRepository.Setup(repo => repo.UpdatePhoneCustomer(id, phone))
                .ThrowsAsync(new KeyNotFoundException($"Customer with ID {id} not found."));

            // Act
            var result = await _controller.UpdatePhoneCustomer(id, phone);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);  // Ensure the result is a BadRequest
            var returnValue = badRequestResult.Value as string;  // Check the returned error message
            Assert.NotNull(returnValue);  // Ensure the error message is not null
            Assert.Equal("Customer with ID 9999 not found.", returnValue);  // Check if the error message matches the expected message
        }



        #endregion



        #region AssignAddress

        [Fact]

       
        public async Task AssignAddress_ReturnsOk_WhenAddressAssignedSuccessfully()
        {
            // Arrange
            var id = 1;  // Example customer ID
            var addressId = 5;  // Example address ID to be assigned to the customer
            var mockAssignedCustomer = new CustomerDTO
            {
                CustomerId = id,
                StoreId = 1,
                FirstName = "MARRIENE",
                LastName = "SMITHS",
                Email = "MARIENE.SMITHS@sakilacustomer.org",
                AddressId = addressId,
                Active = "1",
                CreateDate = new DateTime(2006, 2, 14),
                LastUpdate = new DateTime(2006, 2, 15)
            };

            // Set up the mock behavior
            _mockCustomerRepository.Setup(repo => repo.AssignAddress(id, addressId))
                .ReturnsAsync(new List<CustomerDTO> { mockAssignedCustomer });

            // Act
            var result = await _controller.AssignAddress(id, addressId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);  // Ensure the result is Ok
            var returnValue = Assert.IsType<List<CustomerDTO>>(okResult.Value);  // Ensure the result is of type List<CustomerDTO>

            // Ensure customer data is returned
            Assert.NotNull(returnValue);
            Assert.Single(returnValue);  // Ensure only one customer is returned
            Assert.Equal(id, returnValue[0].CustomerId);  // Verify customer ID
            Assert.Equal(addressId, returnValue[0].AddressId);  // Verify address ID
        }


        [Fact]
        public async Task AssignAddress_ReturnsBadRequest_WhenIdOrAddressIdIsZero()
        {
            // Arrange
            var invalidId = 0;  // Invalid customer ID
            var validAddressId = 5;  // Valid address ID

            // Act
            var result = await _controller.AssignAddress(invalidId, validAddressId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);  // Ensure the result is BadRequest
            Assert.Equal("Customerid or store is Required", badRequestResult.Value);  // Ensure the error message matches
        }

        #endregion



    }
}
