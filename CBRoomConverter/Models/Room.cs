using CBRoomConverter.Enums;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace CBRoomConverter.Models;

[DebuggerDisplay( "{Name}" )]
internal class Room
{
	public string? Name { get; set; }
	public string? Description { get; set; }
	public string? Mesh { get; set; }

	public int Commoness { get; set; } = 0;

	public bool Large { get; set; } = false;
	public bool DisableDecals { get; set; } = false;
	public bool UseVolumeLighting { get; set; } = false;
	public bool DisableOverlapCheck { get; set; } = false;

	public ESCPRoomType RoomType { get; set; } = ESCPRoomType.None;
	public ESCPRoomZone Zone1 { get; set; } = ESCPRoomZone.None;
	public ESCPRoomZone Zone2 { get; set; } = ESCPRoomZone.None;
	public ESCPRoomZone Zone3 { get; set; } = ESCPRoomZone.None;

	public string? DataDirectory { get; set; }
	public string? ArtDirectory { get; set; }

	// Filled from supplied MapSystem.bb
	public List<Entity> Entities { get; set; } = new List<Entity>();


	// An internal map so we can create instances of names as needed.
	[JsonIgnore]
	public Dictionary<string, int> InternalNameIndex { get; set; } = new();

	// Maps a name to a specific entity, will get updated each time an entity with that name is created
	[JsonIgnore]
	public Dictionary<string, Entity> InternalNameToEntity { get; set; } = new();

	public Entity? FindEntity( string Name, bool CheckChildren = false )
	{
		if ( InternalNameToEntity.ContainsKey( Name ) )
		{
			return InternalNameToEntity[Name];
		}

		if ( CheckChildren )
		{
			// Use the text past the last occurance of \ as the child name
			int lastIdx = Name.LastIndexOf( '\\' );
			if ( lastIdx > -1 )
			{
				string childName = Name.Substring( lastIdx + 1 );
				string parentName = Name.Substring( 0, lastIdx );

				var parent = FindEntity( parentName );
				if ( parent is null )
				{
					return null;
				}

				return parent.FindChildEntity( childName );
			}
		}

		return null;
	}
}
