using Microsoft.EntityFrameworkCore;
using TicTacToe.DAL.Interfaces;
using TicTacToe.Domain;

namespace TicTacToe.DAL.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly TicTacToeDbContext _dbContext;

        public GameRepository(TicTacToeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Game> GetById(int id)
        {
            return await _dbContext.Games.Include(g => g.Moves)
                .Include(g =>g.GameResults)
                .FirstOrDefaultAsync(g => g.Id == id); ;
        }

        public async Task<List<Game>> GetAll()
        {
            return await _dbContext.Games.ToListAsync();
        }

        public async Task Add(Game game)
        {
            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Game game)
        {
            _dbContext.Games.Update(game);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Game game)
        {
            _dbContext.Games.Remove(game);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Game> GetActiveGameByPlayerId(int id)
        {
            var activeGame = await _dbContext.Games
                .FirstOrDefaultAsync(g => (g.Player1Id == id || g.Player2Id == id) && g.IsActive);

            return activeGame;
        }

        public async Task<Game> GetGameStats(int id)
        {
            return await _dbContext.Games.Include(g => g.Moves)
                .Include(g=>g.Player1)
                .Include(g=>g.Player2)
                .Include(g=>g.GameResults)
                .FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}
