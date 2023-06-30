using TicTacToe.Domain;

namespace TicTacToe.DAL.Interfaces
{
    public interface IGameRepository
    {
        Task<Game> GetById(int id);
        Task<List<Game>> GetAll();
        Task Add(Game game);
        Task Update(Game game);
        Task Delete(Game game);
        Task<Game> GetActiveGameByPlayerId(int id);
        Task<Game> GetGameStats(int id);
    }
}