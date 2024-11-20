using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Share.Otel;

public static class ClicksMetricsCustoms
{
    // Custom metrics for the application
    public static readonly Meter ClickCounterMeter = new Meter("Clicker.Clicks.Counter", "1.0.0");
    public static readonly Counter<int> CountClicks = ClickCounterMeter.CreateCounter<int>("clicks.count", description: "Counts the number of clicks");
    // Custom ActivitySource for the application
    public static readonly ActivitySource ClicksTrackerActivitySource = new ActivitySource("Clicker.Clicks.Tracker");
}