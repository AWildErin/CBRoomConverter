using AWildErin.Utility;
using CBRoomConverter.Enums;
using CBRoomConverter.FunctionArguments;
using CBRoomConverter.Models;
using CBRoomConverter.Reflection;
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
	private static readonly Regex funcCallRegex = new( @"\s*(.*?)\((.*)\)" );
	private static readonly Regex objectAssignmentRegex = new( @".*?[\\](.*?) =" );

	public static void ParseBlitz( RoomList List, string BlitzPath, Options? Opts = null )
	{
		int functionStartIndex = -1;
		int functionEndIndex = -1;
		bool insideFillRoom = false;

		string? currentCase = null;

		// Script text for each room case
		Dictionary<string, List<string>> roomCaseText = new();
		Dictionary<string, string> keysWithSameCase = new();

		if ( Opts is not null && Opts.Verbose )
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

		if ( Opts is not null && Opts.Verbose )
		{
			Log.Debug( $"Function Start Index: {functionStartIndex}" );
			Log.Debug( $"Function End Index: {functionEndIndex}" );
			Log.Debug( $"Rooms parsed: {roomCaseText.Count}" );
			Log.Debug( "Starting to create entities" );
		}

		foreach ( var pair in roomCaseText )
		{
			if ( Opts is not null && Opts.Verbose )
			{
				Log.Debug( $"Parsing {pair.Key} " );
			}

			if ( !List.Rooms.ContainsKey( pair.Key ) )
			{
				if ( Opts is not null && Opts.Verbose )
				{
					Log.Warn( $" Room {pair.Key} was inside the function, but not in the rooms list!" );
				}
				continue;
			}

			if ( !ParseScriptLines( List.Rooms[pair.Key], pair.Value, Opts ) )
			{
				Log.Error( $"Failed to parse script text for {pair.Key}" );
			}
		}
	}

	private static bool ParseScriptLines( Room Room, List<string> ScriptText, Options? Opts = null )
	{
		bool insideLoop = false;
		int nestedLoopCounter = 0;

		bool insideConditional = false;
		int nestedConditionalCounter = 0;

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

			// Skip loops for now
			if ( line.StartsWith( "For" ) )
			{
				insideLoop = true;

				nestedLoopCounter++;
				continue;
			}
			else if ( line.StartsWith( "Next" ) )
			{
				nestedLoopCounter--;

				if ( nestedLoopCounter <= 0 )
				{
					insideLoop = false;
				}

				continue;
			}

			// Skip if blocks for now, it would be nice to do basic parsing of these
			if ( line.StartsWith( "If" ) )
			{
				insideConditional = true;

				nestedConditionalCounter++;
				continue;
			}
			else if ( line.StartsWith( "EndIf" ) )
			{
				nestedConditionalCounter--;

				if ( nestedConditionalCounter <= 0 )
				{
					insideConditional = false;
				}

				continue;
			}

			if ( insideLoop || insideConditional )
			{
				if ( Opts is not null && Opts.Verbose )
				{
					Log.Info( $"Skipping {line} because of loop or conditional. Current nested count is {nestedConditionalCounter}:{nestedLoopCounter}" );
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

	private static bool ParseScriptText( Room Room, string Line, Options? Opts = null )
	{
		// Check our regexes
		Match? match = null;
		string variableName = string.Empty;
		string funcName = string.Empty;
		string funcArguments = string.Empty;

		if ( funcWithReturnRegex.IsMatch( Line ) )
		{
			match = funcWithReturnRegex.Match( Line );
			variableName = match.Groups[1].Value;
			funcName = match.Groups[2].Value;
			funcArguments = match.Groups[3].Value;
		}
		else if ( funcCallRegex.IsMatch( Line ) )
		{
			match = funcCallRegex.Match( Line );
			funcName = match.Groups[1].Value;
			funcArguments = match.Groups[2].Value;
			variableName = funcName;
		}
		else
		{
			return true;
		}

		if ( match is null )
		{
			throw new ArgumentNullException( nameof( match ) );
		}

		// Clean up any trailing spaces
		variableName = variableName.Trim();
		funcName = funcName.Trim();
		funcArguments = funcArguments.Trim();

		if ( skippedFunctions.Contains( funcName ) )
		{
			if ( Opts is not null && Opts.Verbose )
			{
				Log.Debug( $"Skipping {funcName} as it was a skipped function" );
			}
			return true;
		}

		// Expand any accessors
		var funcArgs = ExtractArgsFromString( funcArguments );
		for ( var i = 0; i < funcArgs.Count; i++)
		{
			var arg = funcArgs[i];

			// Do we have a function?
			if ( !funcCallRegex.IsMatch( arg ) )
			{
				continue;
			}

			// @todo We're a match now, so extract it out and add back the result based on the func name

			var accessorMatch = funcCallRegex.Match( arg );
			var accessorName = accessorMatch.Groups[1].Value;
			var accessorArgs = ExtractArgsFromString( accessorMatch.Groups[2].Value );

			var accessorArgsObj = ReflectionHelper.CreateFuncArgs( accessorName, accessorArgs );
			if ( accessorArgsObj is null )
			{
				if ( Opts is not null && Opts.Verbose )
				{
					Log.Warn( $"Skipping {accessorName} as it did not have a corresponding func args class. Please implement the accessor!" );
				}

				// Remove the accessor and just carry on

				continue;
			}

			// Expand the accessor, and replace the text inside it
			string replacedText = ExpandAccessor( Room, accessorName, accessorArgsObj );
			funcArgs[i] = arg.Remove( accessorMatch.Index, accessorMatch.Length ).Insert( accessorMatch.Index, replacedText );
		}

		// Attempt to find the func args type
		var funcArgsObj = ReflectionHelper.CreateFuncArgs( funcName, funcArgs );
		if ( funcArgsObj is null )
		{
			if ( Opts is not null && Opts.Verbose )
			{
				Log.Warn( $"Skipping {funcName} as it did not have a corresponding func args class" );
			}
			return true;
		}
		funcArgsObj.VariableName = variableName;

		switch ( funcName )
		{
			case "CreateDoor":
				{
					if ( !CreateDoor( Room, (CreateDoorFuncArgs)funcArgsObj ) )
					{
						Log.Error( $"Failed to create door for {Room.Name}" );
						return false;
					}

					break;
				}

			case "LoadMesh_Strict":
			case "LoadAnimMesh_Strict":
				{
					if ( !CreateBasicEntity( Room, funcArgsObj, ESCPCBRoomCreatorEntityType.Mesh ) )
					{
						Log.Error( $"Failed to create mesh for {Room.Name}" );
						return false;
					}

					break;
				}

			case "CreateItem":
				{
					if ( !CreateItem( Room, (CreateItemFuncArgs)funcArgsObj ) )
					{
						Log.Error( $"Failed to create item for {Room.Name}" );
						return false;
					}

					break;
				}

			case "CreateSecurityCam":
				{
					if ( !CreateSecurityCam( Room, (CreateSecurityCamFuncArgs)funcArgsObj ) )
					{
						Log.Error( $"Failed to create securitycam for {Room.Name}" );
						return false;
					}

					break;
				}

			case "CreateButton":
				{
					if ( !CreateButton( Room, (CreateButtonFuncArgs)funcArgsObj ) )
					{
						Log.Error( $"Failed to create button for {Room.Name}" );
						return false;
					}

					break;
				}

			case "CreateWaypoint":
				{
					if ( !CreateWaypoint( Room, (CreateWaypointFuncArgs)funcArgsObj ) )
					{
						Log.Error( $"Failed to create button for {Room.Name}" );
						return false;
					}

					break;
				}

			case "CreatePivot":
				{
					if ( !CreateBasicEntity( Room, funcArgsObj, ESCPCBRoomCreatorEntityType.Pivot ) )
					{
						Log.Error( $"Failed to create pivot for {Room.Name}" );
						return false;
					}

					break;
				}

			// Entity methods
			case "CopyEntity":
				{
					if ( !CopyEntity( Room, (CopyEntityFuncArgs)funcArgsObj ) )
					{
						if ( Opts is not null && Opts.Verbose )
						{
							Log.Warn( $"Failed to copy {match.Groups[1].Value}. Please check the source. If this entity was anything other than a sprite or decal, please create an issue!" );
						}
					}

					break;
				}

			case "ScaleEntity":
				{
					if ( !ScaleEntity( Room, (ScaleEntityFuncArgs)funcArgsObj ) )
					{
						Log.Error( $"failed to parse ScaleEntity call for {Room.Name}" );
					}

					break;
				}

			case "PositionEntity":
				{
					if ( !PositionEntity( Room, (PositionEntityFuncArgs)funcArgsObj ) )
					{
						Log.Error( $"failed to parse PositionEntity call for {Room.Name}" );
					}

					break;
				}

			case "MoveEntity":
				{
					if ( !MoveEntity( Room, (MoveEntityFuncArgs)funcArgsObj ) )
					{
						Log.Error( $"failed to parse MoveEntity call for {Room.Name}" );
					}

					break;
				}

			case "TranslateEntity":
				{
					if ( !TranslateEntity( Room, (TranslateEntityFuncArgs)funcArgsObj ) )
					{
						Log.Error( $"failed to parse TranslateEntity call for {Room.Name}" );
					}

					break;
				}

			case "RotateEntity":
				{
					if ( !RotateEntity( Room, (RotateEntityFuncArgs)funcArgsObj ) )
					{
						Log.Error( $"failed to parse RotateEntity call for {Room.Name}" );
					}

					break;
				}

			case "TurnEntity":
				{
					if ( !TurnEntity( Room, (TurnEntityFuncArgs)funcArgsObj ) )
					{
						Log.Error( $"failed to parse TurnEntity call for {Room.Name}" );
					}

					break;
				}

			default:
				{
					if ( Opts is not null && Opts.Verbose )
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
