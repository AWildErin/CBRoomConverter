namespace CBRoomConverter.Models;

internal class RoomList
{
	public Dictionary<string, string> RoomAmbience { get; set; } = new();
	public List<Room> Rooms {  get; set; } = new();
}
