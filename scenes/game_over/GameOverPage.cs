using Godot;
using System;

public class GameOverPage : CanvasLayer
{
    private Global _global;

    public override void _Ready()
    {
        _global = GetNode<Global>("/root/Global");
        GetNode<Label>("MarginContainer/GameOver").Text = "It's Game Over man! It's Game Over!";
        GetNode<Label>("MarginContainer2/FinalScore").Text = $"Final score: {_global.Score.ToString()}";
    }

    private void _on_Restart_pressed()
    {
        GetTree().ChangeScene("res://scenes/main/Main.tscn");
    }
    
    private void _on_Quit_pressed()
    {
        GetTree().Quit();
    }
}
