using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace CBRoomConverter.Tests;

internal static class TestHelpers
{
	public static string GetTestResource( string ResourcePath )
	{
		// @todo This isn't ideal, but for my usecase it's fne
		string exeDir = Directory.GetCurrentDirectory();
		string testResourcePath = Path.Combine( exeDir, "../../../../TestResources" );
		return Path.GetFullPath( Path.Combine( testResourcePath, ResourcePath ) );
	}
}
