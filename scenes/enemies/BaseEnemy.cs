using Godot;
using System;
using System.Collections.Generic;

public class BaseEnemy : KinematicBody2D
{
    [Signal]
    delegate bool EnemyShooting();

    [Signal]
    delegate void EnemyBoom();

    [Export] public PackedScene EnemyBullet;
    [Export] public int Speed;
    [Export] public int Health;
    [Export] public float Radar_Radius;

    public PathFollow2D Follow;
    private Random _random;

    public RigidBody2D Target;
    
    public override void _Ready()
    {
        _random = new Random();
        GetNode<Sprite>("Explosion").Hide();
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        Follow.SetOffset(Follow.GetOffset() + Speed * delta);
        if (Follow.UnitOffset > 1)
        {
            QueueFree();
        }
    }

    public void SetupPath(PathFollow2D pathFollow2D)
    {
        Follow = pathFollow2D;
    }

    public void SetUpRadar(float radius)
    {
        CircleShape2D c = new CircleShape2D();
        c.Radius = radius;
        GetNode<CollisionShape2D>("Radar/CollisionShape2D").Shape = c;
    }

    public void BaseEnemyExplode()
    {
        Layers = 0;
        GetNode<Sprite>("Sprite").Hide();
        GetNode<Sprite>("Explosion").Show();
        GetNode<AnimationPlayer>("Explosion/AnimationPlayer").Play("explosion");
        EmitSignal("EnemyBoom", 5);
        GetNode<AudioStreamPlayer>("Explode").Play();
    }
    
    private void _on_AnimationPlayer_animation_finished(String anim_name)
    {
        QueueFree();
    }
    
    private void _on_Shield_body_entered(object body)
    {
        if (body is Rock rock)
        {
            if (rock.IsInGroup("rocks"))
            {
                rock.GetNode<AudioStreamPlayer>("impact").Play();
                rock.Explode(true);
            }
        }
    }
}
