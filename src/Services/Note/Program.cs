global using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

string policyName = "TheHostPolicy";
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
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteManager>();
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

var secretKey = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AuthSecretKey").Value);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
