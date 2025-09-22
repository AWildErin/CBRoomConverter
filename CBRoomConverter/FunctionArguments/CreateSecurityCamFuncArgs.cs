using CBRoomConverter.Reflection;

namespace CBRoomConverter.FunctionArguments;

[BlitzFuncArgs( "CreateSecurityCam" )]
internal class CreateSecurityCamFuncArgs : BaseFuncArgs
{
	[BlitzFuncArgIndex( "x", 0, Optional: false )]
	public required string x { get; set; }

	[BlitzFuncArgIndex( "y", 1, Optional: false )]
	public required string y { get; set; }

	[BlitzFuncArgIndex( "z", 2, Optional: false )]
	public required string z { get; set; }

	[BlitzFuncArgIndex( "r", 3, Optional: false )]
	public required string r { get; set; }

	[BlitzFuncArgIndex( "screen", 4, Optional: true )]
	public string? screen { get; set; }
}
