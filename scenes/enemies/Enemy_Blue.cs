using Godot;
using System;

public class Enemy_Blue : BaseEnemy
{

    public override void _Ready()
    {
        base._Ready();
        GD.Print("blue");
        SetUpRadar(Radar_Radius);
    }
    
    public override void _Process(float delta)
    {
        base._Process(delta);   
        Control(delta);
    }

    public void Control(float delta)
    {
        Follow.SetOffset(Follow.GetOffset() + Speed * delta);
        Position = Follow.GlobalPosition;
        if (Follow.UnitOffset > 1)
        {
            QueueFree();
        }
    }
}
