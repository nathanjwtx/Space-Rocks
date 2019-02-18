using Godot;
using System;

public class Enemy_Blue : BaseEnemy
{

    [Signal]
    delegate void EnemyShoot();
        
    [Export] public int _bulletSpeed;

    private Random _rand;
    
    public override void _Ready()
    {
        base._Ready();
        _rand = new Random();
        SetUpRadar(Radar_Radius);
    }

    private void Shoot()
    {
        if (Target != null && IsInstanceValid(Target))
        {
            Vector2 dir = Target.GlobalPosition - GlobalPosition;
            Vector2 pos = GlobalPosition;
            var bullet = EnemyBullet;
            EmitSignal("EnemyShooting", bullet, pos, dir, _bulletSpeed, "red");
        }
    }
    
    private void _on_Radar_body_entered(Godot.Object body)
    {
        /* Blue enemy only shoots the player */
        if (body is Player_v2 player)
        {
            Target = player;
            GetNode<Timer>("Timer").Start();
        }
    }
    
    private void _on_Radar_body_exited(Godot.Object body)
    {
        if (body is Player_v2 player)
        {
            Target = null;   
        }
    }
    
    private void _on_Timer_timeout()
    {
        if (IsInstanceValid(Target))
        {
            Shoot();
        }
        else
        {
            Target = null;
        }
    }

}