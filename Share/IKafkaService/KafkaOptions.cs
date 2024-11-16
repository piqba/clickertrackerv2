namespace Share.IKafkaService;

public class KafkaOptions
{
    public string TopicPrefix { get; set; } = String.Empty;
    public Dictionary<string, string> KafkaConfig { get; set; } = new Dictionary<string, string>();
}