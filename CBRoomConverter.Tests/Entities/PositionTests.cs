using CBRoomConverter.Models;
using NUnit.Framework.Internal;

namespace CBRoomConverter.Tests.Entities;

internal class PositionTests
{
	public RoomList TestRoomList = new();

	[OneTimeSetUp]
	public void Init()
	{
		RoomParser.ParseIni( TestRoomList, TestHelpers.GetTestResource( "EntityTests/PositionTests.ini" ) );
		BlitzParser.ParseBlitz( TestRoomList, TestHelpers.GetTestResource( "EntityTests/PositionTests.bb" ) );

		//Log.Initialize();
	}

	//[Test]
	//public void SetPosition_String()
	//{
	//	Entity ent = new();
	//	ent.SetPosition( "1", "2", "3" );
	//	Assert.That( ent.Properties["x"], Is.EqualTo( "1" ) );
	//	Assert.That( ent.Properties["y"], Is.EqualTo( "2" ) );
	//	Assert.That( ent.Properties["z"], Is.EqualTo( "3" ) );
	//}

	[Test]
	public void PositionEntity()
	{
		var room = TestRoomList.Rooms["PositionEntity"];
		var ent = room.FindEntity( "r\\Objects[2]" );
		Assert.That( ent, Is.Not.Null );

		Assert.That( ent.Position.X, Is.EqualTo( 0f ).Within( 0.1f ) );
		Assert.That( ent.Position.Y, Is.EqualTo( 100f ).Within( 0.1f ) );
		Assert.That( ent.Position.Z, Is.EqualTo( 0f ).Within( 0.1f ) );
	}

	[Test]
	public void MoveEntity()
	{
		var room = TestRoomList.Rooms["MoveEntity"];
		var ent = room.FindEntity( "r\\Objects[2]" );
		Assert.That( ent, Is.Not.Null );

		Assert.That( ent.Position.X, Is.EqualTo( 0f ).Within( 0.1f ) );
		Assert.That( ent.Position.Y, Is.EqualTo( 170.771f ).Within( 0.1f ) );
		Assert.That( ent.Position.Z, Is.EqualTo( 70.7107f ).Within( 0.1f ) );
	}

	[Test]
	public void TranslateEntity()
	{
		var room = TestRoomList.Rooms["TranslateEntity"];
		var ent = room.FindEntity( "r\\Objects[2]" );
		Assert.That( ent, Is.Not.Null );

		Assert.That( ent.Position.X, Is.EqualTo( 0f ).Within( 0.1f ) );
		Assert.That( ent.Position.Y, Is.EqualTo( 200f ).Within( 0.1f ) );
		Assert.That( ent.Position.Z, Is.EqualTo( 0f ).Within( 0.1f ) );
	}
}
