using CBRoomConverter.Reflection;

namespace CBRoomConverter.FunctionArguments;

[BlitzFuncArgs( "CopyEntity" )]
internal class CopyEntityFuncArgs : BaseFuncArgs
{
	[BlitzFuncArgIndex( "entity", 0, Optional: false )]
	public required string entity { get; set; }

	[BlitzFuncArgIndex( "parent", 1, Optional: true )]
	public string? parent { get; set; }
}
