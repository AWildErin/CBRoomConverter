Function FillRoom(r.Rooms)

    Select r\RoomTemplate\Name
        Case "TurnEntity"
            ;[Block]
            r\Objects[2] = LoadMesh_Strict("GFX\map\forest\door_frame.b3d")

            TurnEntity(r\Objects[2], 45, 0, 0, True)
            TurnEntity(r\Objects[2], 45, 0, 0, True)
            ;[End Block]
        Case "RotateEntity"
            ;[Block]
            r\Objects[2] = LoadMesh_Strict("GFX\map\forest\door_frame.b3d")

            RotateEntity(r\Objects[2], 45, 0, 0, True)
            ;[End Block]
    End Select

End Function