using Godot;
using System;

public class StartPage2 : CanvasLayer
{
    private string _mainPage = "res://scenes/main/Main.tscn";
    
    

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Input.IsActionPressed("start"))
        {
            Start();
        }
    }
    
    private void _on_StartButton_pressed()
    {
        Start();
    }
    
    public void Start()
    {
        GetTree().ChangeScene(_mainPage);
    }

    private void _on_CheckButton_toggled(bool buttonPressed)
    {
        GD.Print(buttonPressed);
        Global.SetHardcore(true);
    }
}



