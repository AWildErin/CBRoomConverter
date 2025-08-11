using System.Runtime.InteropServices;

namespace CBRoomConverter.Utility;

public static partial class Log
{
	// Adapted from https://stackoverflow.com/a/718505

	private const int stdOutputHandke = -11;
	private const uint enableVirtualTerminalProcessing = 0x0004;

	[DllImport( "kernel32.dll" )]
	private static extern bool AllocConsole();

	[DllImport( "kernel32.dll" )]
	private static extern bool FreeConsole();

	[DllImport( "kernel32.dll" )]
	private static extern IntPtr GetConsoleWindow();

	[DllImport( "kernel32.dll" )]
	private static extern int GetConsoleOutputCP();

	[DllImport( "kernel32.dll", SetLastError = true )]
	private static extern IntPtr GetStdHandle( int nStdHandle );

	[DllImport( "kernel32.dll", SetLastError = true )]
	private static extern bool GetConsoleMode( IntPtr hConsoleHandle, out uint lpMode );

	[DllImport( "kernel32.dll", SetLastError = true )]
	private static extern bool SetConsoleMode( IntPtr hConsoleHandle, uint dwMode );

	public static bool HasConsole
	{
		get { return GetConsoleWindow() != IntPtr.Zero; }
	}

	public static void ShowConsole()
	{
		if ( HasConsole )
		{
			return;
		}

		AllocConsole();

		var handle = GetStdHandle( stdOutputHandke );
		if ( !GetConsoleMode( handle, out uint mode ) )
		{
			return;
		}
		mode |= enableVirtualTerminalProcessing;
		SetConsoleMode( handle, mode );
	}

	public static void CloseConsole()
	{
		if ( !HasConsole )
		{
			return;
		}

		FreeConsole();
	}
}
