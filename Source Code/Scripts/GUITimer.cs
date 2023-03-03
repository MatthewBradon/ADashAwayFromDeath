using Godot;
using System;

public class GUITimer : RichTextLabel
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	TimerText timerText;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timerText = (TimerText)GetNode("/root/TimerTextSingleton");
		timerText.startTimer();
	}
	public override void _Process(float delta)
	{
		Text = timerText.timer[0].ToString("00") + ":" + timerText.timer[1].ToString("00");
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
