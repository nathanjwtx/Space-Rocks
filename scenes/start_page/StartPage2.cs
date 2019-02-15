using Godot;
using System;

public class StartPage2 : CanvasLayer
{
    private string _mainPage = "res://scenes/main/Main.tscn";
//    private Global _global;
    
    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
//        _global = (Global) GetNode("/root/Global");
        GetNode<Label>("MarginContainer/HighScore").Text = $"Highscore: {Global.HighScore}";
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Input.IsActionPressed("start"))
        {
            Start();
        }
    }
    
    private void _on_sp2StartButton_pressed()
    {
        Start();
    }
    
    public void Start()
    {
        GetTree().ChangeScene(_mainPage);
    }

    private void _on_CheckButton_toggled(bool buttonPressed)
    {
        Global.Hardcore = buttonPressed;
    }
}
