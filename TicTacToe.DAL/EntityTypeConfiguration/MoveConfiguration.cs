using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain;

namespace TicTacToe.DAL.EntityTypeConfiguration
{
    public class MoveConfiguration : IEntityTypeConfiguration<Move>
    {
        public void Configure(EntityTypeBuilder<Move> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.GameId).IsRequired();
            builder.Property(x => x.PlayerId).IsRequired();
            builder.Property(x => x.X).IsRequired();
            builder.Property(x => x.Y).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasOne(x => x.Game)
                .WithMany(x => x.Moves)
                .HasForeignKey(x => x.GameId)
                .IsRequired();
        }
    }

}
