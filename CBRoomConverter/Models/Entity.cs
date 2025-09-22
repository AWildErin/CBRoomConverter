using CBRoomConverter.Enums;
using System.Text.Json.Serialization;

namespace CBRoomConverter.Models;

internal class Entity
{
	public ESCPCBRoomCreatorEntityType Type { get; set; } = ESCPCBRoomCreatorEntityType.None;
	public string? Name { get; set; }
	public Dictionary<string, string> Properties { get; set; } = new();
	public List<Entity> ChildEntities { get; set; } = new();

	[JsonIgnore]
	public Room? OwnerRoom { get; set; }


	public Entity? FindChildEntity( string Name )
	{
		return ChildEntities.Where( x => x.Name == Name ).FirstOrDefault();
	}
}
