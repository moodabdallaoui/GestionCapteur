using Xunit;
using Microsoft.AspNetCore.Mvc;
using GestionCapteurs.Controllers;
using GestionCapteurs.Data;
using GestionCapteurs.Data.Model;


namespace GestionCapteurs.Tests
{
    public class CapteurControllerTests
    {
        private readonly CapteurController _controller;
        private readonly AppDbContext _dbContext;

        [Fact]
        public async Task GetCapteurs_ReturnsAllCapteurs()
        {
            // Act
            var result = await _controller.GetCapteurs();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Capteur>>>(result);
            var capteurs = Assert.IsType<List<Capteur>>(actionResult.Value);
            Assert.Equal(2, capteurs.Count);
        }

        [Fact]
        public async Task GetCapteur_ReturnsCapteur_WhenCapteurExists()
        {
            // Act
            var result = await _controller.GetCapteur(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Capteur>>(result);
            var capteur = Assert.IsType<OkObjectResult>(actionResult.Result).Value as Capteur;
            Assert.NotNull(capteur);
            Assert.Equal("capteur1", capteur.Name);
        }

        [Fact]
        public async Task GetCapteur_ReturnsNotFound_WhenCapteurDoesNotExist()
        {
            // Act
            var result = await _controller.GetCapteur(999);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Capteur>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task CreateCapteur_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newCapteur = new Capteur
            {
                Id = 3,
                Name = "capteur3",
                Type = "Humidity",
                Unit = "%"
            };

            // Act
            var result = await _controller.CreateCapteur(newCapteur);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Capteur>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var capteur = Assert.IsType<Capteur>(createdResult.Value);
            Assert.Equal("capteur3", capteur.Name);

            // Verify the capteur is added
            Assert.Equal(3, _dbContext.Capteurs.Count());
        }

        [Fact]
        public async Task UpdateCapteur_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var updatedCapteur = new Capteur
            {
                Id = 1,
                Name = "Updatedcapteur1",
                Type = "UpdatedType",
                Unit = "UpdatedUnit"
            };

            // Act
            var result = await _controller.UpdateCapteur(1, updatedCapteur);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify the updates
            var capteur = await _dbContext.Capteurs.FindAsync(1);
            Assert.Equal("Updatedcapteur1", capteur.Name);
        }

        [Fact]
        public async Task UpdateCapteur_ReturnsNotFound_WhenCapteurDoesNotExist()
        {
            // Arrange
            var updatedCapteur = new Capteur
            {
                Id = 999,
                Name = "NonExistentcapteur",
                Type = "Type",
                Unit = "Unit"
            };

            // Act
            var result = await _controller.UpdateCapteur(999, updatedCapteur);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteCapteur_ReturnsNoContent_WhenCapteurExists()
        {
            // Act
            var result = await _controller.DeleteCapteur(1);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify the deletion
            Assert.Null(await _dbContext.Capteurs.FindAsync(1));
            Assert.Single(_dbContext.Capteurs); 
        }

        [Fact]
        public async Task DeleteCapteur_ReturnsNotFound_WhenCapteurDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteCapteur(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
