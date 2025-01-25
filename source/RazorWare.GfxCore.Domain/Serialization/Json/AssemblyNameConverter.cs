using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RazorWare.GfxCore.Serialization.Json;

#nullable disable
public class AssemblyNameConverter : JsonConverter<AssemblyName>
{
    public override AssemblyName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string assemblyNameString = reader.GetString();
            return new(assemblyNameString);
        }

        throw new JsonException("Expected string token for AssemblyName.");
    }

    public override void Write(Utf8JsonWriter writer, AssemblyName value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.FullName);
    }
}
