using Godot;
using System;
using System.Runtime.CompilerServices;

public class Enemy_Bullet : Area2D
{
    [Export] private int Speed;

    public int _testSpeed;
    private string _bulletType;

    private Vector2 _velocity;

    public string BulletType { get => _bulletType; set => _bulletType = value; }

    public void Start(Vector2 pos, float dir, int speed, string type)
    {
        BulletType = type;
        GlobalPosition = pos;
        Rotation = dir;
        _velocity = new Vector2(speed, 0).Rotated(dir);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        GlobalPosition += _velocity * delta;
    }
    
    private void _on_VisibilityNotifier2D_screen_exited()
    {
        QueueFree();
    }
	
    private void _on_Area2D_body_entered(Godot.Object body)
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
        else if (body is Player_v2 playerV2)
        {
//            playerV2.
            GD.Print("Hit plater");
        }
    }

    private void _on_Timer_timeout()
    {
        QueueFree();
    }
}
