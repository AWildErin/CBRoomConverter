using CBRoomConverter.Enums;
using CBRoomConverter.FunctionArguments;
using CBRoomConverter.Models;
using System.Text.RegularExpressions;

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

	// Extracts the raw numerical value from the given input
	private static string ExtractPosition( string Input )
	{
		string output = Input;

		if ( posNumberRegex.IsMatch( output ) )
		{
			// NOTE: This function handles whether or not we need to negate the value
			// In SCP:CB they will do stuff like r/x - val, which for actual usage, we need to then take the operator
			var match = posNumberRegex.Match( output );

			var op = match.Groups[1].Value.Trim();
			if ( op.Equals( "-" ) )
			{
				output = $"{op}{match.Groups[2].Value}";
			}
			else
			{
				output = match.Groups[2].Value;
			}
		}

		return output;
	}

	private static void AddOrUpdateEntityPosition( Entity Entity, string X, string Y, string Z )
	{
		if ( Entity.Properties.ContainsKey( "x" ) )
		{
			Entity.Properties["x"] = ExtractPosition( X );
		}
		else
		{
			Entity.Properties.Add( "x", ExtractPosition( X ) );
		}

		if ( Entity.Properties.ContainsKey( "y" ) )
		{
			Entity.Properties["y"] = ExtractPosition( Y );
		}
		else
		{
			Entity.Properties.Add( "y", ExtractPosition( Y ) );
		}

		if ( Entity.Properties.ContainsKey( "z" ) )
		{
			Entity.Properties["z"] = ExtractPosition( Z );
		}
		else
		{
			Entity.Properties.Add( "z", ExtractPosition( Z ) );
		}
	}

	private static bool CreateBasicEntity( Room Room, BaseFuncArgs FuncArgs, ESCPCBRoomCreatorEntityType EntityType )
	{
		var ent = CreateEntity( Room, FuncArgs.VariableName, EntityType );
		FuncArgs.AddPropertiesToEntity( ent );
		return true;
	}

	private static bool ParseCreateDoor( Room Room, CreateDoorFuncArgs FuncArgs )
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
		AddOrUpdateEntityPosition( ent, FuncArgs.x, FuncArgs.y, FuncArgs.z );

		return true;
	}

	private static bool ParseCreateItem( Room Room, CreateItemFuncArgs FuncArgs )
	{
		var ent = CreateEntity( Room, FuncArgs.VariableName, ESCPCBRoomCreatorEntityType.Item );

		FuncArgs.AddPropertiesToEntity( ent );
		AddOrUpdateEntityPosition( ent, FuncArgs.x, FuncArgs.y, FuncArgs.z );

		return true;
	}

	private static bool ParseCreateSecurityCam( Room Room, CreateSecurityCamFuncArgs FuncArgs )
	{
		var ent = CreateEntity( Room, FuncArgs.VariableName, ESCPCBRoomCreatorEntityType.SecurityCam );

		FuncArgs.AddPropertiesToEntity( ent );
		AddOrUpdateEntityPosition( ent, FuncArgs.x, FuncArgs.y, FuncArgs.z );

		return true;
	}

	private static bool ParseCreateButton( Room Room, CreateButtonFuncArgs FuncArgs )
	{
		var ent = CreateEntity( Room, FuncArgs.VariableName, ESCPCBRoomCreatorEntityType.Button );

		FuncArgs.AddPropertiesToEntity( ent );
		AddOrUpdateEntityPosition( ent, FuncArgs.x, FuncArgs.y, FuncArgs.z );

		return true;
	}

	private static bool ParseCreateWaypoint( Room Room, CreateWaypointFuncArgs FuncArgs )
	{
		var ent = CreateEntity( Room, FuncArgs.VariableName, ESCPCBRoomCreatorEntityType.Waypoint );

		FuncArgs.AddPropertiesToEntity( ent );
		AddOrUpdateEntityPosition( ent, FuncArgs.x, FuncArgs.y, FuncArgs.z );

		return true;
	}
}
