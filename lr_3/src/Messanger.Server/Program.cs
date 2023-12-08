using Messanger.Server.Data;
using Messanger.Server.Data.DataProfiles.Base;
using Messanger.Server.Data.Services;
using Messanger.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
IConfiguration config = builder.Configuration;

// Add services to the container.
builder.Services.AddGrpc();
IDataProfileFactory dataProfileFactory = new DataProfileFactory(config);
DataProfile dataProfile = dataProfileFactory.CreateProfile();
// Add services to the container.

builder.Services
    .AddDbContext<MessangerDataContext>(dataProfile.ConfigureDbContextOptionsBuilder);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<MessangerService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
