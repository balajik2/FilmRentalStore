using FilmRentalStore.Controllers;
using FilmRentalStore.DTO;
using FilmRentalStore.Services;
using FluentValidation;
using Moq;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;



namespace FilmRentalStoreTesting
{
    public class ActorControllerTest
    {

        private readonly Mock<IActorRepository> _mockActorRepository;

        private readonly Mock<IValidator<ActorDTO>> _mockValidator;
        private readonly ActorController _controller;

        public ActorControllerTest()
        {
            _mockActorRepository = new Mock<IActorRepository>();

            _mockValidator = new Mock<IValidator<ActorDTO>>();
            _controller = new ActorController(_mockActorRepository.Object, _mockValidator.Object);
        }
        //Adding Actor
        [Fact]
        public async Task AddActor_ValidActor_ReturnsOkResult()
        {
            // Arrange
            var actorDTO = new ActorDTO { FirstName = "John", LastName = "Doe" };
            _mockActorRepository.Setup(r => r.AddActor(actorDTO)).ReturnsAsync(true);

            // Act
            var result = await _controller.AddActor(actorDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Record created successfully", okResult.Value);
        }



        [Fact]
        public async Task AddActor_InvalidActor_ReturnsBadRequestResult()
        {
            // Arrange
            var actorDTO = new ActorDTO { FirstName = "", LastName = "" };  // Invalid data (empty strings)

            // Act
            var result = await _controller.AddActor(actorDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);  // Expect BadRequest
            var response = JObject.FromObject(badRequestResult.Value);  // Convert to JObject to access properties dynamically

            // Assert the message
            Assert.Equal("Validation failed: FirstName and LastName are required.", response["message"].ToString());
        }



        //GetActorsByLastName
        [Fact]
        public async Task GetActorsByLastName_ValidLastName_ReturnsOkResult()
        {
            // Arrange
            var lastName = "Doe";
            var actors = new List<ActorDTO> { new ActorDTO { LastName = lastName } };
            _mockActorRepository.Setup(r => r.GetActorsByLastName(lastName)).ReturnsAsync(actors);

            // Act
            var result = await _controller.GetActorsByLastName(lastName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedActors = Assert.IsType<List<ActorDTO>>(okResult.Value);
            Assert.Equal(actors, returnedActors);
        }
        [Fact]
        public async Task GetActorsByLastName_InvalidLastName_ReturnsNotFoundResult()
        {
            // Arrange
            var lastName = "Unknown";
            _mockActorRepository.Setup(r => r.GetActorsByLastName(lastName)).ReturnsAsync(new List<ActorDTO>());

            // Act
            var result = await _controller.GetActorsByLastName(lastName);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No actors found with the given last name.", notFoundResult.Value);
        }

        //GetActorsByFirstName
        [Fact]
        public async Task GetActorsByFirstName_ValidFirstName_ReturnsOkResult()
        {
            // Arrange
            var firstName = "John";
            var actors = new List<ActorDTO> { new ActorDTO { ActorId = 1, FirstName = firstName, LastName = "Doe" } };

            _mockActorRepository.Setup(r => r.GetActorsByFirstName(firstName)).ReturnsAsync(actors);

            // Act
            var result = await _controller.GetActorsByFirstName(firstName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedActors = Assert.IsType<List<ActorDTO>>(okResult.Value);
            Assert.Equal(actors, returnedActors);
        }
        [Fact]
        public async Task GetActorsByFirstName_InvalidFirstName_ReturnsNotFoundResult()
        {
            // Arrange
            var firstName = "UnknownName";
            _mockActorRepository.Setup(r => r.GetActorsByFirstName(firstName)).ReturnsAsync(new List<ActorDTO>());

            // Act
            var result = await _controller.GetActorsByFirstName(firstName);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No actors found with the given first name.", notFoundResult.Value);
        }

        //UpdateLastNameById
        [Fact]
        public async Task UpdateLastNameById_ValidIdAndName_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var name = "Doe";
            var actorDTOs = new List<ActorDTO>
    {
        new ActorDTO { ActorId = id, FirstName = "John", LastName = name }
    };

            _mockActorRepository.Setup(r => r.UpdateLastNameById(id, name)).ReturnsAsync(actorDTOs);

            // Act
            var result = await _controller.UpdateLastNameById(id, name);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedActors = Assert.IsType<List<ActorDTO>>(okResult.Value);
            Assert.Equal(actorDTOs, returnedActors);
        }
        [Fact]
        public async Task UpdateLastNameById_InvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            var id = 1;
            var name = "Doe";

            // Mock the repository to throw the KeyNotFoundException
            _mockActorRepository.Setup(r => r.UpdateLastNameById(id, name))
                .ThrowsAsync(new KeyNotFoundException($"Actor with ID {id} not found"));

            // Act
            var result = await _controller.UpdateLastNameById(id, name);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Actor with ID {id} not found", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        }


        //UpdateFirstNameById
        [Fact]
        public async Task UpdateFirstNameById_ValidIdAndName_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var name = "John";
            var actorDTOs = new List<ActorDTO>
    {
        new ActorDTO { ActorId = id, FirstName = name, LastName = "Doe" }
    };

            _mockActorRepository.Setup(r => r.UpdateFirstNameById(id, name)).ReturnsAsync(actorDTOs);

            // Act
            var result = await _controller.UpdateFirstNameById(id, name);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedActors = Assert.IsType<List<ActorDTO>>(okResult.Value);
            Assert.Equal(actorDTOs, returnedActors);
        }
        [Fact]
        public async Task UpdateFirstNameById_InvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            var id = 1;
            var name = "John";

            // Mocking the repository to throw KeyNotFoundException
            _mockActorRepository.Setup(r => r.UpdateFirstNameById(id, name))
                .ThrowsAsync(new KeyNotFoundException($"Actor with ID {id} not found"));

            // Act
            var result = await _controller.UpdateFirstNameById(id, name);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Actor with ID {id} not found", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        }



        //GetFilmsByActorId
        [Fact]
        public async Task GetFilmsByActorId_ValidId_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var films = new List<FilmDTO>
    {
        new FilmDTO { FilmId = 101, Title = "Example Film 1" },
        new FilmDTO { FilmId = 102, Title = "Example Film 2" }
    };

            _mockActorRepository.Setup(r => r.GetFilmsByActorId(id)).ReturnsAsync(films);

            // Act
            var result = await _controller.GetFilmsByActorId(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(films, returnedFilms);
        }

        [Fact]
        public async Task GetFilmsByActorId_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var id = 1;

            _mockActorRepository.Setup(r => r.GetFilmsByActorId(id)).ReturnsAsync(new List<FilmDTO>());

            // Act
            var result = await _controller.GetFilmsByActorId(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No films found for the actor.", notFoundResult.Value);
        }

        //AssignFilmToActor
        [Fact]
        public async Task AssignFilmToActor_ValidIdAndFilmId_ReturnsOkResult()
        {
            // Arrange
            var actorId = 1;
            var filmId = 1;
            var filmDTOs = new List<FilmDTO>
    {
        new FilmDTO { FilmId = filmId, Title = "Example Film" }
    };

            _mockActorRepository.Setup(r => r.AssignFilmToActor(actorId, filmId)).ReturnsAsync(filmDTOs);

            // Act
            var result = await _controller.AssignFilmToActor(actorId, filmId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(filmDTOs, returnedFilms);
        }

        [Fact]
        public async Task AssignFilmToActor_InvalidActorId_ReturnsNotFoundResult()
        {
            // Arrange
            var actorId = 1;
            var filmId = 1;

            // Mocking the repository to return null or empty list
            _mockActorRepository.Setup(r => r.AssignFilmToActor(actorId, filmId)).ReturnsAsync((List<FilmDTO>)null);

            // Act
            var result = await _controller.AssignFilmToActor(actorId, filmId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            // Ensure the message returned by the NotFound response is as expected
            Assert.Equal("Failed to assign actor to the film.", notFoundResult.Value);

            // Ensure the status code is 404
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        //GetTopTenActorsByFilmCount
        [Fact]
        public async Task GetTopTenActorsByFilmCount_ValidData_ReturnsOkResult()
        {
            // Arrange
            var actors = new List<Top10ActorByFilmDTO>
    {
        new Top10ActorByFilmDTO { ActorId = 1, FirstName = "John", LastName = "Doe", NoOfFilm = 10 }
    };
            _mockActorRepository.Setup(r => r.GetTopTenActorsByFilmCount()).ReturnsAsync(actors);

            // Act
            var result = await _controller.GetTopTenActorsByFilmCount();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedActors = Assert.IsType<List<Top10ActorByFilmDTO>>(okResult.Value);
            Assert.Equal(actors, returnedActors);
        }

        [Fact]
        public async Task GetTopTenActorsByFilmCount_NoData_ReturnsNotFoundResult()
        {
            // Arrange
            _mockActorRepository.Setup(r => r.GetTopTenActorsByFilmCount()).ReturnsAsync(new List<Top10ActorByFilmDTO>());

            // Act
            var result = await _controller.GetTopTenActorsByFilmCount();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No actors found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetTopTenActorsByFilmCount_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            _mockActorRepository.Setup(r => r.GetTopTenActorsByFilmCount()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetTopTenActorsByFilmCount();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Database error", badRequestResult.Value);
        }

    }
}
