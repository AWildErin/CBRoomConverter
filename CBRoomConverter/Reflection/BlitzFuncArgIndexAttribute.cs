namespace CBRoomConverter.Reflection;

[AttributeUsage( AttributeTargets.Property, Inherited = false )]
internal sealed class BlitzFuncArgIndexAttribute : Attribute
{
	public string SerializedName { get; set; }
	public int ArgumentIndex { get; set; }
	public bool IsOptional { get; set; }

	public BlitzFuncArgIndexAttribute( string InSerializedName, int Index, bool Optional = false )
	{
		SerializedName = InSerializedName;
		ArgumentIndex = Index;
		IsOptional = Optional;
	}
}
