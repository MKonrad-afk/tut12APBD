using Microsoft.EntityFrameworkCore;
using tut12.Data;
using tut12.repositories;
using tut12.services;

namespace tut12;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<ITripRepository, TripRepository>();
        builder.Services.AddScoped<ITripService, TripService>();
        builder.Services.AddDbContext<MasterContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddOpenApi();
        
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}