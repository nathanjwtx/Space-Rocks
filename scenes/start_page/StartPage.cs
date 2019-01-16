using Godot;
using System;

public class StartPage : CanvasLayer
{
    
    private string _mainPage = "res://scenes/main/Main.tscn";

    public override void _Ready()
    {
        string credits = "Credits:\n" +
                         "Background images are courtesy of ESA/Hubble (www.spacetelescope.ord)\n" +
                         "Artwork by Chris Bradfield and Kenney (http://kenney.nl)\n" +
                         "Based on GDScript project code by Chris Bradfield";
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
