using Godot;
using static Godot.GD;
using System;

public class Main : Node
{

    [Export] private PackedScene RockScene;
    
    private Vector2 _screenSize;
    private Random _random;
    public override void _Ready()
    {
        _random = new Random();
        _screenSize = GetViewport().GetVisibleRect().Size;
        GetNode<Player>("Player")._screensize = _screenSize;
        for (int i = 0; i < 3; i++)
        {
            SpawnRock(3);
        }
    }

    private void _on_Player_Shoot(PackedScene bullet, Vector2 pos, float dir)
    {
        Print("bang");
        var b = (Bullet) bullet.Instance();
        b.Start(pos, dir);
        AddChild(b);
    }

    private void SpawnRock(int size, Vector2? pos = null, Vector2? velocity = null)
    {
        if (pos == null)
        {
            GetNode<PathFollow2D>("RockPath/RockSpawn").SetOffset(_random.Next(0, int.MaxValue));
            pos = GetNode<PathFollow2D>("RockPath/RockSpawn").Position;
        }

        if (velocity == null)
        {
            Vector2 temp = new Vector2(1, 0);
            velocity = temp.Rotated(_random.Next(0, 360)) * _random.Next(100, 150);
        }
        Print("rock spawn");
        var r = (Rock) RockScene.Instance();
        Print(velocity);
        r._screensize = _screenSize;
        r.Start((Vector2) pos, (Vector2) velocity, size);
        GetNode<Node>("Rocks").AddChild(r);
    }
}
