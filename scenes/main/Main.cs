using Godot;
using static Godot.GD;
using System;

public class Main : Node
{

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        
    }

    private void _on_Player_Shoot(PackedScene bullet, Vector2 pos, float dir)
    {
        Print("bang");
        var b = (Bullet) bullet.Instance();
        b.Start(pos, dir);
        AddChild(b);
    }

}
