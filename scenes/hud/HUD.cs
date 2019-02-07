using Godot;
using System;
using System.Collections.Generic;

public class HUD : CanvasLayer
{
    [Signal]
    delegate void StartGame();

    public bool Playing;
    private int _score;
    private Global _global;
    
    public override void _Ready()
    {
        // send start game signal
        _global = (Global) GetNode("/root/Global");
        EmitSignal("StartGame");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
//        GetNode<Label>("MarginContainer/HBoxContainer/HitsLabel").Text = _global.Hits.ToString();
    }

    public void ShowMessage(string message)
    {
        Label messageLabel = GetNode<Label>("MessageLabel");
        messageLabel.Text = message;
        messageLabel.Show();
        GetNode<Timer>("MessageTimer").Start();
    }

    public void UpdateScore(int value)
    {
        _score += value;
        GetNode<Label>("MarginContainer/HBoxContainer/ScoreLabel").Text = value.ToString();
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
