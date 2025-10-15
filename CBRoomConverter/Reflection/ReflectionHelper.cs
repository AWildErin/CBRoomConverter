using CBRoomConverter.FunctionArguments;
using System.Reflection;

namespace CBRoomConverter.Reflection;

internal static class ReflectionHelper
{
	private static BaseFuncArgs? ConstructFuncArgsInternal( Type? type, List<string> Args )
	{
		if ( type is null )
		{
			return null;
		}

		return null;
	}

	public static T CreateFuncArgsFromType<T>( List<string> Args )
		where T : BaseFuncArgs, new()
	{
		T argsObj = new();

		for ( int i = 0; i < Args.Count; i++ )
		{

		}

		return argsObj;
	}

	public static BaseFuncArgs? CreateFuncArgs( string FunctionName, List<string> Args )
	{
		var foundType = Assembly
						.GetExecutingAssembly()
						.GetTypes()
						.Where(
							x =>
							{
								var attribs = x.GetCustomAttributes<BlitzFuncArgsAttribute>();
								var attrib = attribs.Where( x => x.FunctionName == FunctionName ).FirstOrDefault();
								return attrib is not null;
							}
						)
						.FirstOrDefault();

		if ( foundType is null )
		{
			return null;
		}

		BaseFuncArgs? argsObj = Activator.CreateInstance( foundType ) as BaseFuncArgs;
		if ( argsObj is null )
		{
			throw new Exception( $"Failed to create type {foundType}" );
		}

		// func arg index to prop info
		// could've just made this an array but i dont feel like doing that
		Dictionary<int, PropertyInfo> propertyInfos = new();

		var props = foundType.GetProperties();
		foreach ( var prop in props )
		{
			if ( prop is null )
			{
				continue;
			}

			var attrib = prop.GetCustomAttribute<BlitzFuncArgIndexAttribute>();
			if ( attrib is null )
			{
				continue;
			}

			propertyInfos.Add( attrib.ArgumentIndex, prop );
		}

		for ( int i = 0; i < Args.Count; i++ )
		{
			var text = Args[i];
			var propInfo = propertyInfos[i];

			propInfo.SetValue( argsObj, text );
		}

		return argsObj;
	}
}
