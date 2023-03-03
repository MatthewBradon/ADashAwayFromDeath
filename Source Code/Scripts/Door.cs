using Godot;
using System;

public class Door : Area2D
{
    [Signal] public delegate void DoorOpen();
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    TimerText timerText;
    LevelCount levelCount;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        timerText = (TimerText)GetNode("/root/TimerTextSingleton");
        levelCount = (LevelCount)GetNode("/root/LevelCountSingleton");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_accept")){
            if(GetOverlappingBodies().Count > 0){
                timerText.stopTimer();
                levelCount.incrementLevelCount();
                GetTree().ChangeScene("res://Scenes/MainScene.tscn");
            }
        }
    }
}
