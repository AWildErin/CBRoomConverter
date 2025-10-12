using CBRoomConverter.Enums;
using CBRoomConverter.Helpers;
using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace CBRoomConverter.Models;

internal partial class Entity
{
	public ESCPCBRoomCreatorEntityType Type { get; set; } = ESCPCBRoomCreatorEntityType.None;
	public string? Name { get; set; }

	public Vector3 Scale { get; set; } = new( 1f );
	public Vector3 Position { get; set; } = new( 0f, 0f, 0f );
	public Quaternion Rotation { get; set; } = new( 0f, 0f, 0f, 1f );

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

	// bb func: RotateEntity
	public void SetAngles( float Pitch, float Yaw, float Roll )
	{
		Rotation = new( Pitch * MathHelper.DegToRad, Yaw * MathHelper.DegToRad, Roll * MathHelper.DegToRad );
	}

	// bb func: PositionEntity
	public void SetPosition( float X, float Y, float Z )
	{
		Position = new( X, Y, Z );
	}

	// bb func: ScaleEntity
	public void SetScale( float X, float Y, float Z )
	{
		Scale = new( X, Y, Z );
	}

	// Positional Methods

	// Moves relative to it's position and orientation
	public void MoveEntity( float X, float Y, float Z )
	{
		Position += Vector3.Transform( new Vector3( X, Y, Z ), Rotation );
	}

	// Moves relative to it's position and no it's orientation
	public void TranslateEntity( float X, float Y, float Z )
	{
		Position += new Vector3( X, Y, Z );
	}

	// Rotational Methods

	// Turns an entity relative to it's current orientation
	public void TurnEntity( float Pitch, float Yaw, float Roll )
	{
		Quaternion delta = new( Pitch * MathHelper.DegToRad, Yaw * MathHelper.DegToRad, Roll * MathHelper.DegToRad );
		Rotation *= delta;
	}

}
