global using MongoDB.Bson;
global using MongoDB.Driver;
global using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(s =>
{
    AppSettings appSettings = new();
    builder.Configuration.Bind("Salt", appSettings.Salt);
    appSettings.IV = s.GetRequiredService<IConfiguration>()["IV"];
    appSettings.MongoUri = s.GetRequiredService<IConfiguration>()["MongoUri"];
    appSettings.AuthSecretKey = s.GetRequiredService<IConfiguration>()["AuthSecretKey"];
    return appSettings;
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddSingleton<IMongoClient, MongoClient>(s =>
{
    var uri = s.GetRequiredService<IConfiguration>()["MongoUri"];
    return new MongoClient(uri);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
