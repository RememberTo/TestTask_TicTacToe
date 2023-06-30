using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Domain
{
    public class GameResult
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int? PlayerId { get; set; }
        public Player Player { get; set; }
        public ResultType ResultType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
