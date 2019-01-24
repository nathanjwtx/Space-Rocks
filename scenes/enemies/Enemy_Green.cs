using Godot;
using System;

public class Enemy_Green : BaseEnemy
{
    private PathFollow2D Follow;

    public override void _Ready()
    {
        Path2D p = GetNode<Path2D>("EnemyPaths/path1");
        Follow = new PathFollow2D();
        p.AddChild(Follow);
        Follow.Loop = false;
        GD.Print("ready1");
    }

    public void Test()
    {
        Path2D p = GetNode<Path2D>("EnemyPaths/path1");
        Follow = new PathFollow2D();
        p.AddChild(Follow);
        Follow.Loop = false;
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
        GD.Print(Follow.UnitOffset);
        if (Follow.UnitOffset > 1)
        {
            QueueFree();
        }
    }
}
