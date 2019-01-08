using Godot;
using System;

public class Start : CanvasLayer
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";

    public override void _Ready()
    {
        string credits = "Credits:\n" +
                         "Background images are courtesy of Wibble\n" +
                         "Based on GDScript code by Chris Bradfield";
        GetNode<RichTextLabel>("MarginContainer/HBoxContainer/RichTextLabel").Text = credits;

    }

//    public override void _Process(float delta)
//    {
//        // Called every frame. Delta is time since last frame.
//        // Update game logic here.
//        
//    }
}
