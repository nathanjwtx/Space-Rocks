using Godot;
using System;

public class StartPage : CanvasLayer
{
    private string _mainPage = "res://scenes/main/Main.tscn";

    public override void _Ready()
    {
        string credits = "Credits:\n" +
                         "Background images are courtesy of Wibble\n" +
                         "Based on GDScript code by Chris Bradfield";
        GetNode<RichTextLabel>("MarginContainer/HBoxContainer/Credits").Text = credits;

    }

    public void Start()
    {
        GD.Print("styasrt");
        GetTree().ChangeScene(_mainPage);
        
    }
    
    private void _on_StartButton_pressed()
    {
        Start();
    }
}
