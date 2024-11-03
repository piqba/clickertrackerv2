
using ClickerC3p0.ClickerApiKeys;
using ClickerC3p0.ClickerApps;
using ClickerC3p0.ClickerUsers;
using ClickerC3p0.Database;

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
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddSingleton<IDBConnectionFactory>(_ =>
    new NpgsqlDbConnectionFactory(builder.Configuration["Database:ConnectionString"]!));


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

// .NET Minimal APIs using REPR (Request-Endpoint-Response) Design Pattern.
// users
app.MapClickerUsersEndpoints()
    .MapClickerApiKeysEndpoints()
    .MapClickerAppsEndpoints();

app.Run();