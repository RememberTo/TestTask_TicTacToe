using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Domain
{
    public class Player
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Score { get; set; }
        public ICollection<GameResult> GameResults { get; set; } = new List<GameResult>();
    }
}
