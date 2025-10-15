using CBRoomConverter.Models;
using NUnit.Framework.Internal;
using OpenTK.Mathematics;

namespace CBRoomConverter.Tests.Entities;

internal class RotationTests
{
	public RoomList TestRoomList = new();

	[OneTimeSetUp]
	public void Init()
	{
		RoomParser.ParseIni( TestRoomList, TestHelpers.GetTestResource( "EntityTests/RotationTests.ini" ) );
		BlitzParser.ParseBlitz( TestRoomList, TestHelpers.GetTestResource( "EntityTests/RotationTests.bb" ) );

		//Log.Initialize();
	}

	[Test]
	public void TurnEntity()
	{
		var room = TestRoomList.Rooms["TurnEntity"];
		var ent = room.FindEntity( "r\\Objects[2]" );
		Assert.That( ent, Is.Not.Null );

		// To euler angles returns radians, but all our forward facing stuff uses degrees
		Vector3 angles = ent.Rotation.ToEulerAngles() * MathHelper.RadToDeg;
		Assert.That( angles.X, Is.EqualTo( 90f ).Within( 0.1f ) );
		Assert.That( angles.Y, Is.EqualTo( 0f ).Within( 0.1f ) );
		Assert.That( angles.Z, Is.EqualTo( 0f ).Within( 0.1f ) );
	}

	[Test]
	public void RotateEntity()
	{
		var room = TestRoomList.Rooms["RotateEntity"];
		var ent = room.FindEntity( "r\\Objects[2]" );
		Assert.That( ent, Is.Not.Null );

		// To euler angles returns radians, but all our forward facing stuff uses degrees
		Vector3 angles = ent.Rotation.ToEulerAngles() * MathHelper.RadToDeg;
		Assert.That( angles.X, Is.EqualTo( 45f ).Within( 0.1f ) );
		Assert.That( angles.Y, Is.EqualTo( 0f ).Within( 0.1f ) );
		Assert.That( angles.Z, Is.EqualTo( 0f ).Within( 0.1f ) );
	}
}
