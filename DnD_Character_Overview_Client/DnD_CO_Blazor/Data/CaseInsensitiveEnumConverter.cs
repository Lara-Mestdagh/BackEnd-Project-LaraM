using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DnD_CO_Blazor.Data;

public class CaseInsensitiveEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var enumString = reader.GetString();
            if (Enum.TryParse(enumString, true, out T value))
            {
                return value;
            }
            else
            {
                throw new JsonException($"Unable to convert \"{enumString}\" to Enum \"{typeof(T)}\"");
            }
        }

        throw new JsonException($"Unexpected token {reader.TokenType} when parsing an enum.");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

public class CaseInsensitiveEnumConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(CaseInsensitiveEnumConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType);
    }
}