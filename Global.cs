using Godot;
using System;

public class Global : Node
{
    private int _score;
    private bool _hardcore;
    private int _hits;

    public override void _Ready()
    {
        Hits = 0;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Input.IsActionJustReleased("exit"))
        {
            GetTree().Quit();
        }
    }

    public int Hits { get => _hits; set => _hits += value; }
    
    public int Score { get => _score; set => _score += value; }

    public bool Hardcore { get => _hardcore; set => _hardcore = value; }

//    public static void SetHardcore(bool value)
//    {
//        _hardcore = value;
//    }
}
