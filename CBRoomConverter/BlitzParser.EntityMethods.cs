using AWildErin.Utility;
using CBRoomConverter.Enums;
using CBRoomConverter.FunctionArguments;
using CBRoomConverter.Models;

namespace CBRoomConverter;

internal partial class BlitzParser
{
	private static bool CopyEntity( Room Room, CopyEntityFuncArgs FuncArgs )
	{
		var newEnt = CreateEntity( Room, FuncArgs.VariableName, ESCPCBRoomCreatorEntityType.None );

		var origEnt = Room.InternalNameToEntity.GetValueOrDefault( newEnt.Name! );
		if ( origEnt is null )
		{
			Room.Entities.Remove( newEnt );
			return false;
		}

		//Log.Info( $"Copying {entName} inside {Room.Name}" );

		newEnt.Type = origEnt.Type;
		newEnt.Properties = new( origEnt.Properties );

		if ( FuncArgs.parent is not null )
		{

			if ( newEnt.Properties.ContainsKey( "parent" ) )
			{
				newEnt.Properties["parent"] = FuncArgs.parent;
			}
			else
			{
				newEnt.Properties.Add( "parent", FuncArgs.parent );
			}
		}

		return true;
	}

	private static bool ScaleEntity( Room Room, ScaleEntityFuncArgs FuncArgs )
	{
		var objectName = FuncArgs.entity;

		// Check to see if the object exists first
		Entity? foundEntity = Room.FindEntity( objectName, true );
		if ( foundEntity is null )
		{
			Log.Error( $"Failed to find entity called '{objectName}'" );
			return false;
		}

		foundEntity.SetScale( FuncArgs.x_scale, FuncArgs.y_scale, FuncArgs.z_scale );

		return true;
	}

	#region Entity Position

	// Generally, when we position objects for this room they will be in a format like:
	// - r\RoomDoors[1]\buttons[0]
	// - d\obj2
	//
	// Some considerations:
	// - we add an appending _num to entries that contain the same name. Such as 914, which reuses "d" for each door it spawns. 
	// - Sometimes we will only do d\obj, in which case we will need to see if it has multiple \. If it does, then we
	//		can assume that past the 2nd var is a child entity, and if it only has one then we can see if it room contains the full d\obj2, if not
	//		then we check to see if d exists, and if it does then we check it's subobjects


	private static bool PositionEntity( Room Room, PositionEntityFuncArgs FuncArgs )
	{
		var objectName = FuncArgs.entity;

		// Check to see if the object exists first
		Entity? foundEntity = Room.FindEntity( objectName, true );
		if ( foundEntity is null )
		{
			Log.Error( $"Failed to find entity called '{objectName}'" );
			return false;
		}

		foundEntity.SetPosition( FuncArgs.x, FuncArgs.y, FuncArgs.z );

		return true;
	}

	private static bool MoveEntity( Room Room, MoveEntityFuncArgs FuncArgs )
	{
		var objectName = FuncArgs.entity;

		// Check to see if the object exists first
		Entity? foundEntity = Room.FindEntity( objectName, true );
		if ( foundEntity is null )
		{
			Log.Error( $"Failed to find entity called '{objectName}'" );
			return false;
		}

		foundEntity.MoveEntity( FuncArgs.x, FuncArgs.y, FuncArgs.z );

		return true;
	}

	private static bool TranslateEntity( Room Room, TranslateEntityFuncArgs FuncArgs )
	{
		var objectName = FuncArgs.entity;

		// Check to see if the object exists first
		Entity? foundEntity = Room.FindEntity( objectName, true );
		if ( foundEntity is null )
		{
			Log.Error( $"Failed to find entity called '{objectName}'" );
			return false;
		}

		foundEntity.TranslateEntity( FuncArgs.x, FuncArgs.y, FuncArgs.z );

		return true;
	}

	#endregion

	#region Entity Rotation

	private static bool RotateEntity( Room Room, RotateEntityFuncArgs FuncArgs )
	{
		var objectName = FuncArgs.entity;

		// Check to see if the object exists first
		Entity? foundEntity = Room.FindEntity( objectName, true );
		if ( foundEntity is null )
		{
			Log.Error( $"Failed to find entity called '{objectName}'" );
			return false;
		}

		foundEntity.SetAngles( FuncArgs.pitch, FuncArgs.yaw, FuncArgs.roll );

		return true;
	}

	private static bool TurnEntity( Room Room, TurnEntityFuncArgs FuncArgs )
	{
		var objectName = FuncArgs.entity;

		// Check to see if the object exists first
		Entity? foundEntity = Room.FindEntity( objectName, true );
		if ( foundEntity is null )
		{
			Log.Error( $"Failed to find entity called '{objectName}'" );
			return false;
		}

		foundEntity.TurnEntity( FuncArgs.pitch, FuncArgs.yaw, FuncArgs.roll );

		return true;
	}

	#endregion
}
