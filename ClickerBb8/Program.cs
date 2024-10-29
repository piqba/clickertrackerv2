using ClickerBb8;
using Share;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.Configure<KafkaOptions>(
    builder.Configuration.GetSection("Kafka"));
var host = builder.Build();


foreach (var c in builder.Configuration.AsEnumerable())
{
    Console.WriteLine(c.Key + " = " + c.Value);
}


host.Run();