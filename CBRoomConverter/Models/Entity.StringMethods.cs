using CBRoomConverter.Helpers;
using OpenTK.Mathematics;

namespace CBRoomConverter.Models;

/// <summary>
/// Partial class that holds all the string based positional/rotational functions, this allows us to directly call from Blitz3D
/// and handle aother aspects like EntityX, EntityY, EntityPitch, EntityYaw etc.
/// </summary>
internal partial class Entity
{
	public void SetPosition( string X, string Y, string Z )
	{
		SetOrUpdatePositionComponent( "x", X );
		SetOrUpdatePositionComponent( "y", Y );
		SetOrUpdatePositionComponent( "z", Z );

		float x = float.Parse( EntityHelpers.ExtractPosition( X ) );
		float y = float.Parse( EntityHelpers.ExtractPosition( Y ) );
		float z = float.Parse( EntityHelpers.ExtractPosition( Z ) );

		SetPosition( x, y, z );
	}

	/// <summary>
	/// Moves the entity relative to it's current position
	/// </summary>
	public void MoveEntity( string X, string Y, string Z )
	{
		float x = float.Parse( EntityHelpers.ExtractPosition( X ) );
		float y = float.Parse( EntityHelpers.ExtractPosition( Y ) );
		float z = float.Parse( EntityHelpers.ExtractPosition( Z ) );

		MoveEntity( x, y, z );
	}
}
