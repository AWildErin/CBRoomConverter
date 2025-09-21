using CBRoomConverter.Enums;
using System.Text.Json.Serialization;

namespace CBRoomConverter.Models;

internal class Entity
{
	public ESCPCBRoomCreatorEntityType Type { get; set; } = ESCPCBRoomCreatorEntityType.None;
	public string? Name { get; set; }
	public Dictionary<string, string> Properties { get; set; } = new();
	public Dictionary<string, Entity> ChildEntities { get; set; } = new();

	[JsonIgnore]
	public Room? OwnerRoom { get; set; }
}
