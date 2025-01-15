using ColorsManagement.Db;
using ColorsManagement.Implementations.Repositories;
using ColorsManagement.Implementations.Services;
using ColorsManagement.Interfaces.Repositories;
using ColorsManagement.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  
              .AllowAnyMethod()  
              .AllowAnyHeader(); 
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSingleton<DatabaseSetup>(sp => new DatabaseSetup(connectionString));
builder.Services.AddScoped<IColorRepository,ColorRepository>();
builder.Services.AddScoped<IColorService,ColorService>();

var app = builder.Build();

app.UseCors("AllowAllOrigins");

var databaseSetup = app.Services.GetRequiredService<DatabaseSetup>();
databaseSetup.CreateDatabaseAndTables();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
