using Godot;
using System;

public class Global : Node
{
    private int _score;

    public override void _Ready()
    {

    }

    public int Score => _score;

    public void UpdateScore(int value)
    {
        _score += value;
    }

}
