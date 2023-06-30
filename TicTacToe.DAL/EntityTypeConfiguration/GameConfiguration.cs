using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text;
using TicTacToe.Domain;

namespace TicTacToe.DAL.EntityTypeConfiguration
{
    internal class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.SymbolPlayer1).HasMaxLength(1).IsRequired(false);
            builder.Property(x => x.SymbolPlayer2).HasMaxLength(1).IsRequired(false);
            builder.Property(x => x.Board).HasConversion(
                v => ConvertBoardToString(v),
                v => ConvertStringToBoard(v)).HasMaxLength(9).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.StartGameTime).IsRequired(false);
            builder.Property(x => x.NextMoveTime).IsRequired(false);

            builder.HasOne(x => x.Player1)
                .WithMany()
                .HasForeignKey(x => x.Player1Id)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Player2)
                .WithMany()
                .HasForeignKey(x => x.Player2Id)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CurrentPlayer)
                .WithMany()
                .HasForeignKey(x => x.CurrentPlayerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.GameResults)
                .WithOne(x => x.Game)
                .HasForeignKey(x => x.GameId)
                .IsRequired();

            builder.HasMany(x => x.Moves)
                .WithOne()
                .HasForeignKey(x => x.GameId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

        private string ConvertBoardToString(char[,] board)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    sb.Append(board[i, j]);
                }
            }
            return sb.ToString();
        }

        private char[,] ConvertStringToBoard(string value)
        {
            char[,] board = new char[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (string.IsNullOrEmpty(value))
                        continue;
                    board[i, j] = value[i * 3 + j];
                }
            }
            return board;
        }
    }
}
