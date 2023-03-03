using Godot;
using System;

public class Menu : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	Button startButton;
	VBoxContainer menuContainer;

	AudioStreamPlayer BackgroundMusic;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		menuContainer = GetNode<VBoxContainer>("MenuContainer");
		startButton = menuContainer.GetNode<Button>("StartButton");
		startButton.GrabFocus();

		BackgroundMusic = GetNode<AudioStreamPlayer>("/root/BackgroundMusic");
		BackgroundMusic.Stop();
	}
	public void _on_StartButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/MainScene.tscn");
	}

	public void _on_QuitButton_pressed()
	{
		GetTree().Quit();
	}

	public void _on_OptionButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/Options.tscn");
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
