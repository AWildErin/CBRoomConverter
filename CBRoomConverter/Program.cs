using AWildErin.Utility;
using CBRoomConverter.Converters;
using CBRoomConverter.Models;
using CommandLine;
using CommandLine.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CBRoomConverter;

internal class Program
{
	public static void Main( string[] args )
	{
		Log.Initialize( false );

		var parser = new CommandLine.Parser( with => { with.HelpWriter = null; with.EnableDashDash = true; } );
		var result = parser.ParseArguments<Options>( args );

		result
			.WithParsed<Options>( OnParsed )
			.WithNotParsed( err => DisplayHelp( result, err ) );
	}

	public static void OnParsed( object Obj )
	{
		Options opts = (Options)Obj;

		Log.Info( "CBRoomConverter" );

		if ( !File.Exists( opts.InputFile ) )
		{
			Log.Error( "File does not exist!" );
			Log.Error( opts.InputFile );
			return;
		}

		if ( File.Exists( opts.OutputFile ) && !opts.Force )
		{
			Log.Error( "File already existed. Either delete the file, or rerun with -f to override the file." );
			Log.Error( opts.OutputFile );
			return;
		}

		if ( opts.BlitzFile is not null && !File.Exists( opts.BlitzFile ) )
		{
			Log.Error( "File does not exist!" );
			Log.Error( opts.BlitzFile );
		}

		Log.Info( $"Reading: {opts.InputFile}" );
		Log.Info( $"Writing: {opts.OutputFile}" );

		RoomList list = new RoomList();
		RoomParser.ParseIni( list, opts.InputFile, opts );

		if ( opts.BlitzFile is not null )
		{
			BlitzParser.ParseBlitz( list, opts.BlitzFile, opts );
		}

		var jsonOpts = new JsonSerializerOptions()
		{
			WriteIndented = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
		};
		jsonOpts.Converters.Add( new Vector3Converter( opts.UnrealVectors ) );
		jsonOpts.Converters.Add( new QuaternionConverter() );
		jsonOpts.Converters.Add( new JsonStringEnumConverter() );

		var outText = JsonSerializer.Serialize( list, jsonOpts );
		File.WriteAllText( opts.OutputFile, outText );
	}

	private static void DisplayHelp<T>( ParserResult<T> Result, IEnumerable<Error> Errors )
	{
		var helpText = HelpText.AutoBuild( Result, h =>
		{
			h.AddNewLineBetweenHelpSections = false;
			h.AdditionalNewLineAfterOption = false;
			h.AddDashesToOption = true;
			h.Copyright = "";
			return HelpText.DefaultParsingErrorsHandler( Result, h );
		}, e => e, verbsIndex: true );

		var splitHelpText = helpText.ToString().Split( Environment.NewLine, StringSplitOptions.None );
		foreach ( var line in splitHelpText )
		{
			Log.Info( line );
		}
	}

}
