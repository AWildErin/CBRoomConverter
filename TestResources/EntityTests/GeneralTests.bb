Function FillRoom(r.Rooms)

    Select r\RoomTemplate\Name
        Case "EntityParent"
            ;[Block]
            parent = LoadMesh_Strict("GFX\map\forest\door_frame.b3d")
            child = LoadMesh_Strict("GFX\map\forest\door_frame.b3d")

            EntityParent(child, parent)
            ;[End Block]
    End Select

End Function