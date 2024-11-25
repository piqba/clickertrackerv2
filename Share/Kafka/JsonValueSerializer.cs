using System.Text.Json;
using Confluent.Kafka;

namespace Share.Kafka;

public class JsonValueSerializer<T> : ISerializer<T>, IDeserializer<T>
{
    readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public byte[] Serialize(T data, SerializationContext context)
    {
        using var ms = new MemoryStream();
        var jsonString = JsonSerializer.Serialize(data, _options);
        var writer = new StreamWriter(ms);

        writer.Write(jsonString);
        writer.Flush();
        ms.Position = 0;

        return ms.ToArray();
    }

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
       
        using var stream = new MemoryStream(data.ToArray());
        return (T)(JsonSerializer.Deserialize<T>(stream, _options) ?? new object());
    }
}