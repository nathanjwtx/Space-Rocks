using Godot;
using System;

public class Player : RigidBody2D
{
    [Export] private int Engine_Power;
    [Export] private int Spin_Power;
    
    Vector2 _thrust;
    int _rotationDir = 0;
    
    private enum States
    {
        INIT, ALIVE, INVULNERABLE, DEAD
    }

    private Enum _state = null;

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        ChangeState(States.ALIVE);
    }

    public override void _Process(float delta)
    {
        GetInput();
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        SetAppliedForce(_thrust.Rotated(Rotation));
        SetAppliedTorque(Spin_Power * _rotationDir);
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
//        else
//        {
//            _thrust = new Vector2(0, 0);
//        }

        _rotationDir = 0;
        if (Input.IsActionPressed("rotate_left"))
        {
            _rotationDir -= 1;
        }

        if (Input.IsActionPressed("rotate_right"))
        {
            _rotationDir += 1;
        }
    }
}
