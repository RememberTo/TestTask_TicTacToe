
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Domain;

namespace TicTacToe.DAL.EntityTypeConfiguration
{
    public class GameResultConfiguration : IEntityTypeConfiguration<GameResult>
    {
        public void Configure(EntityTypeBuilder<GameResult> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ResultType).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasOne(x => x.Player)
                .WithMany(x => x.GameResults)
                .HasForeignKey(x => x.PlayerId)
                .IsRequired();

            builder.HasOne(x => x.Game)
                .WithMany(x => x.GameResults)
                .HasForeignKey(x => x.GameId)
                .IsRequired();
        }
    }
}
