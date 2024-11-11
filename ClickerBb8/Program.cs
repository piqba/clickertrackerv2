using ClickerBb8;
using ClickerBb8.Database;
using Share;


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

foreach (var c in builder.Configuration.AsEnumerable())
{
    Console.WriteLine(c.Key + " = " + c.Value);
}


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext,services )=>
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true,
                reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        services.Configure<KafkaOptions>(
            configuration.GetSection("Kafka"));
        services.AddSingleton<IDBConnectionFactory>(_ =>
            new NpgsqlDbConnectionFactory(builder.Configuration["Database:ConnectionString"]!));;
    })
    .Build();


host.Run();