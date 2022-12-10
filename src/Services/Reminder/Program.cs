global using System.Text;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

string policyName = "TheHostPolicy";

var builder = WebApplication.CreateBuilder(args);

string mongoUri = builder.Configuration["MongoUri"];

// Add services to the container.
builder.Services.AddScoped<IReminderService, ReminderManager>();
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
builder.Services.AddSingleton<IMongoClient, MongoClient>(s =>
{
    return new MongoClient(mongoUri);
});
builder.Services.AddHangfire(conf => conf
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseMongoStorage(
        mongoUri,
        "hangfiredb",
        new MongoStorageOptions
        {
            MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            },
            Prefix = "hangfire.mongo",
            CheckConnection = true
        })
);
builder.Services.AddHangfireServer(serverOptions =>
{
    serverOptions.ServerName = "Reminder Hangfire";
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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/// TODO: configure authorization
/// https://docs.hangfire.io/en/latest/configuration/using-dashboard.html#toc-entry-2
app.UseHangfireDashboard("/hangfire");

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
