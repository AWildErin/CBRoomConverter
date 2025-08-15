# CBRoomConverter
Converts rooms.ini to an intermediate json format used by the game.
Usage: cbroomconverter.exe -i rooms.ini -o rooms.json

## Notes
To use entities from Blitz, you must have `;[Block]` and `;[EndBlock]` at the start and end of your rooms case.

Example:
```bb
Case "room2_4"
	;[Block]
	r\Objects[6] = CreatePivot()
	PositionEntity(r\Objects[6], r\x + 640.0 * RoomScale, 8.0 * RoomScale, r\z - 896.0 * RoomScale)
	EntityParent(r\Objects[6], r\obj)
	;[End Block]
```