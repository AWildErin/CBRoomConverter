namespace CBRoomConverter.Reflection;

[AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = true )]
internal sealed class BlitzFuncArgsAttribute : Attribute
{
	public string FunctionName { get; private set; }

	public BlitzFuncArgsAttribute( string InFunctionName )
	{
		FunctionName = InFunctionName;
	}
}
