using AWildErin.Utility;
using CBRoomConverter.Models;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace CBRoomConverter;

internal partial class BlitzParser
{
	/**
	 * Parse the following
	 * Valid commands to parse:
	 * LoadMesh_Strict
	 * PositionEntity
	 * RotateEntity
	 * TurnEntity
	 * ScaleEntity
	 * EntityTexture
	 * EntityParent
	 * EntityType (?)
	 * CreateDrawPortal
	 * CreateDoor
	 * CreateItem
	 * CreateSecurityCamera
	 * CreateEmitter
	 * CreateDevilEmitter
	 * CreatePivot
	 * CreateSprite
	*/

	private const string fillRoomFunction = "Function FillRoom(r.Rooms)";
	private const string selectCase = "Select r\\RoomTemplate\\Name";
	private const string commentSymbol = ";";
	private const string beginBlock = ";[Block]";
	private const string endBlock = ";[End Block]";

	private static readonly string[] allowedLines = [
		beginBlock,
		endBlock
	];

	private static readonly string[] skippedFunctions = [
		// Most decals are assigned with some means of randomisation, so it's not idea for us to do it that way.
		"CreateDecal",

		// Most sprites are pretty useless to us
		"CreateSprite",

		// Ditto above
		"CreateEmitter",

		// We can't really use textures
		"LoadTexture_Strict",
		"LoadAnimTexture"
	];

	// assignment regex
	// (.*)[\s]*=[\s]*(.*)

	private static readonly Regex caseRegex = new( "Case \"(.*)\"" );
	private static readonly Regex funcWithReturnRegex = new( @"(.*?)\s*=\s*(.*?)\((.*)\)" );
	private static readonly Regex objectAssignmentRegex = new( @".*?[\\](.*?) =" );
	private static readonly Regex posNumberRegex = new( @"([+\-*/]\s*|\b)(\d+(?:\.\d+)?)" );

	public static void ParseBlitz( RoomList List, string BlitzPath, Options Opts )
	{
		int functionStartIndex = -1;
		int functionEndIndex = -1;
		bool insideFillRoom = false;

		string? currentCase = null;

		// Script text for each room case
		Dictionary<string, List<string>> roomCaseText = new();
		Dictionary<string, string> keysWithSameCase = new();

		if ( Opts.Verbose )
		{
			Log.Debug( $"Parsing blitz file: {BlitzPath}" );
		}

		var lines = File.ReadAllLines( BlitzPath );
		for ( int i = 0; i < lines.Length; i++ )
		{
			string line = lines[i].Trim();

			bool isEmpty = string.IsNullOrEmpty( line );

			if ( isEmpty || (line.StartsWith( commentSymbol ) && !allowedLines.Contains( line )) )
			{
				//Log.Info( $"Skipping {line}" );
				continue;
			}

			if ( line.Equals( fillRoomFunction ) )
			{
				functionStartIndex = i;
				insideFillRoom = true;
				continue;
			}

			if ( line.Equals( "End Function" ) && insideFillRoom )
			{
				functionEndIndex = i;
				insideFillRoom = false;
				break;
			}

			if ( !insideFillRoom )
			{
				continue;
			}

			if ( line.StartsWith( "Case \"" ) )
			{
				var match = caseRegex.Match( line );
				currentCase = match.Groups[1].Value;

				// if we contain any comma, this means we need to parse it a bit differently.
				if ( currentCase.Contains( "," ) )
				{
					var allCases = currentCase
						.Split( ',' )
						.Select( x => x.Trim().Trim( '"' ) )
						.ToList();

					// Because this case contains multiple keys,
					// we need to make sure the case text is the same
					List<string> scriptText = new();

					foreach ( var key in allCases )
					{
						roomCaseText.Add( key, scriptText );
					}

					currentCase = allCases[0];
					continue;
				}

				roomCaseText.Add( currentCase, new() );
				continue;
			}

			if ( line.StartsWith( beginBlock ) && currentCase is not null )
			{
				continue;
			}

			if ( line.StartsWith( endBlock ) && currentCase is not null )
			{
				currentCase = null;
				continue;
			}

			if ( currentCase is not null )
			{
				roomCaseText[currentCase].Add( line );
			}
		}

		if ( Opts.Verbose )
		{
			Log.Debug( $"Function Start Index: {functionStartIndex}" );
			Log.Debug( $"Function End Index: {functionEndIndex}" );
			Log.Debug( $"Rooms parsed: {roomCaseText.Count}" );
			Log.Debug( "Starting to create entities" );
		}

		foreach ( var pair in roomCaseText )
		{
			if ( Opts.Verbose )
			{
				Log.Debug( $"Parsing {pair.Key} " );
			}

			if ( !List.Rooms.ContainsKey( pair.Key ) )
			{
				Log.Warn( $" Room {pair.Key} was inside the function, but not in the rooms list!" );
				continue;
			}

			if ( !ParseScriptLines( List.Rooms[pair.Key], pair.Value, Opts ) )
			{
				Log.Error( $"Failed to parse script text for {pair.Key}" );
			}
		}
	}

	private static bool ParseScriptLines( Room Room, List<string> ScriptText, Options Opts )
	{
		foreach ( var line in ScriptText )
		{
			if ( line.IndexOf( ':' ) != -1 )
			{
				List<string> multiLineCalls = line
							.Split( ":" )
							.Select( x =>
							{
								x = x.Trim();
								return x;
							} )
							.ToList();

				foreach ( var subLine in multiLineCalls )
				{
					if ( !ParseScriptText( Room, subLine, Opts ) )
					{
						return false;
					}
				}

				continue;
			}

			if ( !ParseScriptText( Room, line, Opts ) )
			{
				return false;
			}
		}

		return true;
	}

	private static bool ParseScriptText( Room Room, string Line, Options Opts )
	{
		// @todo Split calls with : and parse them as well, for that we should probably have ParseScriptLine

		if ( !funcWithReturnRegex.IsMatch( Line ) )
		{
			return true;
		}

		var match = funcWithReturnRegex.Match( Line );
		var funcName = match.Groups[2].Value;

		if ( skippedFunctions.Contains( funcName ) )
		{
			if ( Opts.Verbose )
			{
				Log.Debug( $"Skipping {funcName} as it was a skipped function" );
			}
			return true;
		}

		switch ( funcName )
		{
			case "CreateDoor":
				{
					if ( !ParseCreateDoor( Room, match, Line ) )
					{
						Log.Error( $"Failed to create door for {Room.Name}" );
						return false;
					}

					break;
				}

			case "LoadAnimMesh_Strict":
			case "LoadMesh_Strict":
				{
					if ( !ParseLoadMeshStrict( Room, match, Line ) )
					{
						Log.Error( $"Failed to create mesh for {Room.Name}" );
						return false;
					}

					break;
				}

			case "CreateItem":
				{
					if ( !ParseCreateItem( Room, match, Line ) )
					{
						Log.Error( $"Failed to create item for {Room.Name}" );
						return false;
					}

					break;
				}

			case "CreatePivot":
				{
					if ( !ParseCreatePivot( Room, match, Line ) )
					{
						Log.Error( $"Failed to create pivot for {Room.Name}" );
						return false;
					}

					break;
				}

			case "CopyEntity":
				{
					if ( !ParseCopyEntity( Room, match, Line ) )
					{
						if ( Opts.Verbose )
						{
							Log.Warn( $"Failed to copy {match.Groups[1].Value}. Please check the source. If this entity was anything other than a sprite or decal, please create an issue!" );
						}
					}

					break;
				}

			case "CreateSecurityCam":
				{
					if ( !ParseCreateSecurityCam( Room, match, Line ) )
					{
						Log.Error( $"Failed to create securitycam for {Room.Name}" );
						return false;
					}

					break;
				}

			case "CreateButton":
				{
					if ( !ParseCreateButton( Room, match, Line ) )
					{
						Log.Error( $"Failed to create button for {Room.Name}" );
						return false;
					}

					break;
				}

			case "CreateWaypoint":
				{
					if ( !ParseCreateWaypoint( Room, match, Line ) )
					{
						Log.Error( $"Failed to create button for {Room.Name}" );
						return false;
					}

					break;
				}

			default:
				{
					if ( Opts.Verbose )
					{
						Log.Warn( $"Skipping {funcName} as it did not have a case" );
					}
					break;
				}
		}

		return true;
	}

	private static string ExtractVarName( string VarName )
	{
		string newName = VarName;

		// If we're r\{var} strip the fluff
		if ( objectAssignmentRegex.IsMatch( newName ) )
		{
			newName = objectAssignmentRegex.Match( newName ).Groups[1].Value;
		}

		// If we're a local variable, strip it
		if ( newName.StartsWith( "Local " ) )
		{
			newName = newName.Remove( 0, 6 );
		}

		// If we contain a type, remove it
		var dotIndex = newName.IndexOf( '.' );
		if ( dotIndex != -1 )
		{
			newName = newName.Substring( 0, dotIndex );
		}

		// Trim the name just in case
		newName = newName.Trim();

		return newName;
	}
}
