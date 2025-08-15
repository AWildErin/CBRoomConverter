using IniParser.Model.Formatting;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace CBRoomConverter.Models;

internal class Entity
{
	public string? Type { get; set; }
	public string? Name { get; set; }
	public Dictionary<string, string> Properties { get; set; } = new();

	[JsonIgnore]
	public Room? OwnerRoom { get; set; }

	// Inefficient and lazy but gets the job done
	// Took this suggestion from a reddit post
	//public Entity Copy()
	//{
	//	var ent = JsonSerializer.Deserialize<Entity>( JsonSerializer.Serialize( this ) );
	//	if (ent is null)
	//	{
	//		throw new Exception( $"Failed to copy {Name}" );
	//	}

	//	return ent;
	//}
}
