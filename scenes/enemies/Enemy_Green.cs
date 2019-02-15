using Godot;
using System;

public class Enemy_Green : BaseEnemy
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
//            EmitSignal("EnemyShoot", bullet, pos, dir.Angle(), dir.Normalized());
            EmitSignal("EnemyShooting", bullet, pos, dir, _bulletSpeed, "green");
        }
    }
    
    private void _on_Radar_body_entered(Godot.Object body)
    {
        if (body is PowerUp powerUp)
        {
            GD.Print("enemy_green sees powerup");
            Target = powerUp;
            GetNode<Timer>("Timer").Start();
        }
        else if (body is Rock rock && Target == null)
        {
            GD.Print("Green shooting a rock");
            Target = rock;
            GetNode<Timer>("Timer").Start();
        }
    }
    
    private void _on_Radar_body_exited(Godot.Object body)
    {
        if (body is PowerUp powerUp)
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