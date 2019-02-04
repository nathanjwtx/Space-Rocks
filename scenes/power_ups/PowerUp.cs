using Godot;
using System;
using System.Linq;

public class PowerUp : RigidBody2D
{

    private Random _random;
    private bool _startVanish = false;
    private Vector2 _screenSize;
    public string PowerUpType;

    private System.Collections.Generic.Dictionary<string, string> _powerUpSprites = new System.Collections.Generic.Dictionary<string, string>();

   
    
    public override void _Ready()
    {
        _powerUpSprites.Add("shield", "res://assets/power_ups/powerupGreen_shield.png");
        _powerUpSprites.Add("repair","res://assets/power_ups/powerupGreen_bolt.png");
        _screenSize = GetViewport().GetVisibleRect().Size;
        _random = new Random();
    }

    public void LoadPowerUp()
    {
        Sprite powerup = GetNode<Sprite>("Sprite");
        var k = _powerUpSprites.Keys.ToList()[_random.Next(0, _powerUpSprites.Count)];
        PowerUpType = k;
        powerup.Texture = (Texture) GD.Load(_powerUpSprites[k]);
        RectangleShape2D square = new RectangleShape2D();
        square.Extents = new Vector2(40, 40);
        GetNode<CollisionShape2D>("CollisionShape2D").Shape = square;
        powerup.Show();
        Position = new Vector2(_random.Next(25, (int) _screenSize.x - 20), _random.Next(25, (int) _screenSize.y - 20));
    }
      
    private void _on_SolidTimer_timeout()
    {
        _startVanish = true;
    }
    
    public override void _Process(float delta)
    {
        var p = GetNode<Sprite>("Sprite");
        if (p.Modulate.a > 0.25 && _startVanish)
        {
            p.Modulate = new Color(p.Modulate.r, p.Modulate.g, p.Modulate.b, p.Modulate.a - delta);    
        }
        else if (p.Modulate.a <= 0.25 && _startVanish)
        {
            QueueFree();
        }
        
    }

    public void PlayCollected()
    {
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
    }
}




