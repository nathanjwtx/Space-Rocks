using Godot;
using System;
using System.Collections.Generic;

public class Rock : RigidBody2D
{
    [Signal]
    delegate void Boom();
    
    public Vector2 _screensize;
    private int _size;
    private float _radius;
    private float _scaleFactor = 0.2f;
    
    private List<string> _rocks = new List<string>
    {
        "res://assets/meteors/meteorGrey_big1.png",
        "res://assets/meteors/meteorGrey_big2.png",
        "res://assets/meteors/meteorGrey_big3.png",
        "res://assets/meteors/meteorGrey_big4.png"
    };

    public override void _Ready()
    {

    }

    public void Start(Vector2 pos, Vector2 velocity, int size, int image)
    {
        Position = pos;
        _size = size;
        var mass = size * 1.5;
        Sprite rock = GetNode<Sprite>("RockSprite");
        rock.Texture = (Texture) GD.Load(_rocks[image]);
        rock.Scale = new Vector2(1.5f, 1.5f) * _scaleFactor * size;
        _radius = rock.Texture.GetSize().x / 2 * _scaleFactor * size;
        CircleShape2D collisionShape = new CircleShape2D();
        collisionShape.Radius = _radius;
        GetNode<CollisionShape2D>("CollisionShape2D").Shape = collisionShape;
        LinearVelocity = velocity;
        Random r = new Random();
        AngularVelocity = r.Next(-15, 15) / 10f;
        GetNode<Sprite>("Explosion").Scale = new Vector2(0.75f, 0.75f) * size;
    }

    public override void _IntegrateForces(Physics2DDirectBodyState physics_state)
    {
        base._IntegrateForces(physics_state);
        Transform2D xform = physics_state.GetTransform();
        Vector2 origin;
        Transform2D wibble;
        if (xform.origin.x > _screensize.x + _radius)
        {
            origin = new Vector2(0 - _radius, xform.origin.y);
            wibble = new Transform2D(xform.x, xform.y, origin);
            physics_state.SetTransform(wibble);
        }
        if (xform.origin.x < 0 - _radius)
        {
            origin = new Vector2(_screensize.x + _radius, xform.origin.y);
            wibble = new Transform2D(xform.x, xform.y, origin);
            physics_state.SetTransform(wibble);
        }

        if (xform.origin.y > _screensize.y + _radius)
        {
            origin = new Vector2(xform.origin.x, 0 - _radius);
            wibble = new Transform2D(xform.x, xform.y, origin);
            physics_state.SetTransform(wibble);
        }

        if (xform.origin.y < 0 - _radius)
        {
            origin = new Vector2(xform.origin.x, _screensize.y + _radius);
            wibble = new Transform2D(xform.x, xform.y, origin);
            physics_state.SetTransform(wibble);
        }
    }

    public void Explode(bool player)
    {
        Layers = 0;
        GetNode<Sprite>("RockSprite").Hide();
        GetNode<Sprite>("Explosion").Show();
        GetNode<AnimationPlayer>("Explosion/AnimationPlayer").Play("explosion");
        GetNode<AudioStreamPlayer>("Explode").Play();
        EmitSignal("Boom", _size, _radius, Position, LinearVelocity, player);
        LinearVelocity = new Vector2();
        AngularVelocity = 0;
    }
    
    private void _on_AnimationPlayer_animation_finished(String anim_name)
    {
        QueueFree();
    }
}



