using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace CBRoomConverter.Converters;

/// <summary>
/// Converts quaternion to FRotator
/// </summary>
internal class QuaternionConverter : JsonConverter<Quaternion>
{
	public override Quaternion Read( ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options )
	{
		throw new NotImplementedException();
	}

	public override void Write( System.Text.Json.Utf8JsonWriter writer, Quaternion value, System.Text.Json.JsonSerializerOptions options )
	{
		writer.WriteStartObject();

		Vector3 angles = value.ToEulerAngles();
		writer.WriteNumber( "Pitch", angles.X * MathHelper.RadToDeg );
		writer.WriteNumber( "Yaw", angles.Y * MathHelper.RadToDeg );
		writer.WriteNumber( "Roll", angles.Z * MathHelper.RadToDeg );

		writer.WriteEndObject();
	}
}
