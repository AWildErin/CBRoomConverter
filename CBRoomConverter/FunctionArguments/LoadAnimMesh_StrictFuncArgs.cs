using CBRoomConverter.Reflection;

namespace CBRoomConverter.FunctionArguments;

[BlitzFuncArgs( "LoadAnimMesh_Strict" )]
internal class LoadAnimMesh_StrictFuncArgs : BaseFuncArgs
{
	[BlitzFuncArgIndex( "File", 0, Optional: false )]
	public required string File { get; set; }

	[BlitzFuncArgIndex( "parent", 1, Optional: true )]
	public string? parent { get; set; }
}
