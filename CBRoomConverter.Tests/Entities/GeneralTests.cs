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

internal class GeneralTests
{
	public RoomList TestRoomList = new();

	[OneTimeSetUp]
	public void Init()
	{
		RoomParser.ParseIni( TestRoomList, TestHelpers.GetTestResource( "EntityTests/GeneralTests.ini" ) );
		BlitzParser.ParseBlitz( TestRoomList, TestHelpers.GetTestResource( "EntityTests/GeneralTests.bb" ) );

		//Log.Initialize();
	}

	[Test]
	public void EntityParent()
	{
		var room = TestRoomList.Rooms["EntityParent"];
		var parent = room.FindEntity( "parent" );
		var child = room.FindEntity( "child" );
		Assert.That( parent, Is.Not.Null );
		Assert.That( child, Is.Not.Null );

		Assert.That( child.Parent, Is.EqualTo( parent ) );
		Assert.That( parent.ChildEntities.Contains( child ), Is.True );
	}
}
