using Microsoft.EntityFrameworkCore;
using TicTacToe.DAL.Interfaces;
using TicTacToe.Domain;

namespace TicTacToe.DAL.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly TicTacToeDbContext _dbContext;

        public PlayerRepository(TicTacToeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Player> GetPlayerById(int? id)
        {
            return await _dbContext.Players.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Player> GetPlayerByUsername(string username)
        {
            return await _dbContext.Players.FirstOrDefaultAsync(p => p.Username == username);
        }
        public async Task AddResultGame(int? idPlayer, GameResult result)
        {
            var player = await _dbContext.Players.FirstOrDefaultAsync(p => p.Id == idPlayer);
            if (player == null)
                return;

            player.GameResults.Add(result);
            _dbContext.SaveChanges();
        }

        public async Task AddPlayer(Player player)
        {
            _dbContext.Players.Add(player);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePlayer(Player player)
        {
            _dbContext.Players.Update(player);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePlayer(Player player)
        {
            _dbContext.Players.Remove(player);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Player> GetPlayerStats(int id)
        {
           return await _dbContext.Players.Include(p => p.GameResults).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
