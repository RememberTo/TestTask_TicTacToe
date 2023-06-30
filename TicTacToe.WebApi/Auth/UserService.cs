using Microsoft.EntityFrameworkCore;
using TicTacToe.DAL;
using TicTacToe.Domain;

public class UserService : IUserService
{
    private readonly TicTacToeDbContext _dbContext;

    public UserService(TicTacToeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Player> Authenticate(string username, string password)
    {
        var user = await _dbContext.Players.FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
            return null;

        if (!VerifyPassword(password, user.Password))
            return null;

        return user;
    }

    public async Task Register(Player user)
    {
        try
        {
            var existingUser = await _dbContext.Players.FirstOrDefaultAsync(x => x.Username == user.Username);
            if (existingUser != null)
            {
                throw new Exception("Username already exists");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var newPlayer = new Player
            {
                Username = user.Username,
                Password = hashedPassword,
                Score = 0,
                GameResults = new List<GameResult>(),
            };

            await _dbContext.Players.AddAsync(newPlayer);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Registration failed", ex);
        }
    }

    private bool VerifyPassword(string password, string userPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, userPassword);
    }
}