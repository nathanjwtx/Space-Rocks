using Godot;
using System;

public class GameOverPage : CanvasLayer
{

    public override void _Ready()
    {
        GetNode<Label>("MarginContainer/GameOver").Text = "It's Game Over man! It's Game Over!";
    }

    private void _on_Restart_pressed()
    {
        GetTree().ChangeScene("res://scenes/main/Main.tscn");
    }
}



