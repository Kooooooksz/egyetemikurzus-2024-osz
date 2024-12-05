using NUnit.Framework;
using Moq;
using IX0WHB.Controllers;
using IX0WHB.Models;
using System;
using System.Collections.Generic;
using Match = IX0WHB.Models.Match;

namespace IX0WHB.Tests.Controllers
{
    [TestFixture]
    public class MatchControllerTests
    {
        private MatchController _controller;
        private Mock<MatchFileHandler> _mockFileHandler;

        [SetUp]
        public void SetUp()
        {
            // Mock-oljuk a MatchFileHandler-t
            _mockFileHandler = new Mock<MatchFileHandler>("fakepath");
            _mockFileHandler.Setup(f => f.LoadMatches()).Returns(new List<Match>());
            _mockFileHandler.Setup(f => f.SaveMatches(It.IsAny<List<Match>>())).Verifiable();

            // Az új MatchController példányosítása a mockolt fájlkezelővel
            _controller = new MatchController("fakepath")
            {
                _fileHandler = _mockFileHandler.Object
            };
        }

        [Test]
        public void AddMatch_ShouldAddMatchSuccessfully()
        {
            // Arrange
            var match = new Match("Team A", "Team B", "Stadium", new DateTime(2024, 12, 1), 3, 2);

            // Act
            _controller.AddMatch(match);

            // Assert
            _mockFileHandler.Verify(f => f.SaveMatches(It.IsAny<List<Match>>()), Times.Once);
            Assert.Contains(match, _controller.Matches);
        }

        [Test]
        public void FilterMatches_ShouldReturnFilteredMatches()
        {
            // Arrange
            var match1 = new Match("Team A", "Team B", "Stadium", new DateTime(2024, 12, 1), 3, 2);
            var match2 = new Match("Team C", "Team D", "Stadium", new DateTime(2024, 12, 2), 1, 1);
            _controller.AddMatch(match1);
            _controller.AddMatch(match2);

            // Act
            var filteredMatches = _controller.FilterMatches(2);

            // Assert
            Assert.Contains(match1, filteredMatches);
            Assert.DoesNotContain(match2, filteredMatches);
        }

        [Test]
        public void DeleteMatch_ShouldRemoveMatchSuccessfully()
        {
            // Arrange
            var match = new Match("Team A", "Team B", "Stadium", new DateTime(2024, 12, 1), 3, 2);
            _controller.AddMatch(match);

            // Act
            _controller.DeleteMatch(match);

            // Assert
            Assert.DoesNotContain(match, _controller.Matches);
        }
    }
}
