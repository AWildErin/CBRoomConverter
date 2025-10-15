Function FillRoom(r.Rooms)

    Select r\RoomTemplate\Name
        ; Rotates the entity using another objects PYR
        Case "EntityPYR"
            ;[Block]
            r\Objects[2] = LoadMesh_Strict("GFX\map\forest\door_frame.b3d")
            r\Objects[3] = LoadMesh_Strict("GFX\map\forest\door_frame.b3d")


            RotateEntity(r\Objects[2], 45, 0, 90, True)
            RotateEntity(r\Objects[3], EntityPitch(r\Objects[2]) + 45, EntityYaw(r\Objects[2]) + 0, EntityRoll(r\Objects[2]) - 45, True)
            ;[End Block]
        ; Rotates the entity using another objects XYZ
        Case "EntityXYZ"
            ;[Block]
            r\Objects[2] = LoadMesh_Strict("GFX\map\forest\door_frame.b3d")
            r\Objects[3] = LoadMesh_Strict("GFX\map\forest\door_frame.b3d")

            PositionEntity(r\Objects[2], 100, 0, 100)
            PositionEntity(r\Objects[3], EntityX(r\Objects[2]) + 100, EntityY(r\Objects[2]) + 0, EntityX(r\Objects[2]) - 50)
            ;[End Block]
    End Select

End Function