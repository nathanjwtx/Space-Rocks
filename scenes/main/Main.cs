using Godot;
using static Godot.GD;
using System;
using System.Collections.Generic;
using Object = Godot.Object;

public class Main : Node
{

    [Export] private PackedScene RockScene;
    [Export] private PackedScene PowerUpScene;

    private Global _global;
    
    private Vector2 _screenSize;
    private Random _random;
    private int _level;
    private int _score;
    private bool _playing;
    private bool _newGame = true;
    private int _hits;
    private bool _powerUp;
    private Sprite _background;
    private int _prevBackground;
    
    private List<string> backgrounds = new List<string>
    {
        "res://assets/backgrounds/level1.jpg",
        "res://assets/backgrounds/level2.jpg",
        "res://assets/backgrounds/level3.jpg",
        "res://assets/backgrounds/level4.jpg",
        "res://assets/backgrounds/level5.jpg",
        "res://assets/backgrounds/level6.jpg",
        "res://assets/backgrounds/level7.jpg",
        "res://assets/backgrounds/level8.jpg",
        "res://assets/backgrounds/level9.jpg",
        "res://assets/backgrounds/level10.jpg"
    };
    
    private List<string> _enemyShips = new List<string>
    {
        "res://scenes/enemies/Enemy_Red.tscn",
        "res://scenes/enemies/Enemy_Green.tscn"
    };
        
    
    public override void _Ready()
    {
        _global = (Global) GetNode("/root/Global");
        _random = new Random();
        _screenSize = GetViewport().GetVisibleRect().Size;
        _prevBackground = _random.Next(0, 10);
        SetBackground(_prevBackground);
        _newGame = true;
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        if (!_newGame && _playing && GetNode<Node>("Rocks").GetChildren().Count == 0)
        {
            _level++;
            NewLevel();
        }
    }

    private void Reset()
    {
        _level = 0;
        _hits = 0;
        _powerUp = true;
        var rocks = GetNode<Node>("Rocks").GetChildren();
        foreach (var rock in rocks)
        {
            Rock r = (Rock) rock;
            r.QueueFree();
        }
    }
    private async void NewGame()
    {
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
        GetNode<Timer>("EnemySpawnTimer").Start();
        if (!_newGame)
        {
            int r;
            do
            {
                r = _random.Next(0, 10);
            } while (_prevBackground == r);
            _prevBackground = r;
            SetBackground(r);                        
        }
        _newGame = false;
        Player_v2 p = GetNode<Player_v2>("Player");
        if (!Global.HardCore)
        {
            _hits = 0;
            p.GetNode<Sprite>("Damage1").Hide();
            p.GetNode<Sprite>("Damage2").Hide();
            p.GetNode<Sprite>("Damage3").Hide();            
        }
        GetNode<HUD>("HUD").ShowMessage($"Wave {_level + 1}");
        for (int i = 0; i < _level + 1; i++)
        {
            SpawnRock(3);
        }
        Timer t = GetNode<Timer>("PowerUpTimer");
        t.WaitTime = _random.Next(3, 20);
        t.Start();
    }

    private void SetBackground(int level)
    {
        _background.Texture = (Texture) Load(backgrounds[level]);
        var xScale = _screenSize.x / _background.GetTexture().GetWidth();
        var yScale = _screenSize.y / _background.GetTexture().GetHeight();
        _background.Position = new Vector2(0, 0);
        _background.Scale = new Vector2(xScale, yScale);
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
        if (size > 1)
        {
            for (int i = -1; i < 2; i += 2)
            {
                Vector2 dir = (pos - GetNode<Player_v2>("Player").Position).Normalized().Tangent() * i;
                Vector2 newPos = pos + dir * radius;
                Vector2 newVel = dir * vel.Length() * 1.5f;
                SpawnRock(size - 1, newPos, newVel);
            }
        }

        HUD h = GetNode<HUD>("HUD");
        _global.UpdateScore(1);
        h.UpdateScore(_global.Score);
    }
    
    private void _on_Player_Shoot(PackedScene bullet, Vector2 pos, float dir)
    {
        var b = (Bullet) bullet.Instance();
        b.Start(pos, dir);
        AddChild(b);
    }
    
    private void _on_HUD_StartGame()
    {
        Reset();
        _background = GetNode<Sprite>("Background");
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
                b.GetNode<AudioStreamPlayer>("impact").Play();
                p.GetNode<AnimationPlayer>("Explosion/AnimationPlayer").Play("explosion");
                p.ChangeState(Player_v2.States2.DEAD);
            }
            else
            {
                b.GetNode<AudioStreamPlayer>("impact").Play();
                GetNode<Sprite>($"Player/Damage{_hits}").Show();
            }
        }

        if (body is PowerUp pUp)
        {
            p.GetNode<AudioStreamPlayer>("PowerUpCollected").Play();
            if (pUp.PowerUpType == "shield")
            {
                p.GetNode<Area2D>("Shield").Show();
                p.Shielded = true;
                p.ChangeState(Player_v2.States2.INVULNERABLE);    
            }
            else if (pUp.PowerUpType == "repair" && _hits > 0)
            {
                if (_hits == 1)
                {
                    GetNode<Sprite>($"Player/Damage{_hits}").Hide();
                    _hits--;
                }
                else if (_hits > 1)
                {
                    GetNode<Sprite>($"Player/Damage{_hits}").Hide();
                    _hits--;
                    GetNode<Sprite>($"Player/Damage{_hits}").Show();   
                }
            }
            
            pUp.QueueFree();
        }
    }
    
    private void _on_Player_Dead()
    {
        _playing = false;
        Reset();
        GetTree().ChangeScene("res://scenes/game_over/GameOverPage.tscn");
    }
 
    private void _on_PowerUpTimer_timeout()
    {
        PowerUp p = (PowerUp) PowerUpScene.Instance();
        GetNode<Node>("PowerUps").AddChild(p);
        p.LoadPowerUp();
        _powerUp = false;
    }
    
    private void _on_EnemySpawnTimer_timeout()
    {
        Print("enemy");
        PackedScene s = (PackedScene) ResourceLoader.Load(_enemyShips[_random.Next(0, _enemyShips.Count)]);
        Node e = s.Instance();
        AddChild(e);
    }
}
