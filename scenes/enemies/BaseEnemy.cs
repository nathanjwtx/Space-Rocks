using Godot;
using System;
using System.Collections.Generic;

public class BaseEnemy : KinematicBody2D
{
    [Signal]
    delegate bool Shoot();

    [Export] public PackedScene Bullet;
    [Export] public int Speed;
    [Export] public int Health;
    [Export] public float Radar_Radius;

    public PathFollow2D Follow;
    private Random _random;

    public RigidBody2D Target;
    
    public override void _Ready()
    {
        _random = new Random();
        // remember to update the random number if new paths added
        Path2D p = GetNode<Path2D>($"EnemyPaths/path{_random.Next(1, 5)}");
        Follow = new PathFollow2D();
        p.AddChild(Follow);
        Follow.Loop = false;
    }

    public void SetUpRadar(float radius)
    {
        CircleShape2D c = new CircleShape2D();
        c.Radius = radius;
        GetNode<CollisionShape2D>("Radar/CollisionShape2D").Shape = c;
    }

}
