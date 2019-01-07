using Godot;
using System;
using System.Collections.Generic;

public class HUD : CanvasLayer
{
    [Signal]
    delegate void StartGame();

    public List<string> LivesCounter = new List<string>
    {
        "MarginContainer/HBoxContainer/LivesCounter/L1",
        "MarginContainer/HBoxContainer/LivesCounter/L2",
        "MarginContainer/HBoxContainer/LivesCounter/L3"
    };
    
    public override void _Ready()
    {

    }

    private void ShowMessage(string message)
    {
        Label messageLabel = GetNode<Label>("MessageLabel");
        messageLabel.Text = message;
        messageLabel.Show();
        GetNode<Timer>("MessageTimer").Start();
    }

    private void UpdateScore(int value)
    {
        GetNode<Label>("MarginContainer/HBoxContainer/ScoreLabel").Text = value.ToString();
    }

    private void UpdateLives(int value)
    {
        for (var i = 0; i < LivesCounter.Count; i++)
        {
            GetNode<TextureRect>(LivesCounter[i]).Visible = value > i;
        }
    }

    private void GameOver()
    {
        ShowMessage("It's Game Over man! It's Game Over!");
    }
    
    private void _on_StartButton_pressed()
    {
        GetNode<Button>("StartButton").Hide();
        EmitSignal("StartGame");
    }
    
    private void _on_MessageTimer_timeout()
    {
        GetNode<Label>("MessageLabel").Hide();
        GetNode<Button>("StartButton").Show();
        EmitSignal("StartGame");
    }
    
//    public override void _Process(float delta)
//    {
//        // Called every frame. Delta is time since last frame.
//        // Update game logic here.
//        
//    }
}
