using Godot;
using System;

public class PowerUp : RigidBody2D
{

    private bool _startVanish = false;
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
      
    private void _on_SolidTimer_timeout()
    {
        _startVanish = true;
    }
    
    public override void _Process(float delta)
    {
        var p = GetNode<Sprite>("Sprite");
        GD.Print(p.Modulate.a);
        GD.Print(_startVanish);
        if (p.Modulate.a > 0.25 && _startVanish)
        {
            p.Modulate = new Color(p.Modulate.r, p.Modulate.g, p.Modulate.b, p.Modulate.a - delta);    
        }
        else if (p.Modulate.a <= 0.25 && _startVanish)
        {
            QueueFree();
        }
        
    }
}




