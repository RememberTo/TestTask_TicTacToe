using TicTacToe.Domain;

public interface IUserService
{
    Task<Player> Authenticate(string username, string password);
    Task Register(Player user);
}