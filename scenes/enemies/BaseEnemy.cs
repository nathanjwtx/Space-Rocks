using Godot;
using System;

public class BaseEnemy : KinematicBody2D
{
    [Signal]
    delegate bool Shoot();

    [Export] public PackedScene Bullet;
    [Export] public int Speed;
    [Export] public int Health;

    private PathFollow2D Follow;
    
    
    public override void _Ready()
    {
        Path2D p = GetNode<Path2D>("EnemyPaths/path1");
        Follow = new PathFollow2D();
        p.AddChild(Follow);
        Follow.Loop = false;
        GD.Print("base");
    }

    public override void _Process(float delta)
    {
//        Follow.SetOffset(Follow.GetOffset() + Speed * delta);
           //        Position = Follow.GlobalPosition;
           //        if (Follow.Offset > 1)
           //        {
           //            QueueFree();
           //        }
    }
}
