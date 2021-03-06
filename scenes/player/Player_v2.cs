using Godot;
using System;

public class Player_v2 : RigidBody2D
{
    [Export] private int Engine_Power;
    [Export] private int Spin_Power;
    [Export] private PackedScene Bullet;
    [Export] private float FireRate;
    
    [Signal]
    delegate void Shoot();

    [Signal]
    delegate void LivesChanged();
    
    [Signal]
    delegate void Dead();

    private int _lives;
    
    private Vector2 _thrust;
    public Vector2 _screensize;
    private Vector2 _startPosition;
    private int _rotationDir;
    private bool CanShoot = true;
    public bool Shielded;
    
    public enum States2
    {
        INIT, ALIVE, INVULNERABLE, DEAD, SHIELDED
    }

    private Enum _state;

    public override void _Ready()
    {
        ChangeState(States2.ALIVE);
        _screensize = GetViewport().GetVisibleRect().Size;
        _startPosition = new Vector2(_screensize.x / 2, _screensize.y / 2);
        GetNode<Timer>("Timer").WaitTime = FireRate;
        GetNode<Sprite>("Ship").Show();
        GetNode<Sprite>("Engine").Show();
        GetNode<Sprite>("Damage1").Hide();
        GetNode<Sprite>("Damage2").Hide();
        GetNode<Sprite>("Damage3").Hide();
        GetNode<Sprite>("Explosion").Hide();
        GetNode<Sprite>("Thrust").Hide();
//        GetNode<Particles2D>("Exhaust").Hide();
//        GetNode<Particles2D>("DamagedExhaust").Hide();
        GetNode<Area2D>("Shield").Hide();
    }

    public void Start()
    {
        Visible = true;
        GlobalPosition = _startPosition;
        ChangeState(States2.ALIVE);
    }
    
//    public int Lives
//    {
//        get => _lives;
//        set
//        {
//            _lives = value;
//            EmitSignal("LivesChanged", _lives);
//        }
//    }

    public override void _Process(float delta)
    {
        GetInput();
    }

    public override void _IntegrateForces(Physics2DDirectBodyState physics_state)
    {
        base._IntegrateForces(physics_state);
        SetAppliedForce(_thrust.Rotated(Rotation));
        SetAppliedTorque(Spin_Power * _rotationDir);
        Transform2D xform = physics_state.GetTransform();
        Vector2 origin;
        Transform2D newTransform;
        if (xform.origin.x > _screensize.x)
        {
            origin = new Vector2(0, xform.origin.y);
            newTransform = new Transform2D(xform.x, xform.y, origin);
            physics_state.SetTransform(newTransform);
        }
        if (xform.origin.x < 0)
        {
            origin = new Vector2(_screensize.x, xform.origin.y);
            newTransform = new Transform2D(xform.x, xform.y, origin);
            physics_state.SetTransform(newTransform);
        }

        if (xform.origin.y > _screensize.y)
        {
            origin = new Vector2(xform.origin.x, 0);
            newTransform = new Transform2D(xform.x, xform.y, origin);
            physics_state.SetTransform(newTransform);
        }

        if (xform.origin.y < 0)
        {
            origin = new Vector2(xform.origin.x, _screensize.y);
            newTransform = new Transform2D(xform.x, xform.y, origin);
            physics_state.SetTransform(newTransform);
        }
    }

    public void ChangeState(Enum state)
    {
        CollisionPolygon2D collision2D = GetNode<CollisionPolygon2D>("CollisionPolygon2D");
        Sprite ship = GetNode<Sprite>("Ship");
        Sprite engine = GetNode<Sprite>("Engine");
        switch (state)
        {
                case States2.INIT:
                //                    collision2D.Disabled = true;
                    collision2D.SetDeferred("disabled", true);
                    ship.Modulate = new Color(ship.Modulate.r, ship.Modulate.g, ship.Modulate.b, 0.5f);
                    engine.Modulate = new Color(engine.Modulate.r, engine.Modulate.g, engine.Modulate.b, 0.5f);
                    break;
                case States2.ALIVE:
//                    collision2D.Disabled = false;
                    collision2D.SetDeferred("disabled", false);
                    Shielded = false;
                    ship.Modulate = new Color(ship.Modulate.r, ship.Modulate.g, ship.Modulate.b);
                    engine.Modulate = new Color(engine.Modulate.r, engine.Modulate.g, engine.Modulate.b);
                    break;
                case States2.INVULNERABLE:
//                    collision2D.Disabled = true;
                    collision2D.SetDeferred("disabled", true);
                    ship.Modulate = new Color(ship.Modulate.r, ship.Modulate.g, ship.Modulate.b, 0.5f);
                    engine.Modulate = new Color(engine.Modulate.r, engine.Modulate.g, engine.Modulate.b, 0.5f);
                    GetNode<Timer>("InvTimer").Start();
                    break;
                case States2.DEAD:
//                    collision2D.Disabled = true;
                    collision2D.SetDeferred("disabled", true);
                    ship.Hide();
                    engine.Hide();
                    LinearVelocity = new Vector2(0, 0);
                    break;
        }
        _state = state;
    }

    public void GetInput()
    {
        _thrust = new Vector2(0, 0);
        if (_state.Equals(States2.DEAD) || _state.Equals(States2.INIT))
        {
            return;
        }
        if (Input.IsActionPressed("thrust") && Global.Hits < 3)
        {
            GetNode<Sprite>("Thrust").Show();            
            GetNode<Particles2D>("Exhaust").Emitting = true;
            _thrust = new Vector2(Engine_Power, 0);
        }
        else if (Input.IsActionPressed("thrust") && Global.Hits == 3)
        {
            GetNode<Particles2D>("DamagedExhaust").Emitting = true;
            _thrust = new Vector2(Engine_Power / 2, 0);
        }

        if (Input.IsActionJustReleased("thrust"))
        {
            GetNode<Sprite>("Thrust").Hide();
            GetNode<Particles2D>("Exhaust").Emitting = false;
            GetNode<Particles2D>("DamagedExhaust").Emitting = false;
        }

        _rotationDir = 0;
        if (Input.IsActionPressed("rotate_left"))
        {
            _rotationDir -= 1;
        }

        if (Input.IsActionPressed("rotate_right"))
        {
            _rotationDir += 1;
        }

        if (Input.IsActionPressed("fire") && CanShoot)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (_state.Equals(States2.INVULNERABLE) && !Shielded)
        {
            return;
        }

        var muzzle = GetNode<Node2D>("Muzzle");
        var bullet = Bullet;
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
        EmitSignal("Shoot", bullet, muzzle.GlobalPosition, Rotation);
        CanShoot = false;
        GetNode<Timer>("Timer").Start();
    }
    
    private void _on_Timer_timeout()
    {
        CanShoot = true;
    }
    
    private void _on_InvTimer_timeout()
    {
        ChangeState(States2.ALIVE);
        GetNode<Area2D>("Shield").Hide();
        Shielded = false;
    }
    
    private void _on_AnimationPlayer_animation_finished(String anim_name)
    {
        GetNode<Sprite>("Explosion").Hide();
        Hide();
        EmitSignal("Dead");
    }
    
    private void _on_Shield_body_entered(Godot.Object body)
    {
        if (body is Rock b && Shielded)
        {
            if (b.IsInGroup("rocks"))
            {
                b.GetNode<AudioStreamPlayer>("impact").Play();
                b.Explode(true);
            }
        }
    }
}
