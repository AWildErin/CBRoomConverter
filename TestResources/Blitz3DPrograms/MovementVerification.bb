; A quick program to verify how certain commands work
  
Graphics3D 800,600
SetBuffer BackBuffer()
camera = CreateCamera()
CameraViewport(camera,0,0,800,600)

light=CreateLight()
cube=CreateCube()
TurnEntity(cube,45,0,0)
PositionEntity(cube,0,100,0)
MoveEntity(cube,0,100,0)

While Not KeyHit(1)	
    UpdateWorld
    RenderWorld

    Text 335,500, "X:" + Str(EntityX(cube)) + " Y:" + Str(EntityY(cube)) + " Z:" + Str(EntityZ(cube))

    Flip
Wend
End