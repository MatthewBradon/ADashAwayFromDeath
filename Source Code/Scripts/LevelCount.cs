using Godot;
using System;

public class LevelCount : Node
{
    private int levelCount = 1;
    private int currentHeight = 5;
    private float moveOffsetTime = 2.0f;
    public override void _Ready()
    {
        
    }
    public void incrementLevelCount()
    {
        levelCount++;
    }
    public int getLevelCount()
    {
        return levelCount;
    }
    public void resetLevelCount()
    {
        levelCount = 1;
    }
    public int getCurrentHeight()
    {
        return currentHeight;
    }
    public void setCurrentHeight(int height)
    {
        currentHeight = height;
    }
    public float getMoveOffsetTime()
    {
        return moveOffsetTime;
    }
    public void setMoveOffsetTime(float offsetTime)
    {
        moveOffsetTime = offsetTime;
    }
}
