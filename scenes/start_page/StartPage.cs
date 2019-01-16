using Godot;
using System;

public class StartPage : CanvasLayer
{
    
    private string _mainPage = "res://scenes/main/Main.tscn";

    public override void _Ready()
    {
        string credits = "Credits:\n" +
                         "Background images are courtesy of ESA/Hubble (www.spacetelescope.ord)\n" +
                         "Artwork by me, Chris Bradfield or Kenney (http://kenney.nl)\n" +
                         "Based on GDScript project code by Chris Bradfield\n" +
                         "Sound effects by qubecad";
        GetNode<RichTextLabel>("MarginContainer/HBoxContainer/Credits").Text = credits;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Input.IsActionPressed("start"))
        {
            Start();
        }
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
