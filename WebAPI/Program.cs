using WebAPI.Hubs;
using WebAPI.Interfaces;
using WebAPI.Repo;

var builder = WebApplication.CreateBuilder();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IInMemoryRepo,InMemoryRepo>();
var app = builder.Build();

app.UseCors(b => {
    b.WithOrigins("https://localhost:7143");
    b.AllowAnyHeader();
    b.AllowAnyMethod();
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PersonHub>("/personhub");
app.Run();
