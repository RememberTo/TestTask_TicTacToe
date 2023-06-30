using Notes.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Domain
{
    public class Game
    {
        public int Id { get; set; }
        public int? Player1Id { get; set; }
        public int? Player2Id { get; set; }
        public Player? Player1 { get; set; }
        public Player? Player2 { get; set; }
        public string? SymbolPlayer1 { get; set; }
        public string? SymbolPlayer2 { get; set; }
        public char[,] Board { get; set; } = new char[3,3];
        public bool IsActive { get; set; }
        public int? CurrentPlayerId { get; set; }
        public Player? CurrentPlayer { get; set; }
        public DateTime? StartGameTime { get; set; }
        public DateTime? NextMoveTime { get; set; }

        public ICollection<Move>? Moves { get; set; } = new List<Move>();
        public ICollection<GameResult>? GameResults { get; set; } = new List<GameResult>();

        public void MakeMove(int x, int y)
        {
            if (IsActive)
            {
                if (Board[x, y] == '\0')
                {
                    var move = new Move
                    {
                        PlayerId = CurrentPlayer.Id,
                        GameId = Id,
                        X = x,
                        Y = y,
                        CreatedAt = DateTime.Now,
                    };

                    Moves.Add(move);
                    Board[x, y] = CurrentPlayer.Id == Player1Id ? SymbolPlayer1[0] : SymbolPlayer2[0];
                }
                else
                {
                    throw new CellOccupiedException(x, y);
                }
            }
        }

        public bool IsGameOver()
        {
            var isBoardFull = IsBoardFull();
            var winPlayer = CalculateWinner();

            if(isBoardFull || winPlayer != null)
            {
                var gameResultPlayer1 = new GameResult
                {
                    GameId = Id,
                    PlayerId = Player1Id,
                    CreatedAt = DateTime.Now
                };
                var gameResultPlayer2 = new GameResult
                {
                    GameId = Id,
                    PlayerId = Player2Id,
                    CreatedAt = DateTime.Now
                };

                if (winPlayer != null)
                {
                    if (winPlayer == Player1)
                    {
                        gameResultPlayer1.ResultType = Enums.ResultType.Win;
                        Player1.Score += 2;
                        gameResultPlayer2.ResultType = Enums.ResultType.Loss;
                    }
                    else
                    {
                        gameResultPlayer1.ResultType = Enums.ResultType.Loss;
                        gameResultPlayer2.ResultType = Enums.ResultType.Win;
                        Player2.Score += 2;
                    }
                }
                else
                {
                    gameResultPlayer1.ResultType = Enums.ResultType.Draw;
                    Player1.Score += 1;
                    gameResultPlayer2.ResultType = Enums.ResultType.Draw;
                    Player2.Score += 1;
                }

                GameResults.Add(gameResultPlayer1);
                GameResults.Add(gameResultPlayer2);
            }

            return isBoardFull || winPlayer != null;
        }

        private Player? CalculateWinner()
        {
            char[,] winningPatterns = new char[,]
        {
            { Board[0, 0], Board[0, 1], Board[0, 2] },
            { Board[1, 0], Board[1, 1], Board[1, 2] },
            { Board[2, 0], Board[2, 1], Board[2, 2] },
            { Board[0, 0], Board[1, 0], Board[2, 0] },
            { Board[0, 1], Board[1, 1], Board[2, 1] },
            { Board[0, 2], Board[1, 2], Board[2, 2] },
            { Board[0, 0], Board[1, 1], Board[2, 2] },
            { Board[0, 2], Board[1, 1], Board[2, 0] }
        };

            for (int i = 0; i < winningPatterns.GetLength(0); i++)
            {
                char firstCell = winningPatterns[i, 0];
                if (firstCell != '\0' && winningPatterns[i, 1] == firstCell && winningPatterns[i, 2] == firstCell)
                {
                    if (firstCell == SymbolPlayer1[0])
                    {
                        return Player1;
                    }
                    else if (firstCell == SymbolPlayer2[0])
                    {
                        return Player2;
                    }
                }
            }

            return null;
        }

        private bool IsBoardFull()
        {
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    if (Board[i, j] == '\0')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

    }
}
