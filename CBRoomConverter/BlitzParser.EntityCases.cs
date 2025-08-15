using CBRoomConverter.Models;
using CBRoomConverter.Utility;
using System.Text.RegularExpressions;

namespace CBRoomConverter;

internal partial class BlitzParser
{
	private static (Entity, List<string>) CreateEntityFromFunction( Room Room, Match RegexMatch, string Type )
	{
		string varName = ExtractVarName( RegexMatch.Groups[1].Value ).Trim();

		// Get the args, trim whitespace, and remove quotes as we don't need them
		List<string> funcArgs = RegexMatch.Groups[3].Value
								.Split( "," )
								.Select( x =>
								{
									x = x.Trim();
									x = x.Trim( '"' );
									return x;
								} )
								.ToList();

		if ( Room.InternalNameIndex.ContainsKey( varName ) )
		{
			// Increase the name index
			Room.InternalNameIndex[varName]++;
		}
		else
		{
			Room.InternalNameIndex.Add( varName, 0 );
		}

		string? entName = null;
		if ( Room.InternalNameIndex[varName] > 0 )
		{
			entName = $"{varName}_{Room.InternalNameIndex[varName]}";
		}
		else
		{
			entName = varName;
		}

		var ent = new Entity()
		{
			Type = Type,
			Name = entName
		};


		Room.Entities.Add( ent );

		if ( Room.InternalNameToEntity.ContainsKey( varName ) )
		{
			Room.InternalNameToEntity[varName] = ent;
		}
		else
		{
			Room.InternalNameToEntity.Add( varName, ent );
		}

		return (ent, funcArgs);
	}

	// Extracts the raw numerical value from the given input
	private static string ExtractPosition( string Input )
	{
		string output = Input;

		if ( posNumberRegex.IsMatch( output ) )
		{
			output = posNumberRegex.Match( output ).Groups[1].Value;
		}

		return output;
	}

	private static void AddPositionToEntity( Entity Entity, string X, string Y, string Z )
	{
		Entity.Properties.Add( "x", ExtractPosition( X ) );
		Entity.Properties.Add( "y", ExtractPosition( Y ) );
		Entity.Properties.Add( "z", ExtractPosition( Z ) );
	}

	private static bool ParseCreateDoor( Room Room, Match RegexMatch, string Line )
	{
		(var ent, var funcArgs) = CreateEntityFromFunction( Room, RegexMatch, "door" );

		// Handle props
		ent.Properties.Add( "lvl", funcArgs[0] );

		AddPositionToEntity( ent, funcArgs[1], funcArgs[2], funcArgs[3] );

		ent.Properties.Add( "angle", funcArgs[4] );
		ent.Properties.Add( "room", funcArgs[5] );
		if ( funcArgs.Count() > 6 )
		{
			ent.Properties.Add( "dopen", funcArgs[6] );
		}

		if ( funcArgs.Count() > 7 )
		{
			ent.Properties.Add( "big", funcArgs[7] );
		}

		if ( funcArgs.Count() > 8 )
		{
			ent.Properties.Add( "keycard", funcArgs[8] );
		}

		if ( funcArgs.Count() > 9 )
		{
			ent.Properties.Add( "code", funcArgs[9] );
		}

		if ( funcArgs.Count() > 10 )
		{
			ent.Properties.Add( "usecollisionmodel", funcArgs[10] );
		}

		return true;
	}

	private static bool ParseLoadMeshStrict( Room Room, Match RegexMatch, string Line )
	{
		(var ent, var funcArgs) = CreateEntityFromFunction( Room, RegexMatch, "mesh" );

		ent.Properties.Add( "mesh", funcArgs[0] );

		if ( funcArgs.Count > 1 )
		{
			ent.Properties.Add( "parent", funcArgs[1] );
		}

		return true;
	}

	private static bool ParseCreateItem( Room Room, Match RegexMatch, string Line )
	{
		(var ent, var funcArgs) = CreateEntityFromFunction( Room, RegexMatch, "item" );

		ent.Properties.Add( "name", funcArgs[0] );
		ent.Properties.Add( "tempname", funcArgs[1] );

		AddPositionToEntity( ent, funcArgs[2], funcArgs[3], funcArgs[4] );

		if ( funcArgs.Count > 5 )
		{
			ent.Properties.Add( "r", funcArgs[5] );
		}

		if ( funcArgs.Count > 6 )
		{
			ent.Properties.Add( "g", funcArgs[6] );
		}

		if ( funcArgs.Count > 7 )
		{
			ent.Properties.Add( "b", funcArgs[7] );
		}

		if ( funcArgs.Count > 8 )
		{
			ent.Properties.Add( "a", funcArgs[8] );
		}

		if ( funcArgs.Count > 9 )
		{
			ent.Properties.Add( "invslots", funcArgs[9] );
		}

		return true;
	}

	private static bool ParseCreatePivot( Room Room, Match RegexMatch, string Line )
	{
		(var ent, var funcArgs) = CreateEntityFromFunction( Room, RegexMatch, "pivot" );

		if ( funcArgs.Count > 0 )
		{
			ent.Properties.Add( "parent", funcArgs[0] );
		}

		return true;
	}

	private static bool ParseCopyEntity( Room Room, Match RegexMatch, string Line )
	{
		(var newEnt, var funcArgs) = CreateEntityFromFunction( Room, RegexMatch, "FILL_ME" );

		var entName = funcArgs[0];

		var origEnt = Room.InternalNameToEntity.GetValueOrDefault( entName );
		if ( origEnt is null )
		{
			Room.Entities.Remove( newEnt );
			return false;
		}

		//Log.Info( $"Copying {entName} inside {Room.Name}" );

		newEnt.Type = origEnt.Type;
		newEnt.Properties = new( origEnt.Properties );

		if ( funcArgs.Count > 1 )
		{
			var parent = funcArgs[1];
			if ( newEnt.Properties.ContainsKey( "parent" ) )
			{
				newEnt.Properties["parent"] = parent;
			}
			else
			{
				newEnt.Properties.Add( "parent", parent );
			}
		}


		return true;
	}

	private static bool ParseCreateSecurityCam( Room Room, Match RegexMatch, string Line )
	{
		(var ent, var funcArgs) = CreateEntityFromFunction( Room, RegexMatch, "securitycam" );

		AddPositionToEntity( ent, funcArgs[0], funcArgs[1], funcArgs[2] );

		ent.Properties.Add( "room", funcArgs[3] );

		if ( funcArgs.Count > 4 )
		{
			ent.Properties.Add( "screen", funcArgs[4] );
		}

		return true;
	}

	private static bool ParseCreateButton( Room Room, Match RegexMatch, string Line )
	{
		(var ent, var funcArgs) = CreateEntityFromFunction( Room, RegexMatch, "button" );

		AddPositionToEntity( ent, funcArgs[0], funcArgs[1], funcArgs[2] );

		ent.Properties.Add( "pitch", funcArgs[3] );
		ent.Properties.Add( "yaw", funcArgs[4] );

		if ( funcArgs.Count > 5 )
		{
			ent.Properties.Add( "roll", funcArgs[5] );
		}

		return true;
	}

	private static bool ParseCreateWaypoint( Room Room, Match RegexMatch, string Line )
	{
		(var ent, var funcArgs) = CreateEntityFromFunction( Room, RegexMatch, "waypoint" );

		AddPositionToEntity( ent, funcArgs[0], funcArgs[1], funcArgs[2] );

		ent.Properties.Add( "door", funcArgs[3] );
		ent.Properties.Add( "room", funcArgs[4] );

		return true;
	}
}
