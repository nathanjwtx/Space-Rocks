using Godot;
using System;
using System.Runtime.CompilerServices;

public class Enemy_Bullet : RigidBody2D
{
    [Export] private int Speed;

    private Vector2 _velocity;

    public string BulletType { get; private set; }

    public void Start(Vector2 pos, Vector2 dir, int speed, string type)
    {
        BulletType = type;
        GlobalPosition = pos;
        Rotation = dir.Angle();
        _velocity = new Vector2(speed, 0).Rotated(dir.Rotated(0f).Angle());
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        Position += _velocity * delta;
    }
    
    private void _on_VisibilityNotifier2D_screen_exited()
    {
        QueueFree();
    }

    private void _on_Timer_timeout()
    {
        QueueFree();
    }
    
//    private void _on_Area2D_body_entered(object body)
//    {
//        GD.Print($"Bullet: {body.GetType().Name}");
//        if (body is Rock rock)
//        {
//            if (rock.IsInGroup("rocks"))
//            {
//                rock.Explode(false);
//                QueueFree();
//            }
//        }
//        else if (body is PowerUp powerUp && BulletType == "green")
//        {
//            GD.Print("I shot a powerup");
//            powerUp.Explode();
//            QueueFree();
//        }
//        else if (body is Player_v2 playerV2)
//        {
//            if (!playerV2.Shielded)
//            {
//                GD.Print("I hit the player");
//            }
//        }
//    }
    
    private void _on_RigidBody2D_body_entered(object body)
    {
//        GD.Print($"Bullet: {body.GetType().Name}");
        if (body is Rock rock)
        {
            if (rock.IsInGroup("rocks"))
            {
                rock.Explode(false);
                QueueFree();
            }
        }
    }
}









