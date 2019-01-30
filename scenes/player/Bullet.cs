using Godot;
using System;
using System.Runtime.CompilerServices;

public class Bullet : Area2D
{
    [Export] private int Speed;

    private Vector2 _velocity;

    public void Start(Vector2 pos, float dir)
    {
        Position = pos;
        Rotation = dir;
        _velocity = new Vector2(Speed, 0).Rotated(dir);
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
	
	private void _on_Area2D_body_entered(Godot.Object body)
    {
        if (body is Rock b)
        {
            if (b.IsInGroup("rocks"))
            {
                b.Explode(true);
                QueueFree();
            }
        }
        else if (body is BaseEnemy baseEnemy)
        {
            baseEnemy.BaseEnemyExplode();
            QueueFree();
        }
	}
}
