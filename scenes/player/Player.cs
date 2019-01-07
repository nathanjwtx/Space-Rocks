using Godot;
using System;

public class Player : RigidBody2D
{
    [Export] private int Engine_Power;
    [Export] private int Spin_Power;
    [Export] private PackedScene Bullet;
    [Export] private float FireRate;
    
    [Signal]
    delegate void Shoot();

    [Signal]
    delegate void LivesChanged();

    private int _lives;
    
    private Vector2 _thrust;
    public Vector2 _screensize;
    private int _rotationDir;
    private bool CanShoot = true;
    
    private enum States
    {
        INIT, ALIVE, INVULNERABLE, DEAD
    }

    private Enum _state = null;

    public override void _Ready()
    {
        ChangeState(States.ALIVE);
        _screensize = GetViewport().GetVisibleRect().Size;
        Position = new Vector2(_screensize.x / 2, _screensize.y / 2);
        GetNode<Timer>("Timer").WaitTime = FireRate;
    }

    public void Start()
    {
        Visible = true;
        Lives = 3;
        ChangeState(States.ALIVE);
    }
    
    public int Lives
    {
        get => _lives;
        set
        {
            _lives = value;
            EmitSignal("LivesChanged", _lives);
        }
    }

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
        Transform2D wibble;
        if (xform.Origin.x > _screensize.x)
        {
            origin = new Vector2(0, xform.Origin.y);
            wibble = new Transform2D(xform.x, xform.y, origin);
            physics_state.SetTransform(wibble);
        }
        if (xform.Origin.x < 0)
        {
            origin = new Vector2(_screensize.x, xform.Origin.y);
            wibble = new Transform2D(xform.x, xform.y, origin);
            physics_state.SetTransform(wibble);
        }

        if (xform.Origin.y > _screensize.y)
        {
            origin = new Vector2(xform.Origin.x, 0);
            wibble = new Transform2D(xform.x, xform.y, origin);
            physics_state.SetTransform(wibble);
        }

        if (xform.Origin.y < 0)
        {
            origin = new Vector2(xform.Origin.x, _screensize.y);
            wibble = new Transform2D(xform.x, xform.y, origin);
            physics_state.SetTransform(wibble);
        }
    }

    public void ChangeState(Enum state)
    {
        CollisionShape2D collision2D = GetNode<CollisionShape2D>("CollisionShape2D");
        switch (state)
        {
                case States.INIT:
                    collision2D.Disabled = true;
                    break;
                case States.ALIVE:
                    collision2D.Disabled = false;
                    break;
                case States.INVULNERABLE:
                    collision2D.Disabled = true;
                    break;
                case States.DEAD:
                    collision2D.Disabled = true;
                    break;
        }

        _state = state;
    }

    public void GetInput()
    {
        _thrust = new Vector2(0, 0);
        if (_state.Equals(States.DEAD) || _state.Equals(States.INIT))
        {
            return;
        }

        if (Input.IsActionPressed("thrust"))
        {
            _thrust = new Vector2(Engine_Power, 0);
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
        if (_state.Equals(States.INVULNERABLE))
        {
            return;
        }

        var muzzle = GetNode<Node2D>("Muzzle");
        var bullet = Bullet;
        EmitSignal("Shoot", bullet, muzzle.GlobalPosition, Rotation);
        CanShoot = false;
        GetNode<Timer>("Timer").Start();
    }
    
    private void _on_Timer_timeout()
    {
        CanShoot = true;
    }
}



