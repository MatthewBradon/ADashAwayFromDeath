using Godot;
using System;

public class spikes : Area2D
{
    public void _on_Area2D_body_entered(Node body)
    {
        if(body.Name == "Player"){
            GetTree().ChangeScene("res://Scenes/GameOver.tscn");    
        }
    }
}
