using CBRoomConverter.Helpers;
using OpenTK.Mathematics;

namespace CBRoomConverter.Models;

/// <summary>
/// Partial class that holds all the string based positional/rotational functions, this allows us to directly call from Blitz3D
/// and handle aother aspects like EntityX, EntityY, EntityPitch, EntityYaw etc.
/// </summary>
internal partial class Entity
{
	public void SetAngles( string Pitch, string Yaw, string Roll )
	{
		float x = float.Parse( EntityHelpers.ExtractRotation( Pitch ) );
		float y = float.Parse( EntityHelpers.ExtractRotation( Yaw ) );
		float z = float.Parse( EntityHelpers.ExtractRotation( Roll ) );

		SetAngles( x, y, z );
	}

	public void SetPosition( string X, string Y, string Z )
	{
		float x = float.Parse( EntityHelpers.ExtractPosition( X ) );
		float y = float.Parse( EntityHelpers.ExtractPosition( Y ) );
		float z = float.Parse( EntityHelpers.ExtractPosition( Z ) );

		SetPosition( x, y, z );
	}

	public void SetScale( string X, string Y, string Z )
	{
		float x = float.Parse( X );
		float y = float.Parse( Y );
		float z = float.Parse( Z );

		SetScale( x, y, z );
	}

	public void MoveEntity( string X, string Y, string Z )
	{
		float x = float.Parse( EntityHelpers.ExtractPosition( X ) );
		float y = float.Parse( EntityHelpers.ExtractPosition( Y ) );
		float z = float.Parse( EntityHelpers.ExtractPosition( Z ) );

		MoveEntity( x, y, z );
	}

	public void TranslateEntity( string X, string Y, string Z )
	{
		float x = float.Parse( EntityHelpers.ExtractPosition( X ) );
		float y = float.Parse( EntityHelpers.ExtractPosition( Y ) );
		float z = float.Parse( EntityHelpers.ExtractPosition( Z ) );

		TranslateEntity( x, y, z );
	}

	public void TurnEntity( string Pitch, string Yaw, string Roll )
	{
		float x = float.Parse( EntityHelpers.ExtractRotation( Pitch ) );
		float y = float.Parse( EntityHelpers.ExtractRotation( Yaw ) );
		float z = float.Parse( EntityHelpers.ExtractRotation( Roll ) );

		TurnEntity( x, y, z );
	}
}
