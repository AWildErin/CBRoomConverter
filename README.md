# CBRoomConverter
Converts rooms.ini to an intermediate json format used by the game.
Usage: cbroomconverter.exe -i rooms.ini -o rooms.json

## Notes
- Room cases must be wrapped in `;[Block]` and `;[EndBlock]`.
- Function calls must have parentheses. Function calls without them will be ignored. I would like this to not be the case in future, but I don't want to spend time reworking all the regex.
- For loops and conditionals are not parsed, I would also like this in the future.

Example:
```bb
Case "room2_4"
	;[Block]
	r\Objects[6] = CreatePivot()
	PositionEntity(r\Objects[6], r\x + 640.0 * RoomScale, 8.0 * RoomScale, r\z - 896.0 * RoomScale)
	EntityParent(r\Objects[6], r\obj)
	;[End Block]
```

## Developer notes
- B3D -> Unreal Vectors must be ZXY