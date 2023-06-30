namespace TicTacToe.WebApi.Models.Dto
{
    public record class SignInRequestDto(string Username,  string Password);
    public record class SignUpRequestDto(string Username, string Password);
}