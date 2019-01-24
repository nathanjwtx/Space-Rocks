using Godot;
using System;

public class Enemy_Green : BaseEnemy
{

    public override void _Ready()
    {
        base._Ready();
        GD.Print("ready1");
        
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
