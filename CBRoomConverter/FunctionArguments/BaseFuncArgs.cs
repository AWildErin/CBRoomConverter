using CBRoomConverter.Models;
using CBRoomConverter.Reflection;
using System.Reflection;

namespace CBRoomConverter.FunctionArguments;

internal abstract class BaseFuncArgs
{
	public string VariableName { get; set; } = "NAME_NOT_ASSIGNED";

	public BaseFuncArgs()
	{
	}

	/// <summary>
	/// Adds all the func arguments to the entity
	/// </summary>
	/// <param name="Ent">Entity which the props will be added to.</param>
	/// <param name="PropsToIgnore">List of serialized names to ignore.</param>
	public void AddPropertiesToEntity( Entity Ent, List<string>? PropsToIgnore = null )
	{
		var type = GetType();

		// Loop over all our properties and add them to the ent props. If the value is null, dont bother adding it.
		// if the property already exists, then just update it's value

		var propInfos = type.GetProperties()
			.Where( x => x.GetCustomAttribute<BlitzFuncArgIndexAttribute>() is not null )
			.ToList();

		foreach ( var prop in propInfos )
		{
			if ( prop is null )
			{
				continue;
			}

			var attrib = prop.GetCustomAttribute<BlitzFuncArgIndexAttribute>()!;

			var serializedName = attrib.SerializedName;

			if ( (PropsToIgnore is not null && PropsToIgnore.Contains( serializedName )) || GlobalConfiguration.ALWAYS_IGNORED_ARGS.Contains( serializedName ) )
			{
				continue;
			}

			var value = prop.GetValue( this ) as string;
			if ( value is null )
			{
				continue;
			}

			if ( Ent.Properties.ContainsKey( serializedName ) )
			{
				Ent.Properties[serializedName] = value;
			}
			else
			{
				Ent.Properties.Add( serializedName, value );
			}
		}
	}
}
