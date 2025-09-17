using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Read connection string from env variable (Render)
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") 
                       ?? builder.Configuration.GetConnectionString("Ayurvedic_remedy");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Render needs correct port binding
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://*:{port}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
