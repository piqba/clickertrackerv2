using System.Collections.Concurrent;
using System.Text.Json;
using ClickerR2d2.Clicks;
using ClickerR2d2.dto;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Share;


var builder = WebApplication.CreateBuilder(args);
const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging/?view=aspnetcore-8.0#enable-http-logging
builder.Services.AddHttpLogging(o => { });
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:8081")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.Configure<KafkaOptions>(
    builder.Configuration.GetSection("Kafka"));

var app = builder.Build();


Console.WriteLine($"Configuration Time: {DateTime.UtcNow} UTC");

foreach (var c in builder.Configuration.AsEnumerable())
{
    Console.WriteLine(c.Key + " = " + c.Value);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();
app.UseCors(myAllowSpecificOrigins);

app.MapGet("/clicker", () => "r2d2")
    .WithName("ClickerR2d2")
    .WithOpenApi();

app.MapClickEndpoint();

app.Run();