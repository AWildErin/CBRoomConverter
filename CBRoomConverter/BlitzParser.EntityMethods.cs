using AWildErin.Utility;
using CBRoomConverter.Enums;
using CBRoomConverter.FunctionArguments;
using CBRoomConverter.Models;
using System.Text.RegularExpressions;

namespace CBRoomConverter;

internal partial class BlitzParser
{
	private static bool ParsePositionEntity( Room Room, PositionEntityFuncArgs FuncArgs )
	{
		// Generally, when we position objects for this room they will be in a format like:
		// - r\RoomDoors[1]\buttons[0]
		// - d\obj2
		//
		// Some considerations:
		// - we add an appending _num to entries that contain the same name. Such as 914, which reuses "d" for each door it spawns. 
		// - Sometimes we will only do d\obj, in which case we will need to see if it has multiple \. If it does, then we
		//		can assume that past the 2nd var is a child entity, and if it only has one then we can see if it room contains the full d\obj2, if not
		//		then we check to see if d exists, and if it does then we check it's subobjects

		// Args 0 = ObjName
		// Args 1 = PosX
		// Args 2 = PosY
		// Args 3 = PosZ


		var objectName = FuncArgs.entity;

		// Check to see if the object exists first
		Entity? foundEntity = Room.FindEntity( objectName );
		if ( foundEntity is null )
		{
			// Chances are it's a child entity, so split at \, and then use the text past the last occurance as the child name
			int lastIdx = objectName.LastIndexOf( '\\' );
			if ( lastIdx > -1 )
			{
				string childName = objectName.Substring( lastIdx + 1 );
				string parentName = objectName.Substring( 0, lastIdx );

				var parent = Room.FindEntity( parentName );
				if ( parent is null )
				{
					Log.Error( "failed to find parent entity '{childName}'" );
					return false;
				}

				foundEntity = parent.FindChildEntity( childName );
			}
		}

		if ( foundEntity is null )
		{
			Log.Error( $"Failed to find entity called '{objectName}'" );
			return false;
		}

		AddOrUpdateEntityPosition( foundEntity, FuncArgs.x, FuncArgs.y, FuncArgs.z );

		return true;
	}

	private static bool ParseCopyEntity( Room Room, CopyEntityFuncArgs FuncArgs )
	{
		//(var newEnt, var funcArgs) = CreateEntityFromFunction( Room, RegexMatch, ESCPCBRoomCreatorEntityType.None );
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
}
