global using MongoDB.Bson;
global using MongoDB.Driver;
global using System.Text;

string policyName = "TheHostPolicy";
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

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

// "AllowedHosts": "http://localhost:3000,http://localhost:5000" in appsettings.json
var allowedHosts = builder.Configuration.GetSection("AllowedHosts").Value.Trim().Split(",");
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
                      builder =>
                      {
                          builder
                            .WithOrigins(allowedHosts)
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policyName);

app.UseAuthorization();

app.MapHealthChecks("/healthCheck");
app.MapControllers();

app.Run();
