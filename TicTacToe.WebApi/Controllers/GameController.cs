using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using TicTacToe.DAL.Interfaces;
using TicTacToe.DAL.Repository;
using TicTacToe.Domain;
using TicTacToe.WebApi.Models.Dto;

namespace TicTacToe.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        public GameController(IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
        }

        [HttpPost("create-game")]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameDto createGameDto)
        {
            try
            {
                var player = await _playerRepository.GetPlayerByUsername(User.Identity.Name);

                if (player == null)
                    return Unauthorized();

                var existingGame = await _gameRepository.GetActiveGameByPlayerId(player.Id);
                if (existingGame != null)
                    return BadRequest("Player is already participating in an active game");

                if (!(createGameDto.SymbolPlayer1 == "x" || createGameDto.SymbolPlayer1 == "o"))
                    return BadRequest("Invalid Symbol");

                var game = new Game
                {
                    Player1Id = player.Id,
                    SymbolPlayer1 = createGameDto.SymbolPlayer1,
                    CurrentPlayerId = player.Id,
                };

                await _gameRepository.Add(game);

                return Ok(game.Id);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Database Error");
            }
            catch (Exception)
            {
                return StatusCode(500, "Server Error");
            }
        }

        [HttpPost("join-game")]
        public async Task<IActionResult> JoinGame([FromBody] JoinGameDto joinGameDto)
        {
            try
            {
                var player = await _playerRepository.GetPlayerByUsername(User.Identity.Name);

                if (player == null)
                    return Unauthorized();

                var existingGame = await _gameRepository.GetActiveGameByPlayerId(player.Id);
                if (existingGame != null)
                    return BadRequest("Player is already participating in an active game");

                if (!(joinGameDto.SymbolPlayer2 == "x" || joinGameDto.SymbolPlayer2 == "o"))
                    return BadRequest("Invalid Symbol");

                var game = await _gameRepository.GetById(joinGameDto.GameId);
                if (game == null)
                    return NotFound();
                ValidateData(game, player.Id, joinGameDto.SymbolPlayer2);

                game.Player2Id = player.Id;
                game.SymbolPlayer2 = joinGameDto.SymbolPlayer2;
                game.IsActive = true;
                game.StartGameTime = DateTime.Now;
                game.NextMoveTime = DateTime.Now;

                await _gameRepository.Update(game);

                return Ok(game.Id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Database Error");
            }
            catch (Exception)
            {
                return StatusCode(500, "Server Error");
            }
        }

        [HttpPost("make-move")]
        public async Task<IActionResult> MakeMove([FromBody] MakeMoveDto makeMoveDto)
        {
            try
            {
                var player = await _playerRepository.GetPlayerByUsername(User.Identity.Name);
                if (player == null)
                    return Unauthorized();

                var existingGame = await _gameRepository.GetActiveGameByPlayerId(player.Id);
                if (existingGame != null)
                    return BadRequest("Player is already participating in an active game");

                var game = await _gameRepository.GetById(makeMoveDto.GameId);
                if (game == null)
                    return NotFound();


                if (game.Player1 != player && game.Player2 != player)
                    return Forbid();

                if (!game.IsActive)
                    return Ok("Game over");

                var lastSecondMove = (DateTime.Now - game.NextMoveTime).Value.TotalSeconds;

                if (lastSecondMove > 15)
                {
                    if ((int)(lastSecondMove / 15) % 2 == 0)
                    {
                        if (game.Player1 == player)
                            game.CurrentPlayer = game.Player1;
                        else
                            game.CurrentPlayer = game.Player2;
                    }
                    else
                    {
                        if (game.Player1 == player)
                            game.CurrentPlayer = game.Player2;
                        else
                            game.CurrentPlayer = game.Player1;

                        await _gameRepository.Update(game);

                        return Ok("Move passed to the next player.");
                    }
                }
                else
                {
                    game.CurrentPlayer = player;
                }

                game.MakeMove(makeMoveDto.X, makeMoveDto.Y);

                if (game.Moves.Count > 2)
                {
                    if (game.IsGameOver())
                    {

                        _playerRepository.AddResultGame(game.Player1Id, 
                            game.GameResults.FirstOrDefault(x => x.PlayerId == game.Player1Id));

                        _playerRepository.AddResultGame(game.Player2Id,
                            game.GameResults.FirstOrDefault(x => x.PlayerId == game.Player2Id));

                        game.IsActive = false;

                        await _gameRepository.Update(game);

                        if (game.GameResults.FirstOrDefault(x => x.ResultType == Domain.Enums.ResultType.Draw) != null)
                        {
                            return Ok("The game ended in a draw");
                        }
                        else
                        {
                            var winner = game.GameResults.FirstOrDefault(x => x.ResultType == Domain.Enums.ResultType.Win);
                            var loser = game.GameResults.FirstOrDefault(x => x.ResultType == Domain.Enums.ResultType.Loss);

                            if (player.Id == winner.PlayerId)
                                return Ok("You win");
                            else
                                return Ok("You loss");
                        }
                    }
                }

                game.NextMoveTime = DateTime.Now;

                if (game.Player1 == player)
                    game.CurrentPlayer = game.Player2;
                else
                    game.CurrentPlayer = game.Player1;

                await _gameRepository.Update(game);

                return Ok();
            }
            catch (CellOccupiedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Server Error");
            }
        }

        private void ValidateData(Game game, int Player2Id, string symbolPlayer2)
        {
            if (game.Player1Id == Player2Id) throw new ArgumentException("The same player cannot participate in the game");
            if (game.SymbolPlayer1 == symbolPlayer2) throw new ArgumentException("Players cannot have the same symbols");
            if (game.Player2Id != null) throw new ArgumentException("Only 2 players can participate in the game");
        }
    }
}
