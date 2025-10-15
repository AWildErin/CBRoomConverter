using AWildErin.Utility;
using CBRoomConverter.FunctionArguments;
using CBRoomConverter.Models;
using OpenTK.Mathematics;

namespace CBRoomConverter;

internal partial class BlitzParser
{
	private static string ExpandAccessor( Room Room, string FunctionName, BaseFuncArgs FuncArgs )
	{
		switch ( FunctionName )
		{
			case "EntityPitch":
				{
					var args = (EntityPitchFuncArgs)FuncArgs;

					var entity = Room.FindEntity( args.entity, true );
					if ( entity is null )
					{
						Log.Error( $"Failed to find entity with name {args.entity} when accessing {FunctionName}" );
						return "";
					}

					return (entity.Rotation.ToEulerAngles().X * MathHelper.RadToDeg).ToString();
				}
			case "EntityYaw":
				{
					var args = (EntityYawFuncArgs)FuncArgs;

					var entity = Room.FindEntity( args.entity, true );
					if ( entity is null )
					{
						Log.Error( $"Failed to find entity with name {args.entity} when accessing {FunctionName}" );
						return "";
					}

					return (entity.Rotation.ToEulerAngles().Y * MathHelper.RadToDeg).ToString();
				}
			case "EntityRoll":
				{
					var args = (EntityRollFuncArgs)FuncArgs;

					var entity = Room.FindEntity( args.entity, true );
					if ( entity is null )
					{
						Log.Error( $"Failed to find entity with name {args.entity} when accessing {FunctionName}" );
						return "";
					}

					return (entity.Rotation.ToEulerAngles().Z * MathHelper.RadToDeg).ToString();
				}
			case "EntityX":
				{
					var args = (EntityXFuncArgs)FuncArgs;

					var entity = Room.FindEntity( args.entity, true );
					if ( entity is null )
					{
						Log.Error( $"Failed to find entity with name {args.entity} when accessing {FunctionName}" );
						return "";
					}

					return (entity.Position.X).ToString();
				}
			case "EntityY":
				{
					var args = (EntityYFuncArgs)FuncArgs;

					var entity = Room.FindEntity( args.entity, true );
					if ( entity is null )
					{
						Log.Error( $"Failed to find entity with name {args.entity} when accessing {FunctionName}" );
						return "";
					}

					return (entity.Position.Y).ToString();
				}
			case "EntityZ":
				{
					var args = (EntityZFuncArgs)FuncArgs;

					var entity = Room.FindEntity( args.entity, true );
					if ( entity is null )
					{
						Log.Error( $"Failed to find entity with name {args.entity} when accessing {FunctionName}" );
						return "";
					}

					return (entity.Position.Z).ToString();
				}
			default:
				{
					Log.Warn( $"No accessor case for {FunctionName}" );
					break;
				}
		}

		return "";
	}
}
