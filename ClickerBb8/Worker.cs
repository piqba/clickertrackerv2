using System.Text.Json;
using ClickerBb8.Database;
using ClickerBb8.Service;
using Confluent.Kafka;
using Dapper;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Share;
using Share.dto;
using Share.Kafka;

namespace ClickerBb8;

public class Worker(
    ILogger<Worker> logger,
    KafkaService svc)
    : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await svc.RunAsync(stoppingToken);
        }
    }
}