using Microsoft.AspNetCore.Mvc;

namespace GameAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        protected virtual GameManager _gameManager { get; }

        public GameController(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        [HttpGet("state")]
        public IActionResult GetGameState()
        {
            try
            {
                var gameState = _gameManager.GetGameState();
                return Ok(gameState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("returnToTown")]
        public IActionResult ReturnToTown()
        {
            try
            {
                var gameState = _gameManager.ReturnToTown();
                return Ok(gameState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("enterStore")]
        public IActionResult EnterShop()
        {
            try
            {
                var gameState = _gameManager.EnterShop();
                return Ok(gameState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("equip")]
        public IActionResult Equip(int index) // Index for which item in inventory is being equipped
        {
            try
            {
                var gameState = _gameManager.Equip(index);
                return Ok(gameState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("battle")]
        public IActionResult Battle()
        {
            try
            {
                var gameState = _gameManager.StartFight();
                return Ok(gameState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("attack")]
        public IActionResult Attack()
        {
            try
            {
                var gameState = _gameManager.Attack();
                return Ok(gameState);
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet("defend")]
        public IActionResult Defend()
        {
            try
            {
                var gameState = _gameManager.Defend();
                return Ok(gameState);
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet("dodge")]
        public IActionResult Dodge()
        {
            try
            {
                var gameState = _gameManager.Dodge();
                return Ok(gameState);
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
            return BadRequest();
        }


        [HttpGet("buy")]
        public IActionResult Buy(int index)
        {
            try
            {
                var gameState = _gameManager.Buy(index);
                return Ok(gameState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("sell")]
        public IActionResult Sell(int index)
        {
            try
            {
                var gameState = _gameManager.Sell(index);
                return Ok(gameState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
