using CBRoomConverter.Models;
using NUnit.Framework.Internal;
using OpenTK.Mathematics;

namespace CBRoomConverter.Tests.Entities;

internal class AccessorTests
{
	public RoomList TestRoomList = new();

	[OneTimeSetUp]
	public void Init()
	{
		RoomParser.ParseIni( TestRoomList, TestHelpers.GetTestResource( "EntityTests/AccessorTests.ini" ) );
		BlitzParser.ParseBlitz( TestRoomList, TestHelpers.GetTestResource( "EntityTests/AccessorTests.bb" ) );

		//Log.Initialize();
	}

	[Test]
	public void EntityPYR()
	{
		var room = TestRoomList.Rooms["EntityPYR"];
		var ent = room.FindEntity( "r\\Objects[3]" );
		Assert.That( ent, Is.Not.Null );

		// To euler angles returns radians, but all our forward facing stuff uses degrees
		Vector3 angles = ent.Rotation.ToEulerAngles() * MathHelper.RadToDeg;
		Assert.That( angles.X, Is.EqualTo( 90f ).Within( 0.1f ) );
		Assert.That( angles.Y, Is.EqualTo( 0f ).Within( 0.1f ) );
		Assert.That( angles.Z, Is.EqualTo( 45f ).Within( 0.1f ) );
	}

	[Test]
	public void EntityXYZ()
	{
		var room = TestRoomList.Rooms["EntityXYZ"];
		var ent = room.FindEntity( "r\\Objects[3]" );
		Assert.That( ent, Is.Not.Null );

		Assert.That( ent.Position.X, Is.EqualTo( 200f ).Within( 0.1f ) );
		Assert.That( ent.Position.Y, Is.EqualTo( 0f ).Within( 0.1f ) );
		Assert.That( ent.Position.Z, Is.EqualTo( 50f ).Within( 0.1f ) );
	}
}
