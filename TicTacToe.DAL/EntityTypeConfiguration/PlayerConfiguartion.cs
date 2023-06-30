using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicTacToe.DAL.EntityTypeConfiguration
{
    internal class PlayerConfiguartion : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Username).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Score).IsRequired();

            builder.HasMany(x => x.GameResults)
                .WithOne(x => x.Player)
                .HasForeignKey(x => x.PlayerId)
                .IsRequired();
        }
    }
}
