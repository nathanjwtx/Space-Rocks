using Godot;
using static Godot.GD;
using System;
using System.Collections.Generic;
using Object = Godot.Object;

public class Main : Node
{

    [Export] private PackedScene RockScene;
    
    private Vector2 _screenSize;
    private Random _random;
    private int _level;
    private int _score;
    private bool _playing;
    private int _hits;
    
    private List<string> backgrounds = new List<string>
    {
        "res://assets/backgrounds/level1.jpg",
        "res://assets/backgrounds/level1.jpg",
        "res://assets/backgrounds/level2-1.jpg",
        "res://assets/backgrounds/level3.jpg",
        "res://assets/backgrounds/level4.jpg",
        "res://assets/backgrounds/level5.jpg"
    };
    
    public override void _Ready()
    {
        _random = new Random();
        _screenSize = GetViewport().GetVisibleRect().Size;
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        if (_playing && GetNode<Node>("Rocks").GetChildren().Count == 0)
        {
            _level++;
            NewLevel();
        }
    }

    private void Reset()
    {
        _level = 0;
        _hits = 0;
        Player_v2 p = GetNode<Player_v2>("Player");
//        p.GlobalPosition = new Vector2(_screenSize.x / 2, _screenSize.y / 2);
        var rocks = GetNode<Node>("Rocks").GetChildren();
        foreach (var rock in rocks)
        {
            Rock r = (Rock) rock;
            r.QueueFree();
        }
    }
    private async void NewGame()
    {
        Reset();
        _score = 0;
        HUD h = GetNode<HUD>("HUD");
        h.UpdateScore(_score);
        Player_v2 p = GetNode<Player_v2>("Player");
        GetNode<Sprite>("Player/Ship").Show();
        GetNode<Sprite>("Player/Engine").Show();
        p.Start();
        h.ShowMessage("Get Ready!");
        Timer mt = GetNode<Timer>("HUD/MessageTimer");
        await ToSignal(mt, "timeout");
        _playing = true;
        NewLevel();
    }

    private void NewLevel()
    {
        int level;
        if (!_playing)
        {
            level = 0;
        }
        else
        {
            level = _level;
        }
        GetNode<Sprite>("Background").Texture = (Texture) Load(backgrounds[level]);
        GetNode<HUD>("HUD").ShowMessage($"Wave {level}");
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
        r.Start((Vector2) pos, (Vector2) velocity, size, _random.Next(0, 4));
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
            Vector2 dir = (pos - GetNode<Player_v2>("Player").Position).Normalized().Tangent() * i;
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
    
    private void _on_Player_body_entered(Object body)
    {
        Player_v2 p = GetNode<Player_v2>("Player");
        if (body is Rock b)
        {
            _hits++;
            if (_hits == 4)
            {
                p.GetNode<Sprite>("Damage1").Hide();
                p.GetNode<Sprite>("Damage2").Hide();
                p.GetNode<Sprite>("Damage3").Hide();
                p.GetNode<Sprite>("Explosion").Show();
                p.GetNode<AnimationPlayer>("Explosion/AnimationPlayer").Play("explosion");
                p.ChangeState(Player_v2.States2.DEAD);
            }
            else
            {
                b.GetNode<AudioStreamPlayer>("impact").Play();
                GetNode<Sprite>($"Player/Damage{_hits}").Show();
            }
        }
    }
    
    private void _on_Player_Dead()
    {
        _playing = false;
        Reset();
        HUD hud = GetNode<HUD>("HUD");
//        hud.GameOver();
        GetTree().ChangeScene("res://scenes/game_over/GameOverPage.tscn");
    }
    
}
