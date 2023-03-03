using Godot;
using System;

public class milliSecTimer : Node
{
    private bool isRunning = false;
    public int timer = 0;
    public override void _Process(float delta)
    {
        if(isRunning)
        {
            updateTimer(delta);
        }
    }

    public void startTimer()
    {
        isRunning = true;
    }

    public void stopTimer()
    {
        isRunning = false;
    }

    public void resetTimer()
    {
        timer = 0;
    }

    public void updateTimer(float delta)
    {
        timer += (int)(delta * 1000);
    }
}
