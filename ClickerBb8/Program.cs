using ClickerBb8;
using ClickerBb8.Database;
using ClickerBb8.Service;
using Share;
using Share.dto;
using Share.Kafka;


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new NpgsqlDbConnectionFactory(builder.Configuration["Database:ConnectionString"]!));
// Inject kafka factories
builder.Services.AddSingleton<KafkaService>();
builder.Services.AddSingleton<ConsumerFactory<WebPageEventDto>>();

foreach (var c in builder.Configuration.AsEnumerable())
{
    Console.WriteLine(c.Key + " = " + c.Value);
}

var host = builder.Build();

host.Run();