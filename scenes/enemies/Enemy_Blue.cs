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
//        GD.Print("blue");
        SetUpRadar(Radar_Radius);
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
        if (Follow.UnitOffset > 1)
        {
            QueueFree();
        }
    }

    private void Shoot()
    {
//        GD.Print(IsInstanceValid(Target));
        if (Target != null && IsInstanceValid(Target))
        {
            Vector2 dir = Target.GlobalPosition - GlobalPosition;
            Vector2 pos = new Vector2(0, 0);
//        float f = _rand.Next((int) -0.1, (int) 0.11);
//        dir = dir.Rotated(f);
            var bullet = EnemyBullet;
            EmitSignal("EnemyShoot", bullet, pos, dir.Angle());    
        }
    }
    
    private void _on_Radar_body_entered(Godot.Object body)
    {
        if (body is Player_v2 player)
        {
            GD.Print("p");
            Target = player;
            GetNode<Timer>("Timer").Start();
        }
        else if (body is Rock rock && Target == null)
        {
            Target = rock;
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
    
    private void _on_Enemy_Blue_EnemyShoot(PackedScene bullet, Vector2 pos, float dir)
    {
        // refactor this to BaseEnemy
        var eb = (Enemy_Bullet) bullet.Instance();
        Timer ebTimer = eb.GetNode<Timer>("Timer");
        ebTimer.WaitTime = 0.5f;
        ebTimer.Start();
        eb.Start(pos, dir, _bulletSpeed);
        AddChild(eb);
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