using Godot;
using System;
using System.Collections.Generic;

public class HUD : CanvasLayer
{
    [Signal]
    delegate void StartGame();
    
    public override void _Ready()
    {
        // send start game signal
        EmitSignal("StartGame");
    }

    public void ShowMessage(string message)
    {
        Label messageLabel = GetNode<Label>("MessageLabel");
        messageLabel.Text = message;
        messageLabel.Show();
        GetNode<Timer>("MessageTimer").Start();
    }

    public void UpdateScore()
    {
        GetNode<Label>("MarginContainer/HBoxContainer/ScoreLabel").Text = Global.Score.ToString();
    }

    public void GameOver()
    {
        ShowMessage("It's Game Over man! It's Game Over!");
    }
    
    private void _on_StartButton_pressed()
    {
        GetNode<TextureButton>("StartButton").Hide();
        EmitSignal("StartGame");
    }
    
    private void _on_MessageTimer_timeout()
    {
        Label messageLabel = GetNode<Label>("MessageLabel");
        messageLabel.Hide();
        messageLabel.Text = "";
    }
}
