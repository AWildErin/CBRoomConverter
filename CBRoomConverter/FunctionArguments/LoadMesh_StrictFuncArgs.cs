using CBRoomConverter.Reflection;

namespace CBRoomConverter.FunctionArguments;

[BlitzFuncArgs( "LoadMesh_Strict" )]
internal class LoadMesh_StrictFuncArgs : BaseFuncArgs
{
	[BlitzFuncArgIndex( "File", 0, Optional: false )]
	public required string File { get; set; }

	[BlitzFuncArgIndex( "parent", 1, Optional: true )]
	public string? parent { get; set; }
}
