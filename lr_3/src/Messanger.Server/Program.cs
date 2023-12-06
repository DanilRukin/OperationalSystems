using Messanger.Server.Data;
using Messanger.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
IConfiguration config = builder.Configuration;

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<MessangerDataContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(config.GetConnectionString("Messanger_test"), sql => sql.MigrationsAssembly("Messanger.Server")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<MessangerService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
