using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13;

public class DateTimeConverter : JsonConverter<DateTime>
{

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
{
    if (reader.TokenType != JsonTokenType.String)
    {
        throw new JsonException($"Для передачи даты был использован неверный тип данных!");
    }

    string dateStr = reader.GetString();

    if (string.IsNullOrWhiteSpace(dateStr))
    {
        throw new JsonException("Некорректная дата!");
    }
    return DateTime.ParseExact(dateStr, "yyyy-MM-dd", null);
}

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
    }
}