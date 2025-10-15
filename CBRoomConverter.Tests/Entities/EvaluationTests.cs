using AWildErin.Utility;
using CBRoomConverter.Converters;
using CBRoomConverter.Models;
using NUnit.Framework.Internal;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CBRoomConverter.Tests.Entities;

internal class EvaluationTests
{
	public RoomList TestRoomList = new();

	[OneTimeSetUp]
	public void Init()
	{
		RoomParser.ParseIni( TestRoomList, TestHelpers.GetTestResource( "EvaluationTests.ini" ) );
		BlitzParser.ParseBlitz( TestRoomList, TestHelpers.GetTestResource( "EvaluationTests.bb" ) );

		//Log.Initialize();
	}

	[Test]
	public void RoomScale()
	{
		var room = TestRoomList.Rooms["RoomScale"];
		var ent = room.FindEntity( "r\\Objects[2]" );
		Assert.That( ent, Is.Not.Null );

		Assert.That( ent.Position.X, Is.EqualTo( 100f * GlobalConfiguration.ROOM_SCALE ).Within( 0.1f ) );
		Assert.That( ent.Position.Y, Is.EqualTo( 0f * GlobalConfiguration.ROOM_SCALE ).Within( 0.1f ) );
		Assert.That( ent.Position.Z, Is.EqualTo( 100f * GlobalConfiguration.ROOM_SCALE ).Within( 0.1f ) );
	}

	[Test]
	public void RoomXYZ()
	{
		var room = TestRoomList.Rooms["RoomXYZ"];
		var ent = room.FindEntity( "r\\Objects[2]" );
		Assert.That( ent, Is.Not.Null );

		Assert.That( ent.Position.X, Is.EqualTo( -832f ).Within( 0.1f ) );
		Assert.That( ent.Position.Y, Is.EqualTo( 0.7f ).Within( 0.1f ) );
		Assert.That( ent.Position.Z, Is.EqualTo( 160f ).Within( 0.1f ) );
	}
}
