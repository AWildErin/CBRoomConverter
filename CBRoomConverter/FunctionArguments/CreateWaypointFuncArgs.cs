using CBRoomConverter.Reflection;

namespace CBRoomConverter.FunctionArguments;

[BlitzFuncArgs( "CreateWaypoint" )]
internal class CreateWaypointFuncArgs : BaseFuncArgs
{
	[BlitzFuncArgIndex( "x", 0, Optional: false )]
	public required string x { get; set; }

	[BlitzFuncArgIndex( "y", 1, Optional: false )]
	public required string y { get; set; }

	[BlitzFuncArgIndex( "z", 2, Optional: false )]
	public required string z { get; set; }

	[BlitzFuncArgIndex( "door", 3, Optional: false )]
	public required string door { get; set; }

	[BlitzFuncArgIndex( "room", 4, Optional: false )]
	public required string room { get; set; }
}
