using Microsoft.AspNetCore.Authentication;
using TicTacToe.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAuthentication("BasicAuthentication")
        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    try
    {
        SQLitePCL.Batteries.Init();
        var context = serviceProvider.GetRequiredService<TicTacToeDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception)
    {
        Console.WriteLine("Db not initilize");
    }
}
// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();