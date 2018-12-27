using Godot;
using System;

public class Player : RigidBody2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
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
                case States.DEAD;
                    collision2D.Disabled = true;
                    break;
        }

        _state = state;
    }
//    public override void _Process(float delta)
//    {
//        // Called every frame. Delta is time since last frame.
//        // Update game logic here.
//        
//    }
}
