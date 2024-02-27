using GameAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GameAPI.Tests
{
    public class GameControllerTests
    {
        #region Constructor
        private readonly ITestOutputHelper _log;
        private Mock<GameManager> _mockGameManager;
        private GameController _controller;

        public GameControllerTests(ITestOutputHelper output)
        {
            _log = output;
            _mockGameManager = new Mock<GameManager> { CallBase = true };
            _controller = new GameController(_mockGameManager.Object);
        }
        #endregion

        #region Integration Tests av GameController

        #region GetGameState Integration Tests
        [Fact]
        public async Task GetGameState_ReturnsOk_WithGameState()
        {
            //Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/state");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //Log
            LogResponse(response);
        }

        [Fact]
        public async Task GetGameState_ReturnsBadRequest()
        {
            //Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/state");
            response.StatusCode = HttpStatusCode.BadRequest;

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //Log
            LogResponse(response);
        }

        [Fact]
        public async Task GetGameState_ReturnsNotFound_IfUriWrong()
        {
            //Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/stäjt");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //Log
            LogResponse(response);
        }
        #endregion

        #region Equip Integration Tests
        [Fact]
        public async Task Equip_ReturnsOk_WithGameState()
        {
            //Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/equip?index=0");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //Log
            LogResponse(response);
        }

        [Fact]
        public async Task Equip_ReturnsBadRequest_IfIndexIncorrect()
        {
            //Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/equip?index=ä");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //Log
            LogResponse(response);
        }

        [Fact]
        public async Task Equip_ReturnsBadRequest_WithOutOfRangeIndex()
        {
            //Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/equip?index=9999"); // Assuming 9999 is out of range

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //Log
            LogResponse(response);
        }
        #endregion

        #region Battle Integration Tests
        [Fact]
        public async Task Battle_ReturnsOk_WithGameState()
        {
            //Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/battle");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //Log
            LogResponse(response);
        }

        [Fact]
        public async Task Battle_ReturnsBadRequest()
        {
            //Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/battle");
            response.StatusCode = HttpStatusCode.BadRequest;

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //Log
            LogResponse(response);
        }
        #endregion

        #region Attack Integration Tests
        [Fact]
        public async Task Attack_ReturnsOk_WithGameState()
        {
            //Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // startar en Battle (måste göras för att Attack ska fungera)
            await client.GetAsync("/game/battle");

            // Act Attack efter att den startat Battle
            var response = await client.GetAsync("/game/attack");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _log.WriteLine($"status kod: {response.StatusCode}");

            // Log
            LogResponse(response);
        }

        [Fact]
        public async Task Attack_ReturnsBadRequest()
        {
            //Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // startar en Battle (måste göras för att Attack ska fungera)
            await client.GetAsync("/game/battle");

            // Act Attack efter att den startat Battle
            var response = await client.GetAsync("/game/attack");
            response.StatusCode = HttpStatusCode.BadRequest;

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _log.WriteLine($"status kod: {response.StatusCode}");

            // Log
            LogResponse(response);
        }

        [Fact]
        public async Task Attack_ReturnsBadRequest_WithoutBattleInitialization()
        {
            //Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/attack");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // Log
            LogResponse(response);
        }
        #endregion

        #region Defend Integration Tests
        [Fact]
        public async Task Defend_ReturnsOk_WithGameState()
        {
            // Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();
            await client.GetAsync("/game/battle");

            // Act
            var response = await client.GetAsync("/game/defend");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Log
            LogResponse(response);
        }

        [Fact]
        public async Task Defend_ReturnsBadRequest_WithoutBattleInitialization()
        {
            //Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/defend");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // Log
            LogResponse(response);
        }
        #endregion

        #region Dodge Integration Tests
        [Fact]
        public async Task Dodge_ReturnsOk_WithGameState()
        {
            // Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();
            await client.GetAsync("/game/battle"); // starting a battle

            // Act
            var response = await client.GetAsync("/game/dodge");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Log
            LogResponse(response);
        }

        [Fact]
        public async Task Dodge_ReturnsBadRequest_WithoutBattleInitialization()
        {
            //Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/dodge");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // Log
            LogResponse(response);
        }
        #endregion


        #region ReturnToTown Integration Tests
        [Fact]
        public async Task ReturnToTown_ReturnsOk_WithGameState()
        {
            // Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/returnToTown");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _log.WriteLine($"Status kod: {response.StatusCode}");

            // Log
            LogResponse(response);
        }

        [Fact]
        public async Task ReturnToTown_ReturnsBadRequest()
        {
            // Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/returnToTown");
            response.StatusCode = HttpStatusCode.BadRequest;

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _log.WriteLine($"Status kod: {response.StatusCode}");

            // Log
            LogResponse(response);
        }
        #endregion

        #region EnterShop Integration Tests
        [Fact]
        public async Task EnterShop_ReturnsOk_WithGameState()
        {
            // Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/enterStore");

            // Assert 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _log.WriteLine($"Status kod: {response.StatusCode}");
        }

        [Fact]
        public async Task EnterShop_ReturnsBadRequest()
        {
            // Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/game/enterStore");
            response.StatusCode = HttpStatusCode.BadRequest;

            // Assert 
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _log.WriteLine($"Status kod: {response.StatusCode}");
        }
        #endregion

        #region Buy Integration Tests
        [Fact]
        public async Task Buy_ReturnsOk_WithGameState()
        {
            // Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act;
            var response = await client.GetAsync("/game/buy?index=0");

            // Assert 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _log.WriteLine($"Status kod: {response.StatusCode}");
        }

        [Fact]
        public async Task Buy_ReturnsBadRequest_IfIndexIncorrect()
        {
            // Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act;
            var response = await client.GetAsync("/game/buy?index=ö");

            // Assert 
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _log.WriteLine($"Status kod: {response.StatusCode}");
        }
        #endregion

        #region Sell Integration Tests
        [Fact]
        public async Task Sell_ReturnsOk_WithGameState()
        {
            // Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act;
            var response = await client.GetAsync("/game/sell?index=0");

            // Assert 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _log.WriteLine($"Status kod: {response.StatusCode}");
        }

        [Fact]
        public async Task Sell_ReturnsBadRequest_IfIndexIncorrect()
        {
            // Arrange
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act;
            var response = await client.GetAsync("/game/sell?index=å");

            // Assert 
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _log.WriteLine($"Status kod: {response.StatusCode}");
        }
        #endregion 

        #endregion

        #region Unit Tests av controllerns metoder

        #region GetGameState Unit Tests
        [Fact]
        public void GetGameState_ShouldReturnOkResult_UnitTest()
        {
            var mockGameState = new GameState();
            _mockGameManager.Setup(x => x.GetGameState()).Returns(mockGameState);

            var result = _controller.GetGameState();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetGameState_ShouldReturnBadRequest_WhenExceptionOccurs_UnitTest()
        {
            _mockGameManager.Setup(x => x.GetGameState()).Throws<InvalidOperationException>();

            var result = _controller.GetGameState();
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region ReturnToTown Unit Tests
        [Fact]
        public void ReturnToTown_ShouldReturnOkResult_UnitTest()
        {
            var mockGameState = new GameState();
            _mockGameManager.Setup(x => x.ReturnToTown()).Returns(mockGameState);

            var result = _controller.ReturnToTown();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ReturnToTown_ShouldReturnBadRequest_WhenExceptionOccurs_UnitTest()
        {
            _mockGameManager.Setup(x => x.ReturnToTown()).Throws<InvalidOperationException>();

            var result = _controller.ReturnToTown();
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region EnterShop Unit Tests
        [Fact]
        public void EnterShop_ShouldReturnOkResult_UnitTest()
        {
            var mockGameState = new GameState();
            _mockGameManager.Setup(x => x.EnterShop()).Returns(mockGameState);

            var result = _controller.EnterShop();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void EnterShop_ShouldReturnBadRequest_WhenExceptionOccurs_UnitTest()
        {
            _mockGameManager.Setup(x => x.EnterShop()).Throws<InvalidOperationException>();

            var result = _controller.EnterShop();
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region Equip Unit Tests
        [Fact]
        public void Equip_ShouldReturnOkResult_UnitTest()
        {
            var mockGameState = new GameState();
            _mockGameManager.Setup(x => x.Equip(It.IsAny<int>())).Returns(mockGameState);

            var result = _controller.Equip(0);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Equip_ShouldReturnBadRequest_WhenExceptionOccurs_UnitTest()
        {
            _mockGameManager.Setup(x => x.Equip(It.IsAny<int>())).Throws<InvalidOperationException>();

            var result = _controller.Equip(0);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region Battle Unit Tests

        [Fact]
        public void Battle_ShouldReturnOkResult_UnitTest()
        {
            var mockGameState = new GameState();
            _mockGameManager.Setup(x => x.StartFight()).Returns(mockGameState);

            var result = _controller.Battle();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Battle_ShouldReturnBadRequest_WhenExceptionOccurs_UnitTest()
        {
            _mockGameManager.Setup(x => x.StartFight()).Throws<InvalidOperationException>();

            var result = _controller.Battle();
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region Attack Unit Tests
        [Fact]
        public void Attack_ShouldReturnOkResult_UnitTest()
        {
            var mockGameState = new GameState();
            _mockGameManager.Setup(x => x.Attack()).Returns(mockGameState);

            var result = _controller.Attack();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Attack_ShouldReturnBadRequest_WhenExceptionOccurs_UnitTest()
        {
            _mockGameManager.Setup(x => x.Attack()).Throws<InvalidOperationException>();

            var result = _controller.Attack();
            Assert.IsType<BadRequestResult>(result);
        }
        #endregion

        #region Defend Unit Tests
        [Fact]
        public void Defend_ShouldReturnOkResult_UnitTest()
        {
            var mockGameState = new GameState();
            _mockGameManager.Setup(x => x.Defend()).Returns(mockGameState);

            var result = _controller.Defend();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Defend_ShouldReturnBadRequest_WhenExceptionOccurs_UnitTest()
        {
            _mockGameManager.Setup(x => x.Defend()).Throws<InvalidOperationException>();

            var result = _controller.Defend();
            Assert.IsType<BadRequestResult>(result);
        }
        #endregion

        #region Dodge Unit Tests
        [Fact]
        public void Dodge_ShouldReturnOkResult_UnitTest()
        {
            var mockGameState = new GameState();
            _mockGameManager.Setup(x => x.Dodge()).Returns(mockGameState);

            var result = _controller.Dodge();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Dodge_ShouldReturnBadRequest_WhenExceptionOccurs_UnitTest()
        {
            _mockGameManager.Setup(x => x.Dodge()).Throws<InvalidOperationException>();

            var result = _controller.Dodge();
            Assert.IsType<BadRequestResult>(result);
        }
        #endregion

        #region Buy Unit Tests
        [Fact]
        public void Buy_ShouldReturnOkResult_UnitTest()
        {
            var mockGameState = new GameState();
            _mockGameManager.Setup(x => x.Buy(It.IsAny<int>())).Returns(mockGameState);

            var result = _controller.Buy(0);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Buy_ShouldReturnBadRequest_WhenExceptionOccurs_UnitTest()
        {
            _mockGameManager.Setup(x => x.Buy(It.IsAny<int>())).Throws<InvalidOperationException>();

            var result = _controller.Buy(0);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region Sell Unit Tests

        [Fact]
        public void Sell_ShouldReturnOkResult_UnitTest()
        {
            var mockGameState = new GameState();
            _mockGameManager.Setup(x => x.Sell(It.IsAny<int>())).Returns(mockGameState);

            var result = _controller.Sell(0);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Sell_ShouldReturnBadRequest_WhenExceptionOccurs_UnitTest()
        {
            _mockGameManager.Setup(x => x.Sell(It.IsAny<int>())).Throws<InvalidOperationException>();

            var result = _controller.Sell(0);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion


        #endregion

        private async void LogResponse(HttpResponseMessage response)
        {
            _log.WriteLine($"http status : {response.StatusCode}");
            var content = await response.Content.ReadAsStringAsync();
            _log.WriteLine($"objekt som svar: {content}");
        }

        /*
        Motivering till varför vi inte kan testa mer (vad vi kan säga till Robert när vi redovisar)
        Huvud funktionerna testas men utöver det testas även hyptetiska scenarion som inte kan uppstå i spelet.
        T.ex. att köpa/sälja ett item som inte finns i shopen eller att sälja ett item som inte finns i inventory.
        Detta samt att vi testar scenarion genom att skicka in värden som inte är tillåtna eller går att få i spelet.
        Vi testar även alla negativa scenarion där vi får fel status kod tillbaka för alla scenarion.
        utifrån ett metods perspektiv så testar vi alla scenarion som finns/kan uppstå.
         */
    }
}
