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
        if (body is Player_v2 player)
        {
            GD.Print("enemy_green sees player");
            Target = player;
            GetNode<Timer>("Timer").Start();
        }
//        else if (body is Rock rock && Target == null)
//        {
//            Target = rock;
//            GetNode<Timer>("Timer").Start();
//        }
    }
    
    private void _on_Radar_body_exited(Godot.Object body)
    {
        if (body is Player_v2 player)
        {
            Target = null;   
        }
    }
    
//    private void _on_Enemy_Blue_EnemyShoot(PackedScene bullet, Vector2 pos, float dir, float bulletAngle)
//    {
//        // refactor this to BaseEnemy
//        var eb = (Enemy_Bullet) bullet.Instance();
//        
////        GetNode<Node>("BlueBullets").AddChild(eb);
//        Timer ebTimer = eb.GetNode<Timer>("Timer");
//        ebTimer.WaitTime = 0.5f;
//        ebTimer.Start();
////        eb.Start(pos, dir, _bulletSpeed, "blue", bulletAngle);
////        AddChild(eb);
//    }
    
    private void _on_Timer_timeout()
    {
        if (IsInstanceValid(Target))
        {
            GD.Print("Green shooting");
            Shoot();
        }
        else
        {
            Target = null;
        }
    }

}