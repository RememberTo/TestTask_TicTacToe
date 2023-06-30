using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TicTacToe.DAL.Interfaces;
using TicTacToe.DAL.Repository;

namespace TicTacToe.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection
            services, IConfiguration configuration)
        {

            var connectionString = configuration["DbConnection"];
            services.AddDbContext<TicTacToeDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IGameRepository, GameRepository>();

            return services;
        }
    }
}
