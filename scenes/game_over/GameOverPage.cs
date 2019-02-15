using Godot;
using System;

public class GameOverPage : CanvasLayer
{
//    private Global _global;

    public override void _Ready()
    {
//        _global = GetNode<Global>("/root/Global");
        GetNode<Label>("MarginContainer/GameOver").Text = "It's Game Over man! It's Game Over!";
        GetNode<Label>("MarginContainer2/FinalScore").Text = $"Final score: {Global.Score.ToString()}";
        Global.SetHighscore();
    }

    private void _on_Restart_pressed()
    {
        LoadStart();
    }
    
    private void _on_Quit_pressed()
    {
        GetTree().Quit();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Input.IsActionJustReleased("start"))
        {
            LoadStart();
        }
    }

    private void LoadStart()
    {
        GetTree().ChangeScene("res://scenes/start_page/StartPage2.tscn");
    }
}
