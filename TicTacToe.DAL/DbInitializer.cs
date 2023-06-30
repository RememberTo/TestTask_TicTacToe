
namespace TicTacToe.DAL
{
    public class DbInitializer
    {
        public static void Initialize(TicTacToeDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();
        }
    }
}
