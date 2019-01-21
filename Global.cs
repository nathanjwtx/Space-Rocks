using Godot;
using System;

public class Global : Node
{
    private int _score;
    private static bool _hardcore;

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

    public int Score => _score;

    public static bool HardCore => _hardcore;

    public void UpdateScore(int value)
    {
        _score += value;
    }

    public static void SetHardcore(bool value)
    {
        _hardcore = value;
    }
}
