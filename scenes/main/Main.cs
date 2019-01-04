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
        var r = (Rock) RockScene.Instance();
        r._screensize = _screenSize;
        r.Start((Vector2) pos, (Vector2) velocity, size);
        GetNode<Node>("Rocks").AddChild(r);
        r.Connect("Boom", this, "_on_Rock_Boom");
    }

    private void _on_Rock_Boom(int size, float radius, Vector2 pos, Vector2 vel)
    {
//        Print("Boom");
        if (size <= 1)
        {
            return;
        }

        for (int i = -1; i < 2; i++)
        {
            Vector2 dir = (pos - GetNode<Player>("Player").Position).Normalized().Tangent() * i;
            Vector2 newPos = pos + dir * radius;
            Vector2 newVel = dir * vel.Length() * 1.5f;
            SpawnRock(size - 1, newPos, newVel);
        }
    }
}
