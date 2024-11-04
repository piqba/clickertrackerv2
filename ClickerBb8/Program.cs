using ClickerBb8;
using ClickerBb8.Database;
using Share;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.Configure<KafkaOptions>(
    builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton<IDBConnectionFactory>(_ =>
    new NpgsqlDbConnectionFactory(builder.Configuration["Database:ConnectionString"]!));


var host = builder.Build();


foreach (var c in builder.Configuration.AsEnumerable())
{
    Console.WriteLine(c.Key + " = " + c.Value);
}


host.Run();