using Godot;
using static Godot.GD;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    private HUD _hud;
    private Node _enemyNode;
    
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
//        "res://scenes/enemies/Enemy_Red.tscn",
//        "res://scenes/enemies/Enemy_Green.tscn",
        "res://scenes/enemies/Enemy_Blue.tscn",
//        "res://scenes/enemies/Enemy_Yellow.tscn"
    };
        
    
    public override void _Ready()
    {
        _global = (Global) GetNode("/root/Global");
        _random = new Random();
        _screenSize = GetViewport().GetVisibleRect().Size;
        _prevBackground = _random.Next(0, 10);
        SetBackground(_prevBackground);
        _newGame = true;
        _hud = GetNode<HUD>("HUD");
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
//        _global.Hits = 0;
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
        Player_v2 player = GetNode<Player_v2>("Player");
        GetNode<Sprite>("Player/Ship").Show();
        GetNode<Sprite>("Player/Engine").Show();
        player.Start();
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
            int rand;
            do
            {
                rand = _random.Next(0, 10);
            } while (_prevBackground == rand);
            _prevBackground = rand;
            SetBackground(rand);                        
        }
        _newGame = false;
        Player_v2 player = GetNode<Player_v2>("Player");
        if (!_global.Hardcore)
        {
            _hits = 0;
//            _global.Hits = 0;
            player.GetNode<Sprite>("Damage1").Hide();
            player.GetNode<Sprite>("Damage2").Hide();
            player.GetNode<Sprite>("Damage3").Hide();            
        }
        GetNode<HUD>("HUD").ShowMessage($"Wave {_level + 1}");
        for (int i = 0; i < _level + 1; i++)
        {
            SpawnRock(3);
        }
        Timer timer = GetNode<Timer>("PowerUpTimer");
        timer.WaitTime = _random.Next(3, 20);
        timer.Start();
    }

    private void SetBackground(int level)
    {
        _background.Texture = (Texture) Load(backgrounds[level]);
        var xScale = _screenSize.x / _background.GetTexture().GetWidth();
        var yScale = _screenSize.y / _background.GetTexture().GetHeight();
        _background.Position = new Vector2(0, 0);
        _background.Scale = new Vector2(xScale, yScale);
    }

    private void SpawnRock(int size)
    {
        GetNode<PathFollow2D>("RockPath/RockSpawn").SetOffset(_random.Next(0, int.MaxValue));
        Vector2 pos = GetNode<PathFollow2D>("RockPath/RockSpawn").Position;
        Vector2 temp = new Vector2(1, 0);
        Vector2 velocity = temp.Rotated(_random.Next(0, 360)) * _random.Next(100, 150);
        SpawnRock(size, pos, velocity);
    }
    
    private void SpawnRock(int size, Vector2 pos, Vector2 velocity)
    {
        var rock = (Rock) RockScene.Instance();
        rock._screensize = _screenSize;
        rock.Start(pos, velocity, size, _random.Next(0, 4));
        GetNode<Node>("Rocks").AddChild(rock);
        rock.Connect("Boom", this, "_on_Rock_Boom");
    }

    
    private void _on_Rock_Boom(int size, float radius, Vector2 pos, Vector2 vel, bool playerShot)
    {
        if (size > 1)
        {
            for (int i = -1; i < 2; i += 2)
            {
                Vector2 dir = (pos - GetNode<Player_v2>("Player").Position).Normalized().Tangent() * i;
                Vector2 newPos = pos + dir * radius;
                Vector2 newVel = dir * vel.Length() * 1.5f;
                CallDeferred("SpawnRock", size - 1, newPos, newVel);
            }
        }

        if (!playerShot) return;
        UpdateScore(1);
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
        Player_v2 player = GetNode<Player_v2>("Player");
        
        Print(body.GetType().Name);
        if (body is Enemy_Bullet enemyBullet)
        {
            Print("eb");
            if (enemyBullet.BulletType == "blue")
            {
                GD.Print("blue bullet");
            }
        }
        
        if (body is Rock rock)
        {
            _hits++;
//            _global.Hits = 1;
            if (_hits >= 4)
//            if (_global.Hits >= 4)
            {
                player.GetNode<Sprite>("Damage1").Hide();
                player.GetNode<Sprite>("Damage2").Hide();
                player.GetNode<Sprite>("Damage3").Hide();
                player.GetNode<Sprite>("Explosion").Show();
                rock.GetNode<AudioStreamPlayer>("impact").Play();
                player.GetNode<AnimationPlayer>("Explosion/AnimationPlayer").Play("explosion");
                player.ChangeState(Player_v2.States2.DEAD);
            }
            else
            {
                rock.GetNode<AudioStreamPlayer>("impact").Play();
//                GetNode<Sprite>($"Player/Damage{_global.Hits}").Show();
                GetNode<Sprite>($"Player/Damage{_hits}").Show();
            }
        }

        if (body is PowerUp pUp)
        {
            player.GetNode<AudioStreamPlayer>("PowerUpCollected").Play();
            if (pUp.PowerUpType == "shield")
            {
                player.GetNode<Area2D>("Shield").Show();
                player.Shielded = true;
                player.ChangeState(Player_v2.States2.INVULNERABLE);    
            }
            else if (pUp.PowerUpType == "repair" && _global.Hits > 0)
            {
                if (_global.Hits == 1)
//                if (_hits == 1)
                {
                    GetNode<Sprite>($"Player/Damage{_global.Hits}").Hide();
                    _global.Hits = -1;
//                    GetNode<Sprite>($"Player/Damage{_hits}").Hide();
//                    _hits--;
                }
                else if (_global.Hits > 1)
//                else if (_hits > 1)
                {
                    GetNode<Sprite>($"Player/Damage{_global.Hits}").Hide();
                    _global.Hits = -1;
                    GetNode<Sprite>($"Player/Damage{_global.Hits}").Show();   
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
        PowerUp power = (PowerUp) PowerUpScene.Instance();
        GetNode<Node>("PowerUps").AddChild(power);
        power.LoadPowerUp();
        _powerUp = false;
    }
    
    private void _on_EnemySpawnTimer_timeout()
    {
        // Create path and pathfollow
        int paths = GetNode<Node>("EnemyPaths").GetChildCount();
        Path2D randomPath = GetNode<Path2D>($"EnemyPaths/path{_random.Next(1, paths + 1)}");
        Path2D path = GetNode<Path2D>("Path2D");
        path.SetCurve(randomPath.Curve);
        PathFollow2D pathFollow2D = new PathFollow2D();
        GetNode<Path2D>("Path2D").AddChild(pathFollow2D);
        pathFollow2D.Rotate = true;
        pathFollow2D.Loop = false;

        // Spawn enemy and send pathfollow to BaseEnemy for movement purposes
        PackedScene s = (PackedScene) ResourceLoader.Load(_enemyShips[_random.Next(0, _enemyShips.Count)]);
        _enemyNode = s.Instance();
        if (_enemyNode != null && _enemyNode.HasMethod("SetupPath"))
        {
            _enemyNode.Call("SetupPath", pathFollow2D);
        }
        pathFollow2D.AddChild(_enemyNode);

        // Connect enemy explosion signal
        _enemyNode.Connect("EnemyBoom", this, "_on_EnemyBoom");
    }

    private void _on_EnemyBoom(int score)
    {
        UpdateScore(score);
    }

    private void UpdateScore(int score)
    {
        _global.Score = score;
        _hud.UpdateScore(_global.Score);        
    }
}
