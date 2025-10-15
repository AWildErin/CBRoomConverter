namespace CBRoomConverter;

internal static class GlobalConfiguration
{
	public const float ROOM_SCALE = 1f;

	// List of function args to always ignore
	public static List<string> ALWAYS_IGNORED_ARGS = new()
	{
		"angle",
		"x",
		"y",
		"z"
	};
}
