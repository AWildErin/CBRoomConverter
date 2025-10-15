using CBRoomConverter.Enums;
using CBRoomConverter.FunctionArguments;
using CBRoomConverter.Models;

namespace CBRoomConverter;

internal partial class BlitzParser
{
	private static List<string> ExtractArgsFromString( string Args )
	{
		// Get the args, trim whitespace, and remove quotes as we don't need them
		List<string> funcArgs = Args
				.Split( "," )
				.Select( x =>
				{
					x = x.Trim();
					x = x.Trim( '"' );
					return x;
				} )
				.ToList();

		return funcArgs;
	}

	private static Entity CreateEntity( Room Room, string EntityName, ESCPCBRoomCreatorEntityType EntityType )
	{
		if ( Room.InternalNameIndex.ContainsKey( EntityName ) )
		{
			// Increase the name index
			Room.InternalNameIndex[EntityName]++;
		}
		else
		{
			Room.InternalNameIndex.Add( EntityName, 0 );
		}

		string? entName = null;
		if ( Room.InternalNameIndex[EntityName] > 0 )
		{
			entName = $"{EntityName}_{Room.InternalNameIndex[EntityName]}";
		}
		else
		{
			entName = EntityName;
		}

		var ent = new Entity()
		{
			Type = EntityType,
			Name = entName
		};

		Room.Entities.Add( ent );

		if ( Room.InternalNameToEntity.ContainsKey( EntityName ) )
		{
			Room.InternalNameToEntity[EntityName] = ent;
		}
		else
		{
			Room.InternalNameToEntity.Add( EntityName, ent );
		}

		return ent;
	}

	private static bool CreateBasicEntity( Room Room, BaseFuncArgs FuncArgs, ESCPCBRoomCreatorEntityType EntityType )
	{
		var ent = CreateEntity( Room, FuncArgs.VariableName, EntityType );
		FuncArgs.AddPropertiesToEntity( ent );
		return true;
	}

	private static bool CreateDoor( Room Room, CreateDoorFuncArgs FuncArgs )
	{
		var ent = CreateEntity( Room, FuncArgs.VariableName, ESCPCBRoomCreatorEntityType.Door );

		// Add child buttons
		var button = new Entity();
		button.Name = "buttons[0]";
		ent.ChildEntities.Add( button );

		button = new Entity();
		button.Name = "buttons[1]";
		ent.ChildEntities.Add( button );

		FuncArgs.AddPropertiesToEntity( ent );
		ent.SetPosition( FuncArgs.x, FuncArgs.y, FuncArgs.z );

		return true;
	}

	private static bool CreateItem( Room Room, CreateItemFuncArgs FuncArgs )
	{
		var ent = CreateEntity( Room, FuncArgs.VariableName, ESCPCBRoomCreatorEntityType.Item );

		FuncArgs.AddPropertiesToEntity( ent );
		ent.SetPosition( FuncArgs.x, FuncArgs.y, FuncArgs.z );

		return true;
	}

	private static bool CreateSecurityCam( Room Room, CreateSecurityCamFuncArgs FuncArgs )
	{
		var ent = CreateEntity( Room, FuncArgs.VariableName, ESCPCBRoomCreatorEntityType.SecurityCam );

		FuncArgs.AddPropertiesToEntity( ent );
		ent.SetPosition( FuncArgs.x, FuncArgs.y, FuncArgs.z );

		return true;
	}

	private static bool CreateButton( Room Room, CreateButtonFuncArgs FuncArgs )
	{
		var ent = CreateEntity( Room, FuncArgs.VariableName, ESCPCBRoomCreatorEntityType.Button );

		FuncArgs.AddPropertiesToEntity( ent );
		ent.SetPosition( FuncArgs.x, FuncArgs.y, FuncArgs.z );

		return true;
	}

	private static bool CreateWaypoint( Room Room, CreateWaypointFuncArgs FuncArgs )
	{
		var ent = CreateEntity( Room, FuncArgs.VariableName, ESCPCBRoomCreatorEntityType.Waypoint );

		FuncArgs.AddPropertiesToEntity( ent );
		ent.SetPosition( FuncArgs.x, FuncArgs.y, FuncArgs.z );

		return true;
	}
}
