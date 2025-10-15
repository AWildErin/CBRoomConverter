using NCalc;
using System.Text.RegularExpressions;

namespace CBRoomConverter.Helpers;

internal class EntityHelpers
{
	private static readonly Regex posNumberRegex = new( @"([+\-*/]\s*|\b)(\d+(?:\.\d+)?)" );

	public static float EvaluateExpression( string Input )
	{
		// Strip room accessors
		// @todo	Is there any instances where room accessors are used to multiply?
		//			if there is, then this will fail.
		var exprString = Input;
		exprString = exprString.Replace( "r\\x", "0" );
		exprString = exprString.Replace( "r\\y", "0" );
		exprString = exprString.Replace( "r\\z", "0" );

		var expr = new Expression( exprString );
		expr.Parameters["RoomScale"] = GlobalConfiguration.ROOM_SCALE;

		return Convert.ToSingle( expr.Evaluate() );
	}

	/*
	public static string ExtractPosition( string Input )
	{
		string output = Input;

		if ( posNumberRegex.IsMatch( output ) )
		{
			// NOTE: This function handles whether or not we need to negate the value
			// In SCP:CB they will do stuff like r/x - val, which for actual usage, we need to then take the operator
			var match = posNumberRegex.Match( output );

			var op = match.Groups[1].Value.Trim();
			if ( op.Equals( "-" ) )
			{
				output = $"{op}{match.Groups[2].Value}";
			}
			else
			{
				output = match.Groups[2].Value;
			}
		}
		else
		{
			return "0";
		}

		// @TODO Handle EntityX, EntityY, EntityZ

		return output;
	}

	public static string ExtractRotation( string Input )
	{
		// @todo Extract this into a base, then have ExtractPosition and ExtractRotation use it,
		// Then both can handle their respective entity funcs
		return EntityHelpers.ExtractPosition( Input );
	}
	*/
}
