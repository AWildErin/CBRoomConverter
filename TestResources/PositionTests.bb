Function FillRoom(r.Rooms)

    Select r\RoomTemplate\Name
        Case "PositionEntity"
            ;[Block]
            r\Objects[2] = LoadMesh_Strict("GFX\map\forest\door_frame.b3d")

            PositionEntity(r\Objects[2], 0, 100, 0, True)
            ;[End Block]
        Case "MoveEntity"
            ;[Block]
            r\Objects[2] = LoadMesh_Strict("GFX\map\forest\door_frame.b3d")

            TurnEntity(r\Objects[2], 45, 0, 0, True)
            PositionEntity(r\Objects[2], 0, 100, 0, True)
            MoveEntity(r\Objects[2], 0, 100, 0)
            ;[End Block]
        Case "TranslateEntity"
            ;[Block]
            r\Objects[2] = LoadMesh_Strict("GFX\map\forest\door_frame.b3d")

            TurnEntity(r\Objects[2], 45, 0, 0, True)
            PositionEntity(r\Objects[2], 0, 100, 0, True)
            TranslateEntity(r\Objects[2], 0, 100, 0)
            ;[End Block]
    End Select

End Function