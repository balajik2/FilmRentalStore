using FilmRentalStore.Controllers;
using FilmRentalStore.Services;
using FilmRentalStore.Validators;
using Moq;
using FilmRentalStore.DTO;
using Microsoft.AspNetCore.Mvc;
using FilmRentalStore.Models;
namespace FilmRentalStoreTesting
{
    public class FilmTesting
    {
        private readonly Mock<IFilmRepository> _filmRepoMock;
        private readonly Mock<CustomFilmValidator> _mockCustomValidator;
        private readonly FilmController _controller;
        public FilmTesting()
        {
            _filmRepoMock = new Mock<IFilmRepository>();
            _mockCustomValidator = new Mock<CustomFilmValidator>();
            _controller = new FilmController(_filmRepoMock.Object, _mockCustomValidator.Object);

        }

        #region AddFilm
        [Fact]
        public async Task AddFilm_ReturnsOk_WhenValidationSucceeds()
        {
            // Arrange
            var film = new FilmDTO
            {
                Title = "Inception",
                Description = "A mind-bending thriller.",
                ReleaseYear = "2010",
                LanguageId = 1,
                OriginalLanguageId = null,
                RentalDuration = 7,
                RentalRate = 3.99m,
                Length = 148,
                ReplacementCost = 19.99m,
                Rating = "PG-13",
                LastUpdate = DateTime.UtcNow
            };

            _mockCustomValidator.Setup(v => v.Validate(It.IsAny<FilmDTO>())).Returns(new List<string>());

            _filmRepoMock.Setup(r => r.AddFilm(It.IsAny<FilmDTO>())).Returns(Task.CompletedTask);


            // Act
            var result = await _controller.AddFilm(film);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Record Created Successfully", okResult.Value);
        }


        [Fact]
        public async Task AddFilm_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var film = new FilmDTO
            {
                Title = "Inception",
                Description = "A mind-bending thriller.",
                ReleaseYear = "2010",
                LanguageId = 1,
                OriginalLanguageId = null,
                RentalDuration = 7,
                RentalRate = 3.99m,
                Length = 148,
                ReplacementCost = 19.99m,
                Rating = "PG-13",
                LastUpdate = DateTime.UtcNow
            };


            var validationErrors = new List<string> { "Title is required!" };
            _mockCustomValidator.Setup(v => v.Validate(It.IsAny<FilmDTO>())).Returns(validationErrors);


            _filmRepoMock.Setup(r => r.AddFilm(It.IsAny<FilmDTO>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddFilm(film);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<List<string>>(badRequestResult.Value);
            Assert.Contains("Title is required!", errors);
        }
        #endregion


        #region SearchFilmsByTitle

        [Fact]
        public async Task SearchFilmsByTitle_ReturnsOk_WhenFilmsExist()
        {
            // Arrange
            var title = "Inception";
            var mockFilms = new List<FilmDTO>
            {
              new FilmDTO { Title = "Inception", Description = "A mind-bending thriller", ReleaseYear = "2010", LanguageId = 1 },
              new FilmDTO { Title = "Inception 2", Description = "A sequel to the mind-bending thriller", ReleaseYear = "2016", LanguageId = 2 }
             };


            _filmRepoMock.Setup(repo => repo.SearchFilmsByTitle(title)).ReturnsAsync(mockFilms);

            // Act
            var result = await _controller.SearchFilmsByTitle(title);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(mockFilms, returnedFilms);
        }

        [Fact]
        public async Task SearchFilmsByTitle_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var title = "Inception";
            var errorMessage = "An error occurred while fetching the films";


            _filmRepoMock.Setup(repo => repo.SearchFilmsByTitle(title)).ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.SearchFilmsByTitle(title);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }

        #endregion


        #region SearchFilmsByReleaseYear
        [Fact]
        public async Task SearchFilmsByReleaseYear_ReturnsOk_WhenFilmsExist()
        {
            // Arrange
            var releaseYear = "2010";
            var mockFilms = new List<FilmDTO>
            {
               new FilmDTO { Title = "Inception", ReleaseYear = "2010", LanguageId = 1 },
               new FilmDTO { Title = "Toy Story 3", ReleaseYear = "2010", LanguageId = 2 }
            };


            _filmRepoMock.Setup(repo => repo.SearchFilmsByReleaseYear(releaseYear)).ReturnsAsync(mockFilms);

            // Act
            var result = await _controller.SearchFilmsByReleaseYear(releaseYear);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(mockFilms, returnedFilms);
        }

        [Fact]
        public async Task SearchFilmsByReleaseYear_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var releaseYear = "2010";
            var errorMessage = "An error occurred while fetching the films";

            _filmRepoMock.Setup(repo => repo.SearchFilmsByReleaseYear(releaseYear)).ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.SearchFilmsByReleaseYear(releaseYear);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }
        #endregion



        #region SearchFilmsByRentalDurationGreater

        [Fact]
        public async Task SearchFilmsByRentalDurationGreater_ReturnsOk_WhenFilmsFound()
        {
            // Arrange
            var rentalDuration = (byte)7;
            var mockFilms = new List<FilmDTO>
            {
               new FilmDTO { Title = "Film 1", RentalDuration = 8 },
               new FilmDTO { Title = "Film 2", RentalDuration = 10 }
            };

            _filmRepoMock.Setup(repo => repo.SearchFilmsByRentalDurationGreater(rentalDuration)).ReturnsAsync(mockFilms);

            // Act
            var result = await _controller.SearchFilmsByRentalDurationGreater(rentalDuration);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(mockFilms, returnedFilms);
        }

        [Fact]
        public async Task SearchFilmsByRentalDurationGreater_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var rentalDuration = (byte)7;
            var errorMessage = "An error occurred while fetching the films";

            _filmRepoMock.Setup(repo => repo.SearchFilmsByRentalDurationGreater(rentalDuration))
                         .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.SearchFilmsByRentalDurationGreater(rentalDuration);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }

        #endregion


        #region SearchFilmsByRentalRateGreater

        [Fact]
        public async Task SearchFilmsByRentalRateGreater_ReturnsOk_WhenFilmsFound()
        {
            // Arrange
            var rentalRate = 5.00m;
            var mockFilms = new List<FilmDTO>
            {
              new FilmDTO { Title = "Film 1", RentalRate = 6.00m },
              new FilmDTO { Title = "Film 2", RentalRate = 7.00m }
            };

            _filmRepoMock.Setup(repo => repo.SearchFilmsByRentalRateGreater(rentalRate)).ReturnsAsync(mockFilms);

            // Act
            var result = await _controller.SearchFilmsByRentalRateGreater(rentalRate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(mockFilms, returnedFilms);
        }

        [Fact]
        public async Task SearchFilmsByRentalRateGreater_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var rentalRate = 5.00m;
            var errorMessage = "An error occurred while fetching the films";


            _filmRepoMock.Setup(repo => repo.SearchFilmsByRentalRateGreater(rentalRate))
                         .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.SearchFilmsByRentalRateGreater(rentalRate);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }
        #endregion


        #region SearchFilmsByLengthGreater
        [Fact]
        public async Task SearchFilmsByLengthGreater_ReturnsOk_WhenFilmsFound()
        {
            // Arrange
            var length = (short)100; // Example length value
            var mockFilms = new List<FilmDTO>
            {
               new FilmDTO { Title = "Film 1", Length = 120 },
               new FilmDTO { Title = "Film 2", Length = 150 }
            };

            _filmRepoMock.Setup(repo => repo.SearchFilmsByLengthGreater(length)).ReturnsAsync(mockFilms);

            // Act
            var result = await _controller.SearchFilmsByLengthGreater(length);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(mockFilms, returnedFilms);
        }

        [Fact]
        public async Task SearchFilmsByLengthGreater_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var length = (short)100; // Example length value
            var errorMessage = "An error occurred while fetching the films";

            _filmRepoMock.Setup(repo => repo.SearchFilmsByLengthGreater(length))
                         .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.SearchFilmsByLengthGreater(length);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }
        #endregion



        #region SearchFilmsByRentalDurationLower

        [Fact]
        public async Task SearchFilmsByRentalDurationLower_ReturnsOk_WhenFilmsFound()
        {
            // Arrange
            var rentalDuration = (byte)7;
            var mockFilms = new List<FilmDTO>
            {
               new FilmDTO { Title = "Film 1", RentalDuration = 5 },
               new FilmDTO { Title = "Film 2", RentalDuration = 6 }
            };

            _filmRepoMock.Setup(repo => repo.SearchFilmsByRentalDurationLower(rentalDuration)).ReturnsAsync(mockFilms);

            // Act
            var result = await _controller.SearchFilmsByRentalDurationLower(rentalDuration);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(mockFilms, returnedFilms);
        }

        [Fact]
        public async Task SearchFilmsByRentalDurationLower_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var rentalDuration = (byte)7;
            var errorMessage = "An error occurred while fetching the films";

            _filmRepoMock.Setup(repo => repo.SearchFilmsByRentalDurationLower(rentalDuration))
                         .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.SearchFilmsByRentalDurationLower(rentalDuration);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }

        #endregion


        #region SearchFilmsByRentalRateLower
        [Fact]
        public async Task SearchFilmsByRentalRateLower_ReturnsOk_WhenFilmsFound()
        {
            // Arrange
            var rentalRate = 5.00m;
            var mockFilms = new List<FilmDTO>
            {
               new FilmDTO { Title = "Film 1", RentalRate = 3.99m },
               new FilmDTO { Title = "Film 2", RentalRate = 4.99m }
            };

            _filmRepoMock.Setup(repo => repo.SearchFilmsByRentalRateLower(rentalRate)).ReturnsAsync(mockFilms);

            // Act
            var result = await _controller.SearchFilmsByRentalRateLower(rentalRate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(mockFilms, returnedFilms);
        }

        [Fact]
        public async Task SearchFilmsByRentalRateLower_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var rentalRate = 5.00m;
            var errorMessage = "An error occurred while fetching the films";

            _filmRepoMock.Setup(repo => repo.SearchFilmsByRentalRateLower(rentalRate))
                         .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.SearchFilmsByRentalRateLower(rentalRate);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }
        #endregion



        #region SearchFilmsByLengthLower

        [Fact]
        public async Task SearchFilmsByLengthLower_ReturnsOk_WhenFilmsFound()
        {
            // Arrange
            var length = (short)100;
            var mockFilms = new List<FilmDTO>
            {
              new FilmDTO { Title = "Film 1", Length = 80 },
              new FilmDTO { Title = "Film 2", Length = 90 }
            };

            _filmRepoMock.Setup(repo => repo.SearchFilmsByLengthLower(length)).ReturnsAsync(mockFilms);

            // Act
            var result = await _controller.SearchFilmsByLengthLower(length);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(mockFilms, returnedFilms);
        }

        [Fact]
        public async Task SearchFilmsByLengthLower_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var length = (short)100;
            var errorMessage = "An error occurred while fetching the films";

            _filmRepoMock.Setup(repo => repo.SearchFilmsByLengthLower(length))
                         .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.SearchFilmsByLengthLower(length);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }

        #endregion

        #region SearchFilmsByReleaseYearRange
        [Fact]
        public async Task SearchFilmsByReleaseYearRange_ReturnsOk_WhenFilmsFound()
        {
            // Arrange
            var fromYear = 2000;
            var toYear = 2010;
            var mockFilms = new List<FilmDTO>
            {
              new FilmDTO { Title = "Film 1", ReleaseYear = "2005" },
              new FilmDTO { Title = "Film 2", ReleaseYear = "2008" }
            };

            _filmRepoMock.Setup(repo => repo.SearchFilmsByReleaseYearRange(fromYear, toYear)).ReturnsAsync(mockFilms);

            // Act
            var result = await _controller.SearchFilmsByReleaseYearRange(fromYear, toYear);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(mockFilms, returnedFilms);
        }

        [Fact]
        public async Task SearchFilmsByReleaseYearRange_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var fromYear = 2000;
            var toYear = 2010;
            var errorMessage = "An error occurred while fetching the films";

            _filmRepoMock.Setup(repo => repo.SearchFilmsByReleaseYearRange(fromYear, toYear))
                         .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.SearchFilmsByReleaseYearRange(fromYear, toYear);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }
        #endregion

        #region SearchFilmsByRatingLower
        [Fact]
        public async Task SearchFilmsByRatingLower_ReturnsOk_WhenFilmsFound()
        {
            // Arrange
            var rating = "PG";
            var mockFilms = new List<FilmDTO>
            {
              new FilmDTO { Title = "Film 1", Rating = "G" },
              new FilmDTO { Title = "Film 2", Rating = "PG" }
            };

            _filmRepoMock.Setup(repo => repo.SearchFilmsByRatingLower(rating)).ReturnsAsync(mockFilms);

            // Act
            var result = await _controller.SearchFilmsByRatingLower(rating);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(mockFilms, returnedFilms);
        }

        [Fact]
        public async Task SearchFilmsByRatingLower_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var rating = "PG";
            var errorMessage = "An error occurred while fetching the films";

            _filmRepoMock.Setup(repo => repo.SearchFilmsByRatingLower(rating))
                         .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.SearchFilmsByRatingLower(rating);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }
        #endregion


        #region SearchFilmsByRatingHigher

        [Fact]
        public async Task SearchFilmsByRatingHigher_ReturnsOk_WhenFilmsFound()
        {
            // Arrange
            var rating = "PG";
            var mockFilms = new List<FilmDTO>
            {
              new FilmDTO { Title = "Film 1", Rating = "G" },
              new FilmDTO { Title = "Film 2", Rating = "PG" }
            };

            _filmRepoMock.Setup(repo => repo.SearchFilmsByRatingHigher(rating)).ReturnsAsync(mockFilms);

            // Act
            var result = await _controller.SearchFilmsByRatingHigher(rating);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(mockFilms, returnedFilms);
        }

        [Fact]
        public async Task SearchFilmsByRatingHigher_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var rating = "PG";
            var errorMessage = "An error occurred while fetching the films";

            _filmRepoMock.Setup(repo => repo.SearchFilmsByRatingHigher(rating))
                         .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.SearchFilmsByRatingHigher(rating);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }

        #endregion

        #region SearchFilmsByLanguage
        [Fact]
        public async Task SearchFilmsByLanguage_ReturnsOk_WhenFilmsFound()
        {
            // Arrange
            var language = "English";
            var mockFilms = new List<FilmDTO>
            {
              new FilmDTO { Title = "Film 1", LanguageId = 1 },
              new FilmDTO { Title = "Film 2", LanguageId = 1 }
            };

            _filmRepoMock.Setup(repo => repo.SearchFilmsByLanguage(language)).ReturnsAsync(mockFilms);

            // Act
            var result = await _controller.SearchFilmsByLanguage(language);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(mockFilms, returnedFilms);
        }

        [Fact]
        public async Task SearchFilmsByLanguage_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var language = "English"; // Example language value
            var errorMessage = "An error occurred while fetching the films";

            _filmRepoMock.Setup(repo => repo.SearchFilmsByLanguage(language))
                         .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.SearchFilmsByLanguage(language);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }

        #endregion


        #region GetFilmCountByReleaseYear

        [Fact]
        public async Task GetFilmCountByReleaseYear_ReturnsOk_WhenFilmCountsFound()
        {
            // Arrange
            var mockFilmCount = new Dictionary<string, int>
            {
              { "2020", 10 },
              { "2021", 15 }
            };

            _filmRepoMock.Setup(repo => repo.GetFilmCountByReleaseYear()).ReturnsAsync(mockFilmCount);

            // Act
            var result = await _controller.GetFilmCountByReleaseYear();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilmCount = Assert.IsType<Dictionary<string, int>>(okResult.Value);
            Assert.Equal(mockFilmCount, returnedFilmCount);
        }

        [Fact]
        public async Task GetFilmCountByReleaseYear_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var errorMessage = "An error occurred while fetching the film counts";

            _filmRepoMock.Setup(repo => repo.GetFilmCountByReleaseYear())
                         .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.GetFilmCountByReleaseYear();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }

        #endregion



        #region GetActorsByFilmId
        [Fact]
        public async Task GetActorsByFilmId_ReturnsOk_WhenActorsFound()
        {
            // Arrange
            var filmId = 1;
            var mockActors = new List<ActorDTO>
            {
               new ActorDTO { ActorId = 1, FirstName = "Leonardo", LastName = "DiCaprio" },
               new ActorDTO { ActorId = 2, FirstName = "Joseph", LastName = "Gordon-Levitt" }
            };

            _filmRepoMock.Setup(repo => repo.GetActorsByFilmId(filmId)).ReturnsAsync(mockActors);

            // Act
            var result = await _controller.GetActorsByFilmId(filmId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedActors = Assert.IsType<List<ActorDTO>>(okResult.Value);
            Assert.Equal(mockActors, returnedActors);
        }

        [Fact]
        public async Task GetActorsByFilmId_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var filmId = 1;
            var errorMessage = "An error occurred while fetching the actors";

            _filmRepoMock.Setup(repo => repo.GetActorsByFilmId(filmId))
                         .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.GetActorsByFilmId(filmId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }
        #endregion


        #region GetFilmsByCategory
        [Fact]
        public async Task GetFilmsByCategory_ReturnsOk_WhenFilmsFound()
        {
            // Arrange
            var category = "Action";
            var mockFilms = new List<FilmDTO>
            {
               new FilmDTO { FilmId = 1, Title = "Film1", ReleaseYear = "2020", LanguageId = 1, RentalDuration = 7, RentalRate = 3.99m },
               new FilmDTO { FilmId = 2, Title = "Film2", ReleaseYear = "2021", LanguageId = 1, RentalDuration = 7, RentalRate = 4.99m }
            };


            _filmRepoMock.Setup(repo => repo.GetFilmsByCategory(category))
                .ReturnsAsync(mockFilms);

            // Act
            var result = await _controller.GetFilmsByCategory(category);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(mockFilms.Count, returnedFilms.Count);
        }

        [Fact]
        public async Task GetFilmsByCategory_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var category = "Action"; // Example category
            var errorMessage = "An error occurred while fetching films by category";


            _filmRepoMock.Setup(repo => repo.GetFilmsByCategory(category))
                                       .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.GetFilmsByCategory(category);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedErrorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorMessage, returnedErrorMessage);
        }
        #endregion


        #region AssignActorToFilm
        [Fact]
        public async Task AssignActorToFilm_ReturnsOk_WhenActorsAssignedSuccessfully()
        {
            // Arrange
            int filmId = 1;
            int actorId = 1;
            var mockActors = new List<ActorDTO>
            {
              new ActorDTO { ActorId = 1, FirstName = "John", LastName = "Doe" },
              new ActorDTO { ActorId = 2, FirstName = "Jane", LastName = "Smith" }
             };

            _filmRepoMock.Setup(repo => repo.AssignActorToFilm(filmId, actorId))
                         .ReturnsAsync(mockActors);

            // Act
            var result = await _controller.AssignActorToFilm(filmId, actorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedActors = Assert.IsType<List<ActorDTO>>(okResult.Value);
            Assert.Equal(mockActors.Count, returnedActors.Count);
        }

        [Fact]
        public async Task AssignActorToFilm_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            int filmId = 1;
            int actorId = 1;
            var exceptionMessage = "An error occurred while assigning actor to the film.";

            _filmRepoMock.Setup(repo => repo.AssignActorToFilm(filmId, actorId))
                         .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.AssignActorToFilm(filmId, actorId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }

        #endregion


        #region UpdateTitleOfFilm
        [Fact]
        public async Task UpdateTitleOfFilm_ReturnsOk_WhenTitleUpdatedSuccessfully()
        {
            int filmId = 1;
            string newTitle = "Updated Film Title";
            var updatedFilms = new List<FilmDTO>
            {
               new FilmDTO { FilmId = filmId, Title = newTitle }
            };

            _filmRepoMock.Setup(repo => repo.UpdateTitleOfFilm(filmId, newTitle))
                         .ReturnsAsync(updatedFilms);

            var result = await _controller.UpdateTitleOfFilm(filmId, newTitle);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(updatedFilms.Count, returnedFilms.Count);
            Assert.Equal(newTitle, returnedFilms[0].Title);
        }

        [Fact]
        public async Task UpdateTitleOfFilm_ReturnsBadRequest_WhenExceptionOccurs()
        {
            int filmId = 1;
            string newTitle = "Updated Film Title";
            var exceptionMessage = "An error occurred while updating the title.";

            _filmRepoMock.Setup(repo => repo.UpdateTitleOfFilm(filmId, newTitle))
                         .ThrowsAsync(new Exception(exceptionMessage));

            var result = await _controller.UpdateTitleOfFilm(filmId, newTitle);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }

        #endregion


        #region UpdateReleaseYearOfFilm
        [Fact]
        public async Task UpdateReleaseYearOfFilm_ReturnsOk_WhenReleaseYearUpdatedSuccessfully()
        {
            int filmId = 1;
            string newReleaseYear = "2024";
            var updatedFilms = new List<FilmDTO>
            {
               new FilmDTO { FilmId = filmId, ReleaseYear = newReleaseYear }
            };

            _filmRepoMock.Setup(repo => repo.UpdateReleaseYearOfFilm(filmId, newReleaseYear))
                         .ReturnsAsync(updatedFilms);

            var result = await _controller.UpdateReleaseYearOfFilm(filmId, newReleaseYear);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(updatedFilms.Count, returnedFilms.Count);
            Assert.Equal(newReleaseYear, returnedFilms[0].ReleaseYear);
        }

        [Fact]
        public async Task UpdateReleaseYearOfFilm_ReturnsBadRequest_WhenExceptionOccurs()
        {
            int filmId = 1;
            string newReleaseYear = "2024";
            var exceptionMessage = "An error occurred while updating the release year.";

            _filmRepoMock.Setup(repo => repo.UpdateReleaseYearOfFilm(filmId, newReleaseYear))
                         .ThrowsAsync(new Exception(exceptionMessage));

            var result = await _controller.UpdateReleaseYearOfFilm(filmId, newReleaseYear);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }

        #endregion



        #region UpdateRentalDurationOfFilm
        [Fact]
        public async Task UpdateRentalDurationOfFilm_ReturnsOk_WhenRentalDurationUpdatedSuccessfully()
        {
            int filmId = 1;
            byte newRentalDuration = 10;
            var updatedFilms = new List<FilmDTO>
            {
               new FilmDTO { FilmId = filmId, RentalDuration = newRentalDuration }
            };

            _filmRepoMock.Setup(repo => repo.UpdateRentalDurationOfFilm(filmId, newRentalDuration))
                         .ReturnsAsync(updatedFilms);

            var result = await _controller.UpdateRentalDurationOfFilm(filmId, newRentalDuration);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(updatedFilms.Count, returnedFilms.Count);
            Assert.Equal(newRentalDuration, returnedFilms[0].RentalDuration);
        }

        [Fact]
        public async Task UpdateRentalDurationOfFilm_ReturnsBadRequest_WhenExceptionOccurs()
        {
            int filmId = 1;
            byte newRentalDuration = 10;
            var exceptionMessage = "An error occurred while updating the rental duration.";

            _filmRepoMock.Setup(repo => repo.UpdateRentalDurationOfFilm(filmId, newRentalDuration))
                         .ThrowsAsync(new Exception(exceptionMessage));

            var result = await _controller.UpdateRentalDurationOfFilm(filmId, newRentalDuration);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }
        #endregion


        #region UpdateRentalRateOfFilm
        [Fact]
        public async Task UpdateRentalRateOfFilm_ReturnsOk_WhenRentalRateUpdatedSuccessfully()
        {
            int filmId = 1;
            decimal newRentalRate = 4.99m;
            var updatedFilms = new List<FilmDTO>
            {
              new FilmDTO { FilmId = filmId, RentalRate = newRentalRate }
            };

            _filmRepoMock.Setup(repo => repo.UpdateRentalRateOfFilm(filmId, newRentalRate))
                         .ReturnsAsync(updatedFilms);

            var result = await _controller.UpdateRentalRateOfFilm(filmId, newRentalRate);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(updatedFilms.Count, returnedFilms.Count);
            Assert.Equal(newRentalRate, returnedFilms[0].RentalRate);
        }

        [Fact]
        public async Task UpdateRentalRateOfFilm_ReturnsBadRequest_WhenExceptionOccurs()
        {
            int filmId = 1;
            decimal newRentalRate = 4.99m;
            var exceptionMessage = "An error occurred while updating the rental rate.";

            _filmRepoMock.Setup(repo => repo.UpdateRentalRateOfFilm(filmId, newRentalRate))
                         .ThrowsAsync(new Exception(exceptionMessage));

            var result = await _controller.UpdateRentalRateOfFilm(filmId, newRentalRate);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }

        #endregion


        #region UpdateRatingOfFilm
        [Fact]
        public async Task UpdateRatingOfFilm_ReturnsOk_WhenRatingUpdatedSuccessfully()
        {
            int filmId = 1;
            string newRating = "PG-13";
            var updatedFilms = new List<FilmDTO>
            {
              new FilmDTO { FilmId = filmId, Rating = newRating }
            };

            _filmRepoMock.Setup(repo => repo.UpdateRatingOfFilm(filmId, newRating))
                         .ReturnsAsync(updatedFilms);

            var result = await _controller.UpdateRatingOfFilm(filmId, newRating);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilms = Assert.IsType<List<FilmDTO>>(okResult.Value);
            Assert.Equal(updatedFilms.Count, returnedFilms.Count);
            Assert.Equal(newRating, returnedFilms[0].Rating);
        }

        [Fact]
        public async Task UpdateRatingOfFilm_ReturnsBadRequest_WhenInvalidRatingProvided()
        {
            int filmId = 1;
            string invalidRating = "InvalidRating";

            var result = await _controller.UpdateRatingOfFilm(filmId, invalidRating);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Please provide a valid rating: NC-17, R, PG-13, PG, or G.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateRatingOfFilm_ReturnsBadRequest_WhenExceptionOccurs()
        {
            int filmId = 1;
            string newRating = "PG-13";
            var exceptionMessage = "An error occurred while updating the rating.";

            _filmRepoMock.Setup(repo => repo.UpdateRatingOfFilm(filmId, newRating))
                         .ThrowsAsync(new Exception(exceptionMessage));

            var result = await _controller.UpdateRatingOfFilm(filmId, newRating);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }

        #endregion


        #region UpdateLanguageOfFilm
        [Fact]
        public async Task UpdateLanguageOfFilm_ReturnsOk_WhenLanguageUpdatedSuccessfully()
        {
            int filmId = 1;
            var language = new LanguageDTO { LanguageId = 1, Name = "English" };

            _filmRepoMock.Setup(repo => repo.UpdateLanguageOfFilm(filmId, language))
                         .Returns(Task.CompletedTask);

            var result = await _controller.UpdateLanguageOfFilm(filmId, language);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Updated Successfully", okResult.Value);
        }

        [Fact]
        public async Task UpdateLanguageOfFilm_ReturnsBadRequest_WhenExceptionOccurs()
        {
            int filmId = 1;
            var language = new LanguageDTO { LanguageId = 1, Name = "English" };
            var exceptionMessage = "An error occurred while updating the language.";

            _filmRepoMock.Setup(repo => repo.UpdateLanguageOfFilm(filmId, language))
                         .ThrowsAsync(new Exception(exceptionMessage));

            var result = await _controller.UpdateLanguageOfFilm(filmId, language);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }
        #endregion

        #region UpdateCategoryOfFilm
        [Fact]
        public async Task UpdateCategoryOfFilm_ReturnsOk_WhenCategoryUpdatedSuccessfully()
        {
            // Arrange
            int filmId = 1;
            int categoryId = 2;

            _filmRepoMock.Setup(repo => repo.UpdateCategoryOfFilm(filmId, categoryId))
                         .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateCategoryOfFilm(filmId, categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Updated Successfully", okResult.Value);
        }

        [Fact]
        public async Task UpdateCategoryOfFilm_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            int filmId = 1;
            int categoryId = 2;


            _filmRepoMock.Setup(repo => repo.UpdateCategoryOfFilm(filmId, categoryId))
                         .ThrowsAsync(new Exception("Update failed"));

            // Act
            var result = await _controller.UpdateCategoryOfFilm(filmId, categoryId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Update failed", badRequestResult.Value);
        }
        #endregion
    }
}