using CommandLine;

namespace CBRoomConverter;

internal class Options
{
	[Option( 'i', "input", Required = true, HelpText = "Input file, rooms.ini" )]
	public required string InputFile { get; set; }

	[Option( 'o', "output", Required = true, HelpText = "Output file, rooms.json" )]
	public required string OutputFile { get; set; }

	[Option( 'b', "blitz", HelpText = "Path to mapsystem.bb, used to get room entities" )]
	public string? BlitzFile { get; set; }

	[Option( 'f', "force", HelpText = "If the output file exists, it will override it." )]
	public bool Force { get; set; }

	[Option( 'v', "verbose", HelpText = "Enables additional logging" )]
	public bool Verbose { get; set; }

	[Option( 'u', "unreal", HelpText = "Enables exporting vectors in Unreal format (It needs to be Z X Y from Blitz3D)" )]
	public bool UnrealVectors { get; set; }
}
