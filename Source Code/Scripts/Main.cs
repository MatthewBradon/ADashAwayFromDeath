using Godot;
using System;


public class Main : Node2D
{
	private bool generationReady = true;
	private PackedScene playerScene;
	private PackedScene reaperScene;
	private KinematicBody2D player;
	private KinematicBody2D reaper;
	private PackedScene RandomLevelGenerationScene;
	private RandomLevelGeneration levelGenerator;

	private AudioStreamPlayer backgroundMusic;
	
	public override void _Ready()
	{
		backgroundMusic = GetNode<AudioStreamPlayer>("/root/BackgroundMusic");
		backgroundMusic.Play();
	}

	public override void _Process(float delta)
	{
		if(generationReady){
			foreach(Node child in GetChildren()){
				child.QueueFree();
			}
		var timerText = (TimerText)GetNode("/root/TimerTextSingleton");

			RandomLevelGenerationScene = (PackedScene)ResourceLoader.Load("res://Scenes/RandomLevelGeneration.tscn");
			levelGenerator = RandomLevelGenerationScene.Instance() as RandomLevelGeneration;
			levelGenerator.Connect("mapDrawn", this, "_on_RandomLevelGeneration_mapDrawn");
			AddChild(levelGenerator);

			generationReady = false;
		}
		
	}
	public void _on_RandomLevelGeneration_mapDrawn(){
		loadPlayer();
		loadReaper();
	}

	public void NextLevel(){
		generationReady = true;
	}

	public void loadPlayer(){
		playerScene = (PackedScene)ResourceLoader.Load("res://Scenes/Player.tscn");
		KinematicBody2D instance = playerScene.Instance() as KinematicBody2D;
		player = instance;
		//instance.Position = new Vector2((levelGenerator.startPosition + 1) * RandomLevelGeneration.ROOM_WIDTH - 128, RandomLevelGeneration.ROOM_HEIGHT/2);
		instance.Position = levelGenerator.GetNode<Node2D>("rooms").GetNode<Node2D>("start").GetNode<Node2D>("spawnPoint").GlobalPosition;
		AddChild(instance);
	}

	public void loadReaper(){
		reaperScene = (PackedScene)ResourceLoader.Load("res://Scenes/Reaper.tscn");
		KinematicBody2D instance = reaperScene.Instance() as KinematicBody2D;
		reaper = instance;
		//instance.Position = new Vector2((levelGenerator.startPosition + 1) * 256 - 128, 176/2);
		instance.Position = player.Position;
		AddChild(instance);

	}
}
