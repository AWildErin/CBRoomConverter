using AWildErin.Utility;
using CBRoomConverter.Enums;
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
	public HashSet<int> ChildEntityIndexes { get; set; } = new();

	// here so we can directly check to see if it's a child in Unreal
	public int ParentIndex { get; set; } = -1;

	[JsonIgnore]
	public Room? OwnerRoom { get; set; }

	[JsonIgnore]
	public Entity? Parent { get; set; }

	public Entity? FindChildEntity( string Name )
	{
		// Children can only exist in the same room
		if ( OwnerRoom is null )
		{
			return null;
		}

		foreach ( var index in ChildEntityIndexes )
		{
			Entity? entity = OwnerRoom.Entities.ElementAtOrDefault( index );

			// Check to see if our entity matches what we want
			if ( entity is not null && entity.Name is not null && entity.Name.Equals( Name ) )
			{
				return entity;
			}
		}

		// Didn't find any entities
		return null;
	}

	public void SetParent( Entity? NewParent )
	{
		// We're already parented to this entity, so just early return
		if ( NewParent == Parent )
		{
			return;
		}

		if ( OwnerRoom is null )
		{
			Log.Error( $"Cannot set parent on {Name}, owner room null" );
			return;
		}

		int entityIndex = OwnerRoom.GetEntityIndex( this );
		if ( entityIndex == -1 )
		{
			Log.Error( $"Cannot set parent on {Name}, failed to get entity index." );
			return;
		}

		// Remove our parent association
		if ( NewParent is null )
		{
			if ( Parent is not null )
			{
				Parent.ChildEntityIndexes.Remove( entityIndex );
			}

			Parent = null;
			ParentIndex = -1;

			return;
		}

		// Remove ourselves from our current parent
		if ( Parent is not null )
		{
			Parent.ChildEntityIndexes.Remove( entityIndex );
		}

		Parent = NewParent;
		Parent.ChildEntityIndexes.Add( entityIndex );
		ParentIndex = OwnerRoom.GetEntityIndex( Parent );
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
