using ClickerC3p0.ClickerApiKeys;
using ClickerC3p0.ClickerApps;
using ClickerC3p0.ClickerEvents;
using ClickerC3p0.ClickerUsers;
using ClickerC3p0.Clicks;
using ClickerC3p0.Database;
using Share;
using Share.dto;
using Share.Kafka;

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

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new NpgsqlDbConnectionFactory(builder.Configuration["Database:ConnectionString"]!));
builder.Services.Configure<KafkaOptions>(
    builder.Configuration.GetSection("Kafka"));

// Inject Kafka Service
builder.Services.AddSingleton<KafkaService>();
// Inject other services
builder.Services.AddSingleton<ClickEventsService>();
builder.Services.AddSingleton<ClickerUserService>();
builder.Services.AddSingleton<ClickerApiKeyService>();
builder.Services.AddSingleton<ClickerAppService>();

// Inject kafka factories
builder.Services.AddSingleton<ProducerFactory<string>>();
builder.Services.AddSingleton<ConsumerFactory<string>>();

var app = builder.Build();

Console.WriteLine($"Configuration Time: {DateTime.UtcNow} UTC");

foreach (var c in builder.Configuration.AsEnumerable())
{
    Console.WriteLine(c.Key + " = " + c.Value);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// .NET Minimal APIs using REPR (Request-Endpoint-Response) Design Pattern.
// users
app.MapClickerUsersEndpoints()
    .MapClickerApiKeysEndpoints()
    .MapClickerAppsEndpoints()
    .MapClickerEventsEndpoint()
    .MapClickEndpoint();

app.Run();