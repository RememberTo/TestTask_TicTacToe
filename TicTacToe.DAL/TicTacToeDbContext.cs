using Microsoft.EntityFrameworkCore;
using TicTacToe.DAL.EntityTypeConfiguration;
using TicTacToe.Domain;

namespace TicTacToe.DAL
{
    public class TicTacToeDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameResult> GameResults { get; set; }
        public DbSet<Move> Moves { get; set; }
        public TicTacToeDbContext(DbContextOptions<TicTacToeDbContext> options) : base(options)
        {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new GameResultConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerConfiguartion());
            modelBuilder.ApplyConfiguration(new MoveConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
