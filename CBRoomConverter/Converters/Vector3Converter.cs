using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace CBRoomConverter.Converters;

internal class Vector3Converter : JsonConverter<Vector3>
{
	public override Vector3 Read( ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options )
	{
		throw new NotImplementedException();
	}

	public override void Write( System.Text.Json.Utf8JsonWriter writer, Vector3 value, System.Text.Json.JsonSerializerOptions options )
	{
		writer.WriteStartObject();
		writer.WriteNumber( "X", value.X );
		writer.WriteNumber( "Y", value.Y );
		writer.WriteNumber( "Z", value.Z );
		writer.WriteEndObject();
	}
}
