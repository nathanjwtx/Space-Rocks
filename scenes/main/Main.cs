using Godot;
using static Godot.GD;
using System;
using Object = Godot.Object;

public class Main : Node
{

    [Export] private PackedScene RockScene;
    
    private Vector2 _screenSize;
    private Random _random;
    private int _level;
    private int _score;
    private bool _playing;
    
    public override void _Ready()
    {
        _random = new Random();
        _screenSize = GetViewport().GetVisibleRect().Size;
        GetNode<Player>("Player")._screensize = _screenSize;
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        if (_playing && GetNode<Node>("Rocks").GetChildren().Count == 0)
        {
            NewLevel();
        }
    }

    private async void NewGame()
    {
        var rocks = GetNode<Node>("Rocks").GetChildren();
        foreach (var rock in rocks)
        {
            Rock r = (Rock) rock;
            r.QueueFree();
        }

        _level = 0;
        _score = 0;
        HUD h = GetNode<HUD>("HUD");
        h.UpdateScore(_score);
        GetNode<Player>("Player").Start();
        h.ShowMessage("Get Ready!");
        Timer mt = GetNode<Timer>("HUD/MessageTimer");
        await ToSignal(mt, "timeout");
        _playing = true;
        NewLevel();
    }

    private void NewLevel()
    {
        _level += 1;
        GetNode<HUD>("HUD").ShowMessage($"Wave {_level}");
        for (int i = 0; i < _level * 3; i++)
        {
            SpawnRock(3);
        }
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
        if (size <= 1)
        {
            return;
        }

        for (int i = -1; i < 2; i+= 2)
        {
            Vector2 dir = (pos - GetNode<Player>("Player").Position).Normalized().Tangent() * i;
            Vector2 newPos = pos + dir * radius;
            Vector2 newVel = dir * vel.Length() * 1.5f;
            SpawnRock(size - 1, newPos, newVel);
        }
    }
    
    private void _on_Player_Shoot(PackedScene bullet, Vector2 pos, float dir)
    {
        var b = (Bullet) bullet.Instance();
        b.Start(pos, dir);
        AddChild(b);
    }
    
    private void _on_HUD_StartGame()
    {
        NewGame();
    }
}
