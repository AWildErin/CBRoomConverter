using CBRoomConverter.Reflection;

namespace CBRoomConverter.FunctionArguments;

[BlitzFuncArgs( "CreatePivot" )]
internal class CreatePivotFuncArgs : BaseFuncArgs
{
	[BlitzFuncArgIndex( "parent", 0, Optional: true )]
	public string? parent { get; set; }
}
