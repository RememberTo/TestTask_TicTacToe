using TicTacToe.Domain;

namespace TicTacToe.DAL.Interfaces
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayerById(int? id);
        Task<Player> GetPlayerByUsername(string username);
        Task AddResultGame(int? idPlayer, GameResult result);
        Task AddPlayer(Player player);
        Task UpdatePlayer(Player player);
        Task DeletePlayer(Player player);
        Task<Player> GetPlayerStats(int id);
    }
}
