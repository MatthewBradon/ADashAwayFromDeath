using Godot;
using System;

public class LevelCountText : RichTextLabel
{
    private LevelCount levelCount;
    public override void _Ready()
    {
        levelCount = (LevelCount)GetNode("/root/LevelCountSingleton");
    }

    public override void _Process(float delta)
    {
        Text = "Level " + levelCount.getLevelCount().ToString();
    }
}
