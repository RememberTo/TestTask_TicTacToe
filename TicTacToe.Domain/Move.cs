using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Domain
{
    public class Move
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int PlayerId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
