using Godot;
using System;

public class StartPage : CanvasLayer
{
    
    private string _startPage2 = "res://scenes/start_page/StartPage2.tscn";

    public override void _Ready()
    {
        string credits = "Credits:\n" +
                         "Background images are courtesy of ESA/Hubble (www.spacetelescope.ord)\n" +
                         "Artwork by me, Chris Bradfield or Kenney (http://kenney.nl)\n" +
                         "Based on GDScript project code by Chris Bradfield\n" +
                         "Sound effects by qubecad\n and GameDev Market" +
                         "Rocks In Space font: Rock Font by Jester Font Studio";
        GetNode<RichTextLabel>("MarginContainer/Credits").Text = credits;
        GetNode<AnimationPlayer>("AnimationPlayer").Play("ZoomFont");
    }

    private void _on_Timer_timeout()
    {
        GetTree().ChangeScene(_startPage2);
    }
    
    private void _on_AnimationPlayer_animation_finished(String anim_name)
    {
        GetNode<Timer>("Timer").Start();
    }
}
