using Godot;
using System;

public class Global : Node
{
    private static  int _score;
    private static bool _hardcore;
    private static int _hits;

    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Input.IsActionJustReleased("exit"))
        {
            GetTree().Quit();
        }
    }

    public static int Hits { get => _hits; set => _hits += value; }
    
    public static int Score { get => _score; set => _score += value; }

    public static bool Hardcore { get => _hardcore; set => _hardcore = value; }

}
