namespace CBRoomConverter.Enums;

// Named to keep inline with the native enum
public enum ESCPRoomType
{
	None,
	EndRoom = 1,
	TwoWay = 2,
	TwoWayCorner = 3,
	ThreeWay = 4,
	FourWay = 5,
	Checkpoint = 255 // Checkpoints are actually Room2, but we use this for GridType checks
}
