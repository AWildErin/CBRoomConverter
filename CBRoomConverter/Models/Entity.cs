using CBRoomConverter.Enums;
using CBRoomConverter.Helpers;
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

	private void SetOrUpdatePositionComponent( string Component, string NewValue )
	{
		if ( Properties.ContainsKey( Component ) )
		{
			Properties[Component] = EntityHelpers.ExtractPosition( NewValue );
		}
		else
		{
			Properties.Add( Component, EntityHelpers.ExtractPosition( NewValue ) );
		}
	}

	private float GetPositionComponentAsNumber( string Component )
	{
		if ( Properties.ContainsKey( Component ) )
		{
			return float.Parse( Properties[Component] );
		}

		return 0f;
	}

	public void SetPosition( string X, string Y, string Z )
	{
		SetOrUpdatePositionComponent( "x", X );
		SetOrUpdatePositionComponent( "y", Y );
		SetOrUpdatePositionComponent( "z", Z );
	}
}
