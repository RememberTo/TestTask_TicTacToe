namespace TicTacToe.WebApi.Models.Dto
{
    public class MakeMoveDto
    {
        public int GameId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}