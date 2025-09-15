using CBRoomConverter.Enums;
using CBRoomConverter.Models;
using AWildErin.Utility;
using IniParser;
using IniParser.Model;

namespace CBRoomConverter;
internal class RoomParser
{
	// {0} is zone
	private const string roomDataFormat = "/Game/Developers/erinsh/RoomTest2/Blueprints/Map/Rooms/{0}";
	private const string roomArtFormat = "/Game/Developers/erinsh/RoomTest2/Art/Map/Rooms/{0}";

	private static readonly Dictionary<string, ESCPRoomZone> stringToZone = new()
	{
		{ "1", ESCPRoomZone.LightContainmentZone },
		{ "2", ESCPRoomZone.HeavyContainmentZone },
		{ "3", ESCPRoomZone.EntranceZone }
	};

	private static readonly Dictionary<ESCPRoomZone, string> zoneToString = new()
	{
		{ ESCPRoomZone.None, "Special" },
		{ ESCPRoomZone.LightContainmentZone, "LCZ" },
		{ ESCPRoomZone.HeavyContainmentZone, "HCZ" },
		{ ESCPRoomZone.EntranceZone, "EZ" }
	};

	private static readonly Dictionary<string, ESCPRoomType> stringToType = new()
	{
		{ "1", ESCPRoomType.EndRoom },
		{ "2", ESCPRoomType.TwoWay },
		{ "2c", ESCPRoomType.TwoWayCorner },
		{ "3", ESCPRoomType.ThreeWay },
		{ "4", ESCPRoomType.FourWay }
	};

	public static void ParseIni( RoomList List, string IniPath, Options Opts )
	{
		var parser = new FileIniDataParser();
		IniData data = parser.ReadFile( IniPath );

		if ( Opts.Verbose )
		{
			Log.Debug( "Adding room ambience" );
		}

		if ( data.Sections.ContainsSection( "room ambience" ) )
		{
			var section = data.Sections["room ambience"];
			foreach ( var pair in section )
			{
				List.RoomAmbience.Add( pair.KeyName, pair.Value.Replace( "\\", "/" ) );
			}
		}

		// Loop over all the rooms
		foreach ( var section in data.Sections )
		{
			// Skip ambience
			if ( section.SectionName == "room ambience" )
			{
				continue;
			}

			if ( Opts.Verbose )
			{
				Log.Debug( $"Adding {section.SectionName}" );
			}

			Room room = new();
			room.Name = section.SectionName;

			if ( section.Keys.ContainsKey( "descr" ) )
			{
				room.Description = section.Keys["descr"];
			}

			if ( section.Keys.ContainsKey( "mesh path" ) )
			{
				room.Mesh = section.Keys["mesh path"];
			}

			if ( section.Keys.ContainsKey( "shape" ) )
			{
				room.RoomType = stringToType[section.Keys["shape"].ToLower()];
			}

			if ( section.Keys.ContainsKey( "commonness" ) )
			{
				room.Commoness = int.Parse( section.Keys["commonness"] );
			}

			if ( section.Keys.ContainsKey( "large" ) )
			{
				bool result = false;
				string value = section.Keys["large"];

				if ( !bool.TryParse( value, out result ) )
				{
					result = value == "1";
				}

				room.Large = result;
			}

			if ( section.Keys.ContainsKey( "disabledecals" ) )
			{
				bool result = false;
				string value = section.Keys["disabledecals"];

				if ( !bool.TryParse( value, out result ) )
				{
					result = value == "1";
				}

				room.DisableDecals = result;
			}

			if ( section.Keys.ContainsKey( "usevolumelighting" ) )
			{
				bool result = false;
				string value = section.Keys["usevolumelighting"];

				if ( !bool.TryParse( value, out result ) )
				{
					result = value == "1";
				}

				room.UseVolumeLighting = result;
			}

			if ( section.Keys.ContainsKey( "disableoverlapcheck" ) )
			{
				bool result = false;
				string value = section.Keys["disableoverlapcheck"];

				if ( !bool.TryParse( value, out result ) )
				{
					result = value == "1";
				}

				room.DisableOverlapCheck = result;
			}

			if ( section.Keys.ContainsKey( "zone1" ) )
			{
				room.Zone1 = stringToZone[section.Keys["zone1"]];
			}

			if ( section.Keys.ContainsKey( "zone2" ) )
			{
				room.Zone2 = stringToZone[section.Keys["zone2"]];
			}

			if ( section.Keys.ContainsKey( "zone3" ) )
			{
				room.Zone3 = stringToZone[section.Keys["zone3"]];
			}

			// Add our properties
			room.DataDirectory = string.Format( roomDataFormat, zoneToString[room.Zone1] );
			room.ArtDirectory = string.Format( roomArtFormat, zoneToString[room.Zone1] );

			List.Rooms.Add( section.SectionName, room );
		}
	}
}
