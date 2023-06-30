using Microsoft.AspNetCore.Mvc;
using TicTacToe.DAL.Interfaces;

namespace TicTacToe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        public StatController(IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
        }

        [HttpGet("player-stats:{id}")]
        public async Task<IActionResult> GetPlayerStats(int id)
        {
            try
            {
                var playerStats = await _playerRepository.GetPlayerStats(id);
                var stats = new 
                {
                    Username =  playerStats.Username,
                    Score = playerStats.Score,
                    GameResults = playerStats.GameResults.Select(gr => new
                    {
                        ResultType = gr.ResultType,
                        GameId = gr.GameId,
                        CreatedAt = gr.CreatedAt,
                    }).ToList()
                };

                return Ok(stats);
            }
            catch (Exception)
            {
                return StatusCode(500, "Server Error");
            }
        }

        [HttpGet("game-stats:{id}")]
        public async Task<IActionResult> GetGameStats(int id)
        {
            try
            {
                var gameStats = await _gameRepository.GetGameStats(id);
                var stats = new
                {
                    StartGameTime = gameStats.StartGameTime,
                    IsActive = gameStats.IsActive,
                    Player1 = new
                    {
                        Username = gameStats.Player1.Username,
                        Result = gameStats.GameResults.Select(gs => new
                        {
                            PlayerId = gs.PlayerId,
                            ResultType = gs.ResultType,
                            GameId = gs.GameId,
                            CreatedAt = gs.CreatedAt,
                        })
                        .FirstOrDefault(p => p.PlayerId == gameStats.Player1Id)
                    },
                    Player2 = new
                    {
                        Username = gameStats.Player1.Username,
                        Result = gameStats.GameResults.Select(gs => new
                        {
                            PlayerId = gs.PlayerId,
                            ResultType = gs.ResultType,
                            GameId = gs.GameId,
                            CreatedAt = gs.CreatedAt,
                        }).FirstOrDefault(p => p.PlayerId == gameStats.Player2Id)
                    },

                    Moves = gameStats.Moves.Select(gs => new
                    {
                        X = gs.X,
                        Y = gs.Y,
                        CreatedAt = gs.CreatedAt,
                    }).ToList(),
                };

                return Ok(stats);
            }
            catch (Exception)
            {
                return StatusCode(500, "Server Error");
            }
        }
    }
}
