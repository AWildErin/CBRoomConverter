using CBRoomConverter.Helpers;

namespace CBRoomConverter.Models;

/// <summary>
/// Partial class that holds all the string based positional/rotational functions, this allows us to directly call from Blitz3D
/// and handle aother aspects like EntityX, EntityY, EntityPitch, EntityYaw etc.
/// </summary>
internal partial class Entity
{
	public void SetAngles( string Pitch, string Yaw, string Roll )
	{
		float x = EntityHelpers.EvaluateExpression( Pitch );
		float y = EntityHelpers.EvaluateExpression( Yaw );
		float z = EntityHelpers.EvaluateExpression( Roll );

		SetAngles( x, y, z );
	}

	public void SetPosition( string X, string Y, string Z )
	{
		float x = EntityHelpers.EvaluateExpression( X );
		float y = EntityHelpers.EvaluateExpression( Y );
		float z = EntityHelpers.EvaluateExpression( Z );

		SetPosition( x, y, z );
	}

	public void SetScale( string X, string Y, string Z )
	{
		float x = EntityHelpers.EvaluateExpression( X );
		float y = EntityHelpers.EvaluateExpression( Y );
		float z = EntityHelpers.EvaluateExpression( Z );

		SetScale( x, y, z );
	}

	public void MoveEntity( string X, string Y, string Z )
	{
		float x = EntityHelpers.EvaluateExpression( X );
		float y = EntityHelpers.EvaluateExpression( Y );
		float z = EntityHelpers.EvaluateExpression( Z );

		MoveEntity( x, y, z );
	}

	public void TranslateEntity( string X, string Y, string Z )
	{
		float x = EntityHelpers.EvaluateExpression( X );
		float y = EntityHelpers.EvaluateExpression( Y );
		float z = EntityHelpers.EvaluateExpression( Z );

		TranslateEntity( x, y, z );
	}

	public void TurnEntity( string Pitch, string Yaw, string Roll )
	{
		float x = EntityHelpers.EvaluateExpression( Pitch );
		float y = EntityHelpers.EvaluateExpression( Yaw );
		float z = EntityHelpers.EvaluateExpression( Roll );

		TurnEntity( x, y, z );
	}
}
