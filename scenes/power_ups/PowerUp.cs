using Godot;
using System;

public class PowerUp : RigidBody2D
{

    public override void _Ready()
    {

    }

    public void LoadPowerUp()
    {
        GD.Print("called");
        Sprite powerup = GetNode<Sprite>("Sprite");
        powerup.Texture = (Texture) GD.Load("res://assets/power_ups/powerupGreen_shield.png");
        RectangleShape2D square = new RectangleShape2D();
        square.Extents = new Vector2(40, 40);
        GetNode<CollisionShape2D>("CollisionShape2D").Shape = square;
        GD.Print(powerup.Texture.GetWidth());
        powerup.Show();
        Position = new Vector2(200, 200);
    }

//    public override void _Process(float delta)
//    {
//        // Called every frame. Delta is time since last frame.
//        // Update game logic here.
//        
//    }
}
