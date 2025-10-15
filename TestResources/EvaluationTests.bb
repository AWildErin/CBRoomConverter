Function FillRoom(r.Rooms)

    Select r\RoomTemplate\Name
        Case "RoomScale"
            ;[Block]
            r\Objects[2] = LoadMesh_Strict("GFX\map\forest\door_frame.b3d")

            PositionEntity(r\Objects[2], 100 * RoomScale, 0 * RoomScale, 100 * RoomScale)
            ;[End Block]
        Case "RoomXYZ"
            ;[Block]
            r\Objects[2] = LoadMesh_Strict("GFX\map\forest\door_frame.b3d")

            PositionEntity(r\Objects[2], r\x - 832.0, 0.7, r\z + 160.0, True)
            ;[End Block]
    End Select

End Function