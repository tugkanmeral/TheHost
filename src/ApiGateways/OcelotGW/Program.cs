using Ocelot.DependencyInjection;
using Ocelot.Middleware;

string policyName = "TheHostPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddOcelot();

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

//if (app.Environment.IsDevelopment())
//{
// app.UseSwagger();
// app.UseSwaggerUI();
//}

// app.UseHttpsRedirection();

app.UseCors(policyName);

app.UseAuthorization();

app.MapControllers();

app.UseOcelot().Wait();

app.Run();