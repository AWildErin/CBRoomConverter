using CBRoomConverter.Enums;

namespace CBRoomConverter.Models;

internal class Room
{
	public string? Name { get; set; }
	public string? Description { get; set; }
	public string? Mesh { get; set; }

	public int Commoness { get; set; } = 0;

	public bool Large {  get; set; } = false;
	public bool DisableDecals { get; set; } = false;
	public bool UseVolumeLighting { get; set; } = false;
	public bool DisableOverlapCheck { get; set; } = false;

	public ESCPRoomType RoomType { get; set; } = ESCPRoomType.None;
	public ESCPRoomZone Zone1 { get; set; } = ESCPRoomZone.None;
	public ESCPRoomZone Zone2 { get; set; } = ESCPRoomZone.None;
	public ESCPRoomZone Zone3 { get; set; } = ESCPRoomZone.None;


	// Filled from supplied MapSystem.bb
	public List<Entity> Entities { get; set; } = new List<Entity>();
}
