using Godot;
using System;

public class TimerText : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private bool isRunning = false;
    public int[] timer = new int[3];
    // Called when the node enters the scene tree for the first time.s
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
        timer = new int[3];
    }

    public void updateTimer(float delta)
    {
        timer[2] += (int)(delta * 1000);
        if(timer[2] >= 1000)
        {
            timer[1]++;
            timer[2] = 0;
        }
        if(timer[1] >= 60)
        {
            timer[0]++;
            timer[1] = 0;
        }
    }
}
