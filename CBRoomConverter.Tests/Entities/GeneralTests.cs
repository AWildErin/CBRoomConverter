using CBRoomConverter.Models;
using NUnit.Framework.Internal;

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

		int entityIndex = room.GetEntityIndex( child );
		Assert.That( child.Parent, Is.EqualTo( parent ) );
		Assert.That( parent.ChildEntityIndexes.Contains( entityIndex ), Is.True );
	}
}
