using System.Collections.Concurrent;
using System.Text.Json;
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

app.MapPost("/clicks", async (WebPageEventDto webPageEventDto, IOptions<KafkaOptions> opts) =>
{
    var jsonString = JsonSerializer.Serialize(webPageEventDto);
    var cfg = opts.Value;
    using (var p = new ProducerBuilder<Null, string>(new ProducerConfig(opts.Value.KafkaConfig)).Build())
    {
        try
        {
            var dr = await p.ProduceAsync(opts.Value.TopicPrefix, new Message<Null, string> { Value = jsonString });
            Console.WriteLine(
                $"Delivered offset: '{dr.Offset}' to '{dr.TopicPartitionOffset}' ts: '{dr.Timestamp.UnixTimestampMs}'");
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        }
    }

    // TODO: check the status code response
    return Results.Json(null, statusCode: StatusCodes.Status202Accepted);
});

app.MapGet("/clicks", async (HttpContext ctx, IOptions<KafkaOptions> opts, CancellationToken ct) =>
{
    ctx.Response.Headers.Append("Content-Type", "text/event-stream");

    using (var consumer = new ConsumerBuilder<Ignore, string>(new ConsumerConfig(opts.Value.KafkaConfig))
               // Note: All handlers are called on the main .Consume thread.
               .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
               .SetStatisticsHandler((_, json) => Console.WriteLine($"Statistics: {json}"))
               .SetPartitionsAssignedHandler((c, partitions) =>
               {
                   // Since a cooperative assignor (CooperativeSticky) has been configured, the
                   // partition assignment is incremental (adds partitions to any existing assignment).
                   Console.WriteLine(
                       "Partitions incrementally assigned: [" +
                       string.Join(',', partitions.Select(p => p.Partition.Value)) +
                       "], all: [" +
                       string.Join(',', c.Assignment.Concat(partitions).Select(p => p.Partition.Value)) +
                       "]");

                   // Possibly manually specify start offsets by returning a list of topic/partition/offsets
                   // to assign to, e.g.:
                   // return partitions.Select(tp => new TopicPartitionOffset(tp, externalOffsets[tp]));
               })
               .SetPartitionsRevokedHandler((c, partitions) =>
               {
                   // Since a cooperative assignor (CooperativeSticky) has been configured, the revoked
                   // assignment is incremental (may remove only some partitions of the current assignment).
                   var remaining = c.Assignment.Where(atp =>
                       partitions.Where(rtp => rtp.TopicPartition == atp).Count() == 0);
                   Console.WriteLine(
                       "Partitions incrementally revoked: [" +
                       string.Join(',', partitions.Select(p => p.Partition.Value)) +
                       "], remaining: [" +
                       string.Join(',', remaining.Select(p => p.Partition.Value)) +
                       "]");
               })
               .SetPartitionsLostHandler((c, partitions) =>
               {
                   // The lost partitions handler is called when the consumer detects that it has lost ownership
                   // of its assignment (fallen out of the group).
                   Console.WriteLine($"Partitions were lost: [{string.Join(", ", partitions)}]");
               })
               .Build())

    {
        consumer.Subscribe(opts.Value.TopicPrefix);

        try
        {
            while (true)
            {
                try
                {
                    var consumeResult = consumer.Consume(ct);

                    if (consumeResult.IsPartitionEOF)
                    {
                        Console.WriteLine(
                            $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");

                        continue;
                    }

                    Console.WriteLine(
                        $"Received message at {consumeResult.TopicPartitionOffset}: {consumeResult.Message.Value}");
                    await ctx.Response.WriteAsync($"data: ");
                    await JsonSerializer.SerializeAsync(ctx.Response.Body, consumeResult.Message.Value);
                    await ctx.Response.WriteAsync($"\n\n");
                    await ctx.Response.Body.FlushAsync();
                    try
                    {
                        // Store the offset associated with consumeResult to a local cache. Stored offsets are committed to Kafka by a background thread every AutoCommitIntervalMs. 
                        // The offset stored is actually the offset of the consumeResult + 1 since by convention, committed offsets specify the next message to consume. 
                        // If EnableAutoOffsetStore had been set to the default value true, the .NET client would automatically store offsets immediately prior to delivering messages to the application. 
                        // Explicitly storing offsets after processing gives at-least once semantics, the default behavior does not.
                        consumer.StoreOffset(consumeResult);
                    }
                    catch (KafkaException e)
                    {
                        Console.WriteLine($"Store Offset error: {e.Error.Reason}");
                    }
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Consume error: {e.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Closing consumer.");
            consumer.Close();
        }
    }
});
app.Run();